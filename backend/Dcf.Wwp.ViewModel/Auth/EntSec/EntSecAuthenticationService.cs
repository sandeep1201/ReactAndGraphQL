using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using DCF.Common.Logging;
using Dcf.Wwp.Api.Core.EntSec;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Helpers;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Core.Threading;
using Newtonsoft.Json;


// ReSharper disable once CheckNamespace
namespace Dcf.Wwp.Api.Library.Auth
{
    /// <summary>
    /// Enum of the status of the auth request.  
    /// </summary>
    public enum AuthRequestStatus
    {
        [Description("EntSec Service is returning null or exception")]
        CwaError,

        [Description("Authentication Successful")]
        AuthenticationSuccess,

        [Description("Logon Authentication Failed for the user")]
        AuthenticationFail,

        [Description("The User is not in the specified AD group")]
        UserNotInGroup,

        [Description("The token is expired")] TokenIsExpired
    }

    public class EntSecAuthenticationService : IAuthenticationService
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IRepository    _repo;
        private static   IEntSecConfig  _esConfig;
        private readonly ILog           _logger = LogProvider.GetLogger(typeof(EntSecAuthenticationService));

        // see http://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient HttpClient { get; set; }

        public EntSecAuthenticationService(ITokenProvider tokenProvider, IRepository repository, IEntSecConfig esConfig)
        {
            _tokenProvider = tokenProvider;
            _repo          = repository;
            _esConfig      = esConfig;
        }

        static void UpdateConfifgurationForEnvironment(bool useInteropKey = false)
        {
            var baseAddress = _esConfig.Endpoint;
            var appKey      = useInteropKey ? _esConfig.InteropApplicationKey : _esConfig.ApplicationKey;

            HttpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
            HttpClient.DefaultRequestHeaders.Add("ApplicationKey", appKey);
        }

        private const string PREFIX_INFOS = "_ifo_";
        private const string PREFIX_ROLES = "_rle_WWP_";

        public string GenerateToken(string username, params Claim[] privateClaims)
        {
            return _tokenProvider.GenerateToken(username, privateClaims);
        }

        public virtual IUserProfile AuthenticateUser(string username, string password, string environment, DateTime? cdoDate = null)
        {
            var userProfile = DoAuthenticateUser(username, password, environment);

            return userProfile;
        }

        public IEnumerable<IRole> GetRoles()
        {
            return _repo.AuthorizationRoles();
        }

        public IEnumerable<string> GetAllUserames()
        {
            return _repo.GetWorkerUsernames();
        }

        public IEnumerable<IOffice> GetAllOffices()
        {
            return _repo.GetOffices();
        }

        public bool ValidateToken(string authToken, string environment)
        {
            UpdateConfifgurationForEnvironment( true);

            var request = new HttpRequestMessage()
                          {
                              RequestUri = new Uri(HttpClient.BaseAddress, "enterprise/securityservices/authenticatetoken"),
                              Method     = HttpMethod.Post
                          };

            request.Headers.Add("AuthenticatedToken", authToken);
            var response = AsyncHelper.RunSync(() => HttpClient.SendAsync(request));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                //Log.As.Error("EntSec authenticatetoken StatusCode={0}", response.StatusCode);
                return false;
            }

            var bodyRaw = AsyncHelper.RunSync(() => response.Content.ReadAsStringAsync());
            var body    = bodyRaw.AsJson();

            if (body.MessageCode == null)
            {
                //Log.As.Warn("EntSec authorizetoken MessageCode NULL");
                return false;
            }

            return body.MessageCode == "SUCCESS" && (bool) body.IsAuthenticated;
        }

        //public virtual Boolean ValidateToken(String token)
        //{
        //    var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //    JwtSecurityTokenHandler.ValidateToken()
        //}


        /// <summary>
        /// Authenticates the user/password combination by reading the Active Directory details from config settings
        /// and returns the CWA response.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="environment"></param>
        /// <returns>true if the user authenticates, false otherwise</returns>
        internal IUserProfile DoAuthenticateUser(string userId, string password, string environment)
        {
            UpdateConfifgurationForEnvironment();

            IUserProfile userProfile = new UserProfile();

            //string appKey = WebConfigurationManager.AppSettings["Security.AppKey"];
            //string baseAddress = WebConfigurationManager.AppSettings["Security.Server"];
            string authToken;

            //Audit.As.Info("AuthenticateUser Attempt");
            //Log.As.Trace("UserAuthenticationService:AuthenticateUser called for '{0}'", userId);

            var request = new HttpRequestMessage()
                          {
                              RequestUri = new Uri(HttpClient.BaseAddress, "enterprise/securityservices/authenticateuser"),
                              Method     = HttpMethod.Post
                          };

            request.Headers.Add("Username", userId);
            request.Headers.Add("Password", password);


            // Always set the AdUserId field to what was passed in.
            userProfile.WamsUserId = userId;

            var request1 = request;
            var response = AsyncHelper.RunSync(() => HttpClient.SendAsync(request1));

            if (response.StatusCode != HttpStatusCode.OK)
            {
                UpdateUserProfileForError(userProfile, AuthRequestStatus.CwaError, "Bad HTTP status from EntSec: {0}", response.StatusCode);
                return userProfile;
            }

            var response1 = response;
            var rawBody   = AsyncHelper.RunSync(() => response1.Content.ReadAsStringAsync());

            var body = rawBody.AsJson();
            if (body.MessageCode == null)
            {
                UpdateUserProfileForError(userProfile, AuthRequestStatus.CwaError, "MessageCode missing from response");
                return userProfile;
            }
            else
            {
                if (body.MessageCode == "SUCCESS")
                {
                    authToken = body.AuthenticatedToken;
                    if (string.IsNullOrEmpty(authToken))
                    {
                        UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "No Authentication Token for user");
                        return userProfile;
                    }

                    userProfile.IsAuthorized = true;
                    userProfile.Wiuid        = body.UserId;
                    userProfile.Status       = AuthRequestStatus.AuthenticationSuccess.ToString();
                    userProfile.ErrorMessage = EnumHelpers.GetEnumDescription(AuthRequestStatus.AuthenticationSuccess);
                }
                else
                {
                    //Audit.As.Info("AuthenticateUser Failed");

                    if (body.MessageCode == "INVALIDUSERNAME")
                        UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Invalid username");
                    else
                        if (body.MessageCode == "INVALIDCREDENTIALS")
                            UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Invalid credentials");
                        else
                            if (body.MessageCode == "INVALIDPASSWORD")
                                UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Invalid password");
                            else
                                if (body.MessageCode == "ACCESSDENIED")
                                    UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Access denied");
                                else
                                    if (body.MessageCode == "UNAUTHORIZED")
                                        UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "User unauthorized");
                                    else
                                        UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "User authentication failed");

                    // If the user didn't auth, return the profile here.
                    return userProfile;
                }
            }

            userProfile = Authorizetoken(authToken, userProfile, environment);

            return userProfile;
        }

        public IUserProfile Authorizetoken(string authToken, IUserProfile userProfile, string environment)
        {
            UpdateConfifgurationForEnvironment();

            dynamic body;
            var request = new HttpRequestMessage()
                          {
                              RequestUri = new Uri(HttpClient.BaseAddress, "enterprise/securityservices/authorizetoken"),
                              Method     = HttpMethod.Post
                          };

            request.Headers.Add("AuthenticatedToken", authToken);
            var response = AsyncHelper.RunSync(() => HttpClient.SendAsync(request));


            if (response.StatusCode != HttpStatusCode.OK)
            {
                //Log.As.Warn("EntSec authorizetoken StatusCode={0}", response.StatusCode);
                UpdateUserProfileForError(userProfile, AuthRequestStatus.CwaError, "Bad HTTP status on authorizetoken from EntSec: {0}", response.StatusCode);
                return userProfile;
            }

            //Log.As.Trace("EntSec authorizetoken StatusCode={0}", response.StatusCode);

            var bodyRaw = AsyncHelper.RunSync(() => response.Content.ReadAsStringAsync());
            body = bodyRaw.AsJson();

            if (body.MessageCode == null)
            {
                //Log.As.Warn("EntSec authorizetoken MessageCode NULL");
                UpdateUserProfileForError(userProfile, AuthRequestStatus.CwaError, "MessageCode missing on authorizetoken from response");
                return userProfile;
            }
            else
            {
                //Log.As.Trace("EntSec authorizetoken MessageCode={0}", body.MessageCode);

                if (body.MessageCode == "SUCCESS")
                {
                    userProfile.IsAuthorized = true;
                    userProfile.Status       = AuthRequestStatus.AuthenticationSuccess.ToString();
                    userProfile.ErrorMessage = EnumHelpers.GetEnumDescription(AuthRequestStatus.AuthenticationSuccess);

                    try
                    {
                        var infos                                                 = FilterClaimsIntoDictionary(body.ClaimList, PREFIX_INFOS);
                        if (infos.ContainsKey("lastlogon")) userProfile.LastLogon = Convert.ToDateTime(infos["lastlogon"]);
                        if (infos.ContainsKey("email")) userProfile.Email         = infos["email"];
                        userProfile.FirstName = infos["enterprisefirstname"];
                        userProfile.LastName  = infos["enterpriselastname"];
                        if (infos.ContainsKey("enterprisemiddleinitial")) userProfile.MiddleInitial = infos["enterprisemiddleinitial"];
                        userProfile.OrganizationCode = infos["primaryoffice_WWP"];
                        if (infos.ContainsKey("primarymainframeid_WWP"))
                            userProfile.MainFrameId = infos["primarymainframeid_WWP"]?.ToUpper();
                        if (string.IsNullOrWhiteSpace(userProfile.MainFrameId))
                            userProfile.MainFrameId = _repo.GetFnMFId()?.ToUpper();
                        else
                        {
                            if (userProfile.MainFrameId.Length > 6)
                                // We can only have 6 character mainframe IDs.
                                userProfile.MainFrameId = userProfile.MainFrameId.Substring(0, 6);
                        }
                    }
                    catch
                    {
                        var jsonUserProfile = JsonConvert.SerializeObject(userProfile);
                        _logger.Error($"KeyNotFoundException occured for user:{jsonUserProfile}");
                    }

                    userProfile.Roles     = FilterClaimsIntoList(body.ClaimList, PREFIX_ROLES).ToArray();
                    userProfile.RoleNames = _repo.AuthorizationRoles(userProfile.Roles).Select(x => x.Name).ToArray();

                    userProfile.EntSecToken = authToken;
                }
               
                else
                {
                    //Audit.As.Info("AuthenticateUser Failed");

                    if (body.MessageCode == "EXPIRED")
                        UpdateUserProfileForError(userProfile, AuthRequestStatus.TokenIsExpired, "Token has expired");
                    else
                        if (body.MessageCode == "LDAPERROR")
                            UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Error connecting to LDAP server, contact system administrator");
                        else
                            if (body.MessageCode == "CONNECTIONERROR")
                                UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Error connecting to database server");
                            else
                                if (body.MessageCode == "INTERNALERROR")
                                    UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Internal error, contact system administrator");
                                else
                                    if (body.MessageCode == "ACCESSDENIED")
                                        UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "The token user is not valid in the CWA ecosystem");
                                    else
                                        if (body.MessageCode == "INVALIDSSO")
                                            UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "Origin or Relying party/application is not participating in SSO");
                                        else
                                            UpdateUserProfileForError(userProfile, AuthRequestStatus.AuthenticationFail, "User authentication failed");

                    return userProfile;
                }
            }

            //Audit.As.Info("AuthenticateUser Success");

            return userProfile;
        }

        public AuthRequestStatus AuthenticateAPIKey(string key)
        {
            if (key.StartsWith("EntSec", StringComparison.InvariantCultureIgnoreCase))
            {
                return AuthRequestStatus.AuthenticationSuccess;
            }
            else
                if (key.StartsWith("ExpiredKey"))
                {
                    return AuthRequestStatus.TokenIsExpired;
                }
                else
                {
                    return AuthRequestStatus.AuthenticationFail;
                }
        }

        private static List<string> FilterClaimsIntoList(dynamic claimList, string prefix)
        {
            var roles = new List<string>();

            foreach (var dict in claimList)
            {
                if (dict.Key != null)
                {
                    if (dict.Key is string key && key.StartsWith(prefix))
                        roles.Add(key.Remove(0, prefix.Length));
                }
            }

            return roles;
        }


        private static Dictionary<string, string> FilterClaimsIntoDictionary(dynamic claimList, string prefix)
        {
            var values = new Dictionary<string, string>();

            foreach (var dict in claimList)
            {
                if (dict.Key != null)
                {
                    if (dict.Key is string key && key.StartsWith(prefix))
                        values.Add(key.Remove(0, prefix.Length), dict.Value);
                }
            }

            return values;
        }

        private static void UpdateUserProfileForError(IUserProfile userProfile, AuthRequestStatus status, string format, params object[] args)
        {
            string errorMessage = string.Format(format, args);
            //Log.As.Warn(errorMessage);
            userProfile.ErrorMessage = errorMessage;
            userProfile.IsAuthorized = false;
            userProfile.Status       = status.ToString();
        }
    }
}
