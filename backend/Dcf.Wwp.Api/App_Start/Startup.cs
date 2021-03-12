using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dcf.Wwp.Api.ActionFilters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using log4net;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Sinks.MSSqlServer;
using Dcf.Wwp.Api.Common;
using Dcf.Wwp.Api.Core;
using Dcf.Wwp.Api.Errors;
using Dcf.Wwp.Api.Library.Auth;
using Dcf.Wwp.Api.Library.Auth.Handlers;
using Dcf.Wwp.Api.Library.Auth.Policies;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Logging;
using Dcf.Wwp.Api.Library.Model;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Api.Middleware;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.ConnectedServices.FileUpload;
using Dcf.Wwp.ConnectedServices.GoogleApi;
using Dcf.Wwp.ConnectedServices.Logging;
using Dcf.Wwp.ConnectedServices.Mci;
using Dcf.Wwp.ConnectedServices.Shared;
using Dcf.Wwp.ConnectedServices.Tableau;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.DataAccess.Contexts;
using Dcf.Wwp.DataAccess.Infrastructure;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Dcf.Wwp.Model.Services;
using DCF.Common;
using DCF.Common.Configuration;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using Dcf.Wwp.Api.Library.Domains.EmergencyAssistance;
using Dcf.Wwp.Api.Library.Rules.Infrastructure;
using Dcf.Wwp.ConnectedServices.Finalist;
using Dcf.Wwp.TelerikReport.Library.Domain;
using Dcf.Wwp.TelerikReport.Library.Interface;
using IAuthenticationService = Dcf.Wwp.Api.Library.Auth.IAuthenticationService;
using AuthenticationMiddleware = Dcf.Wwp.Api.Middleware.AuthenticationMiddleware;

namespace Dcf.Wwp.Api
{
    public class Startup
    {
        #region Properties

        private static readonly ILog _log = LogManager.GetLogger("Dcf.Wwp.Api");

        public static           StringBuilder        sb = new StringBuilder();
        private static readonly StringWriter         sw = new StringWriter(sb);
        private readonly        SymmetricSecurityKey _signingKey;
        public static           LoggingLevelSwitch   LevelSwitch   { get; set; } = new LoggingLevelSwitch();
        public                  IConfigurationRoot   Configuration { get; }

        private DateTime _startTime { get; set; }
        private DateTime _endTime   { get; set; }

        #endregion

        #region Methods

        public Startup(IHostingEnvironment env)
        {
            _startTime = DateTime.Now;

            StartupLogger.Configure();

            _log.Info("BEGIN STARTUP LOG ".PadRight(45, '*'));
            _log.Info("Dcf.Wwp.Api Starting");

            _log.Info($"API version {Program.Version.Major}.{Program.Version.Minor}.{Program.Version.Build}.{Program.Version.Revision}");

            Configuration = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json",                        true, true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                            .AddJsonFile($"connectedServices.json",                 true)
                            .AddEnvironmentVariables()
                            .Build();

            var secretKey = Environment.GetEnvironmentVariable("TokenSecretKey");

            if (string.IsNullOrEmpty(secretKey))
            {
                secretKey = "A19ACB534DC4A4BD5FA16AF2B7CFB7BB1D42B48D5CF7EF1A5D72BCA8EF";
            }

            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // This method gets called by the runtime. Use this method to add services to the container.

            _log.Info("".PadRight(25, '-'));
            _log.Info("Configuring .NET pipeline services");

            _log.Debug("\t- adding CORS");

            services.AddCors(options => options.AddPolicy("AllowAll", builder => { builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); }));

            _log.Debug("\t- adding JsonSerializer");

            var jsonSerializerSettings = new JsonSerializerSettings
                                         {
                                             ContractResolver     = new CamelCasePropertyNamesContractResolver(),
                                             NullValueHandling    = NullValueHandling.Include,
                                             DateFormatHandling   = DateFormatHandling.IsoDateFormat,
                                             DateTimeZoneHandling = DateTimeZoneHandling.Local
                                         };

            // Add framework services.
            // the following is for Core < 2.0 to 2.1.x 
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            services.AddSingleton(manager);

            _log.Debug("\t- adding MVC services with JSON serializer options");

            services.AddMvc()
                    .AddJsonOptions(o =>
                                    {
                                        o.SerializerSettings.ContractResolver     = jsonSerializerSettings.ContractResolver;
                                        o.SerializerSettings.NullValueHandling    = jsonSerializerSettings.NullValueHandling;
                                        o.SerializerSettings.DateFormatHandling   = jsonSerializerSettings.DateFormatHandling;
                                        o.SerializerSettings.DateTimeZoneHandling = jsonSerializerSettings.DateTimeZoneHandling;
                                    });

            _log.Debug("\t- adding Routing table: options.LowercaseUrls = true;");
            services.AddRouting(options => { options.LowercaseUrls = true; });

            _log.Debug("\t- adding Memory Cache");
            services.AddMemoryCache();

            services.AddSingleton<IReadOnlyCollection<ICountyAndTribe>>(ctx =>
                                                                        {
                                                                            var db = ctx.GetService<WwpEntities>();

                                                                            var ro = (db.CountyAndTribes
                                                                                        .Where(i => !i.IsDeleted)
                                                                                        .OrderBy(i => i.CountyName)
                                                                                        .ToList().AsReadOnly());

                                                                            return ro;
                                                                        });

            // Add other DI stuff here.
            //_log.Info("Registering components with DI container");

            //_log.Debug("\t- AddSingleton(Configuration)");
            services.AddSingleton(Configuration);

            //_log.Debug("\t- AddSingleton<IDatabaseConfiguration,DatabaseConfiguration>");
            services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>();

            //_log.Debug("\t- AddScoped<IRepository,Repository>");
            services.AddScoped<IRepository, Repository>();

            //_log.Debug("\t- AddSingleton<JsonSerializerSettings, JsonSerializerSettings>");
            services.AddSingleton<JsonSerializerSettings, JsonSerializerSettings>(provider => jsonSerializerSettings);

            //_log.Debug("\t- AddTransient<WwpEntities>");
            services.AddTransient<WwpEntities>(); // https://docs.asp.net/en/latest/fundamentals/dependency-injection.html

            //_log.Debug("\t- AddSingleton<Func<WwpEntities>>(c => () => c.GetService<WwpEntities>())");
            services.AddSingleton<Func<WwpEntities>>(c => () => c.GetService<WwpEntities>());

            //_log.Debug("\t- AddScoped<IAuthUser, AuthUser>");
            services.AddScoped<IAuthUser, AuthUser>();

            //_log.Debug("\t- AddScoped<IApiUser, ApiUser>");
            services.AddScoped<IApiUser, ApiUser>();

            //_log.Debug("\t- AddScoped<IAuthenticationService, EntSecAuthenticationService>");
            services.AddScoped<IAuthenticationService, EntSecAuthenticationService>();
            //services.AddScoped<IAuthenticationService, LocalAuthenticationService>();

            //_log.Debug("\t- AddScoped<ITimelimitService, TimelimitService>");
            services.AddScoped<ITimelimitService, TimelimitService>();

            //_log.Debug("\t- AddScoped<IDb2TimelimitService, Db2TimelimitService>");
            services.AddScoped<IDb2TimelimitService, Db2TimelimitService>();

            services.AddScoped<SingleInstanceFactory>(c => { return t => { return c.GetService(t); }; });

            //_log.Debug("\t- AddScoped<IContentService, ContentService>");
            services.AddScoped<IContentService, ContentService>();

            //_log.Debug("\t- AddScoped<ITokenProvider, JwtSecurityTokenProvider>");
            services.AddScoped<ITokenProvider, JwtSecurityTokenProvider>();

            //_log.Debug("\t- AddSingleton<IErrorInfoConverter, DefaultErrorInfoConverter>");
            services.AddSingleton<IErrorInfoConverter, DefaultErrorInfoConverter>();

            //_log.Debug("\t- AddScoped(ApplicationContext.Current)");
            services.AddScoped(x => ApplicationContext.Current);

            //_log.Debug("\t- AddSingleton<IApiExceptionHandler, ApiExceptionHandler>");
            services.AddSingleton<IApiExceptionHandler, ApiExceptionHandler>();

            //_log.Debug("\t- AddScoped<EntSecAuthenticationService>");
            services.AddScoped<EntSecAuthenticationService>();

            //_log.Debug("\t- AddSingleton<IAuthorizationHandler, ApiKeyHandler>");
            //services.AddSingleton<IAuthorizationHandler, ApiKeyHandler>();

            //_log.Debug("\t- AddTransient<IAuthorizationPolicyProvider, WwpAuthorizationPolicyProvider>");
            //services.AddTransient<IAuthorizationPolicyProvider, WwpAuthorizationPolicyProvider>();

            //_log.Debug("\t- AddTransient<IConfidentialityChecker, ConfidentialityChecker>");
            services.AddTransient<IConfidentialityChecker, ConfidentialityChecker>();

            //_log.Debug("\t- AddTransient<IAuthAccessChecker, AuthAccessChecker>");
            services.AddTransient<IAuthAccessChecker, AuthAccessChecker>();

            services.AddScoped<ILocations, Locations>();

            //_log.Debug("\t- AddTransient<IClientRegistrationViewModel, ClientRegistrationViewModel>");
            services.AddTransient<IClientRegistrationViewModel, ClientRegistrationViewModel>();

            services.AddScoped<IAppVersion, AppVersion>(c => new AppVersion(Program.Version.Major, Program.Version.Minor, Program.Version.Build, Program.Version.Revision, LevelSwitch));

            //_log.Debug("\t- AddSingleton(new TokenProviderOptions)");

            var tpo = new TokenProviderOptions
                      {
                          Path               = "/api/token",
                          Audience           = "WWPUser",
                          Issuer             = "DCF",
                          SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
                          Expiration         = TimeSpan.FromHours(12)
                      };

            services.AddSingleton(s => tpo);

            var tvp = new TokenValidationParameters
                      {
                          // The signing key must match!
                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey         = _signingKey,

                          // Validate the JWT Issuer (iss) claim
                          ValidateIssuer = true,
                          ValidIssuer    = "DCF",

                          // Validate the JWT Audience (aud) claim
                          ValidateAudience = true,
                          ValidAudience    = "WWPUser",

                          // Validate the token expiry
                          ValidateLifetime = true,

                          // If you want to allow a certain amount of clock drift, set that here:
                          ClockSkew = TimeSpan.FromMinutes(5)
                      };

            services.AddSingleton(s => tvp);

            ConfigureAuth(services, tvp);

            // Phase II (Employability Plan)
            var dbc = new DatabaseConfiguration();
            var cs  = WwpEntities.CreateSqlConnectionString(dbc.Server, dbc.Catalog, dbc.UserId, dbc.Password, "WWP", dbc.MaxPoolSize);

            ApplicationContext.SetIsInTransitionPeriod();

            services.AddScoped<ValidAuthUserMustExistAttribute>();
            services.AddScoped<ValidPinMustExistAttribute>();
            services.AddScoped<ValidEpIdMustExistAttribute>();

            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped<IDbContext, EPContext>(c => new EPContext(cs));
            services.AddScoped<EPContext>(c => new EPContext(cs));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IWWPRuleRepository, WWPRuleRepository>();

            // use namespace qualified names to prevent any naming clashes with Phase I repos.
            services.AddScoped<DataAccess.Interfaces.IAbsenceRepository, DataAccess.Repositories.AbsenceRepository>();
            services.AddScoped<DataAccess.Interfaces.IActionAssigneeRepository, DataAccess.Repositories.ActionAssigneeRepository>();
            services.AddScoped<DataAccess.Interfaces.IActionItemRepository, DataAccess.Repositories.ActionItemRepository>();
            services.AddScoped<DataAccess.Interfaces.IActionNeededRepository, DataAccess.Repositories.ActionNeededRepository>();
            services.AddScoped<DataAccess.Interfaces.IActionNeededPageRepository, DataAccess.Repositories.ActionNeededPageRepository>();
            services.AddScoped<DataAccess.Interfaces.IActionNeededTaskRepository, DataAccess.Repositories.ActionNeededTaskRepository>();
            services.AddScoped<DataAccess.Interfaces.IActionPriorityRepository, DataAccess.Repositories.ActionPriorityRepository>();
            services.AddScoped<DataAccess.Interfaces.IActivityCompletionReasonRepository, DataAccess.Repositories.ActivityCompletionReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IActivityContactBridgeRepository, DataAccess.Repositories.ActivityContactBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IActivityLocationRepository, DataAccess.Repositories.ActivityLocationRepository>();
            services.AddScoped<DataAccess.Interfaces.IActivityRepository, DataAccess.Repositories.ActivityRepository>();
            services.AddScoped<DataAccess.Interfaces.IActivityScheduleFrequencyBridgeRepository, DataAccess.Repositories.ActivityScheduleFrequencyBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IActivityScheduleRepository, DataAccess.Repositories.ActivityScheduleRepository>();
            services.AddScoped<DataAccess.Interfaces.IActivityTypeRepository, DataAccess.Repositories.ActivityTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IAfterPullDownWeeklyBatchDetailsRepository, DataAccess.Repositories.AfterPullDownWeeklyBatchDetailsRepository>();
            services.AddScoped<DataAccess.Interfaces.IAuxiliaryRepository, DataAccess.Repositories.AuxiliaryRepository>();
            services.AddScoped<DataAccess.Interfaces.IAuxiliaryReasonRepository, DataAccess.Repositories.AuxiliaryReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IAuxiliaryStatusRepository, DataAccess.Repositories.AuxiliaryStatusRepository>();
            services.AddScoped<DataAccess.Interfaces.IAuxiliaryStatusTypeRepository, DataAccess.Repositories.AuxiliaryStatusTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.ICareerAssessmentRepository, DataAccess.Repositories.CareerAssessmentRepository>();
            services.AddScoped<DataAccess.Interfaces.ICareerAssessmentElementBridgeRepository, DataAccess.Repositories.CareerAssessmentElementBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.ICFParticipationEntryRepository, DataAccess.Repositories.CFParticipationEntryRepository>();
            services.AddScoped<DataAccess.Interfaces.ICityRepository, DataAccess.Repositories.CityRepository>();
            services.AddScoped<DataAccess.Interfaces.IContactRepository, DataAccess.Repositories.ContactRepository>();
            services.AddScoped<DataAccess.Interfaces.IContactTitleTypeRepository, DataAccess.Repositories.ContactTitleTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IContractAreaRepository, DataAccess.Repositories.ContractAreaRepository>();
            services.AddScoped<DataAccess.Interfaces.ICountryRepository, DataAccess.Repositories.CountryRepository>();
            services.AddScoped<DataAccess.Interfaces.IDocumentRepository, DataAccess.Repositories.DocumentRepository>();
            services.AddScoped<DataAccess.Interfaces.IDrugScreeningRepository, DataAccess.Repositories.DrugScreeningRepository>();
            services.AddScoped<DataAccess.Interfaces.IDrugScreeningStatusRepository, DataAccess.Repositories.DrugScreeningStatusRepository>();
            services.AddScoped<DataAccess.Interfaces.IDrugScreeningStatusTypeRepository, DataAccess.Repositories.DrugScreeningStatusTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.ICountyAndTribeRepository, DataAccess.Repositories.CountyAndTribeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAAssetsRepository, DataAccess.Repositories.EAAssetsRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAAlternateMailingAddressRepository, DataAccess.Repositories.EAAlternateMailingAddressRepository>();
            services.AddScoped<DataAccess.Interfaces.IEACommentRepository, DataAccess.Repositories.EACommentRepository>();
            services.AddScoped<DataAccess.Interfaces.IEACommentTypeRepository, DataAccess.Repositories.EACommentTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEACommentTypeBridgeRepository, DataAccess.Repositories.EACommentTypeBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAEmergencyTypeRepository, DataAccess.Repositories.EAEmergencyTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAEmergencyTypeReasonBridgeRepository, DataAccess.Repositories.EAEmergencyTypeReasonBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAEmergencyTypeReasonRepository, DataAccess.Repositories.EAEmergencyTypeReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAEnergyCrisisRepository, DataAccess.Repositories.EAEnergyCrisisRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAFinancialNeedRepository, DataAccess.Repositories.EAFinancialNeedRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAHomelessnessRepository, DataAccess.Repositories.EAHomelessnessRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAHouseHoldIncomeRepository, DataAccess.Repositories.EAHouseHoldIncomeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAImpendingHomelessnessRepository, DataAccess.Repositories.EAImpendingHomelessnessRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAIndividualTypeRepository, DataAccess.Repositories.EAIndividualTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAIPVRepository, DataAccess.Repositories.EAIPVRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAIPVOccurrenceRepository, DataAccess.Repositories.EAIPVOccurrenceRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAIPVReasonRepository, DataAccess.Repositories.EAIPVReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAIPVReasonBridgeRepository, DataAccess.Repositories.EAIPVReasonBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAIPVStatusRepository, DataAccess.Repositories.EAIPVStatusRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAPaymentRepository, DataAccess.Repositories.EAPaymentRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAPaymentAmountRepository, DataAccess.Repositories.EAPaymentAmountRepository>();
            services.AddScoped<DataAccess.Interfaces.IEARelationshipTypeRepository, DataAccess.Repositories.EARelationshipTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEARequestContactInfoRepository, DataAccess.Repositories.EARequestContactInfoRepository>();
            services.AddScoped<DataAccess.Interfaces.IEARequestEmergencyTypeBridgeRepository, DataAccess.Repositories.EARequestEmergencyTypeBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEARequestParticipantBridgeRepository, DataAccess.Repositories.EARequestParticipantBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEARequestRepository, DataAccess.Repositories.EARequestRepository>();
            services.AddScoped<DataAccess.Interfaces.IEARequestStatusRepository, DataAccess.Repositories.EARequestStatusRepository>();
            services.AddScoped<DataAccess.Interfaces.IEARequestStatusReasonRepository, DataAccess.Repositories.EARequestStatusReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IEASSNExemptTypeRepository, DataAccess.Repositories.EASSNExemptTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAStatusRepository, DataAccess.Repositories.EAStatusRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAStatusReasonRepository, DataAccess.Repositories.EAStatusReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAVerificationTypeBridgeRepository, DataAccess.Repositories.EAVerificationTypeBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAVehiclesRepository, DataAccess.Repositories.EAVehiclesRepository>();
            services.AddScoped<DataAccess.Interfaces.IEAVerificationTypeRepository, DataAccess.Repositories.EAVerificationTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IElementRepository, DataAccess.Repositories.ElementRepository>();
            services.AddScoped<DataAccess.Interfaces.IEligibilityByFPLRepository, DataAccess.Repositories.EligibilityByFPLRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmployabilityPlanRepository, DataAccess.Repositories.EmployabilityPlanRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmployabilityPlanRepository, DataAccess.Repositories.EmployabilityPlanRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmploymentInformationRepository, DataAccess.Repositories.EmploymentInformationRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmploymentVerificationRepository, DataAccess.Repositories.EmploymentVerificationRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmployabilityPlanStatusTypeRepository, DataAccess.Repositories.EmployabilityPlanStatusTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmployabilityPlanActivityBridgeRepository, DataAccess.Repositories.EmployabilityPlanActivityBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmployabilityPlanEmploymentInfoBridgeRepository, DataAccess.Repositories.EmployabilityPlanEmploymentInfoBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEmployabilityPlanGoalBridgeRepository, DataAccess.Repositories.EmployabilityPlanGoalBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEnrolledProgramActivityCompletionReasonBridgeRepository, DataAccess.Repositories.EnrolledProgramActivityCompletionReasonBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEnrolledProgramEPActivityTypeBridgeRepository, DataAccess.Repositories.EnrolledProgramEPActivityTypeBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEnrolledProgramJobTypeBridgeRepository, DataAccess.Repositories.EnrolledProgramJobTypeBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEnrolledProgramParticipationStatusTypeBridgeRepository, DataAccess.Repositories.EnrolledProgramParticipationStatusTypeBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEnrolledProgramPinCommentTypeBridgeRepository, DataAccess.Repositories.EnrolledProgramPinCommentTypeBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IEnrolledProgramRepository, DataAccess.Repositories.EnrolledProgramRepository>();
            services.AddScoped<DataAccess.Interfaces.IEnrolledProgramValidityRepository, DataAccess.Repositories.EnrolledProgramValidityRepository>();
            services.AddScoped<DataAccess.Interfaces.IFrequencyRepository, DataAccess.Repositories.FrequencyRepository>();
            services.AddScoped<DataAccess.Interfaces.IFrequencyTypeRepository, DataAccess.Repositories.FrequencyTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IGoalEndReasonRepository, DataAccess.Repositories.GoalEndReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IGoalRepository, DataAccess.Repositories.GoalRepository>();
            services.AddScoped<DataAccess.Interfaces.IGoalStepRepository, DataAccess.Repositories.GoalStepRepository>();
            services.AddScoped<DataAccess.Interfaces.IGoalTypeRepository, DataAccess.Repositories.GoalTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IGoodCauseGrantedReasonRepository, DataAccess.Repositories.GoodCauseGrantedReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IGoodCauseDeniedReasonRepository, DataAccess.Repositories.GoodCauseDeniedReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.IJobReadinessRepository, DataAccess.Repositories.JobReadinessRepository>();
            services.AddScoped<DataAccess.Interfaces.IJobTypeRepository, DataAccess.Repositories.JobTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IJRApplicationInfoRepository, DataAccess.Repositories.JRApplicationInfoRepository>();
            services.AddScoped<DataAccess.Interfaces.IJRContactInfoRepository, DataAccess.Repositories.JRContactInfoRepository>();
            services.AddScoped<DataAccess.Interfaces.IJRHistoryInfoRepository, DataAccess.Repositories.JRHistoryInfoRepository>();
            services.AddScoped<DataAccess.Interfaces.IJRInterviewInfoRepository, DataAccess.Repositories.JRInterviewInfoRepository>();
            services.AddScoped<DataAccess.Interfaces.IJRWorkPreferencesRepository, DataAccess.Repositories.JRWorkPreferencesRepository>();
            services.AddScoped<DataAccess.Interfaces.IJRWorkPreferenceShiftBridgeRepository, DataAccess.Repositories.JRWorkPreferenceShiftBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IJRWorkShiftRepository, DataAccess.Repositories.JRWorkShiftRepository>();
            services.AddScoped<DataAccess.Interfaces.INonParticipationReasonRepository, DataAccess.Repositories.NonParticipationReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.INonSelfDirectedActivityRepository, DataAccess.Repositories.NonSelfDirectedActivityRepository>();
            services.AddScoped<DataAccess.Interfaces.IOfficeRepository, DataAccess.Repositories.OfficeRepository>();
            services.AddScoped<DataAccess.Interfaces.IOrganizationRepository, DataAccess.Repositories.OrganizationRepository>();
            services.AddScoped<DataAccess.Interfaces.IOrganizationInformationRepository, DataAccess.Repositories.OrganizationInformationRepository>();
            services.AddScoped<DataAccess.Interfaces.IOrganizationLocationRepository, DataAccess.Repositories.OrganizationLocationRepository>();
            services.AddScoped<DataAccess.Interfaces.IOverPaymentRepository, DataAccess.Repositories.OverPaymentRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipantRepository, DataAccess.Repositories.ParticipantRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipantEnrolledProgramRepository, DataAccess.Repositories.ParticipantEnrolledProgramRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipantEnrolledProgramCutOverBridgeRepository, DataAccess.Repositories.ParticipantEnrolledProgramCutOverBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipantPlacementRepository, DataAccess.Repositories.ParticipantPlacementRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipationEntryRepository, DataAccess.Repositories.ParticipationEntryRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipationEntryHistoryRepository, DataAccess.Repositories.ParticipationEntryHistoryRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipationMakeUpEntryRepository, DataAccess.Repositories.ParticipationMakeUpEntryRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipantPaymentHistoryRepository, DataAccess.Repositories.ParticipantPaymentHistoryRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipationPeriodSummaryRepository, DataAccess.Repositories.ParticipationPeriodSummaryRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipationPeriodLookUpRepository, DataAccess.Repositories.ParticipationPeriodLookUpRepository>();
            services.AddScoped<DataAccess.Interfaces.IParticipationStatusRepository, DataAccess.Repositories.ParticipationStatusRepository>();
            services.AddScoped<DataAccess.Interfaces.IPCCTBridgeRepository, DataAccess.Repositories.PCCTBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPinCommentRepository, DataAccess.Repositories.PinCommentRepository>();
            services.AddScoped<DataAccess.Interfaces.IPinCommentTypeRepository, DataAccess.Repositories.PinCommentTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPlacementTypeRepository, DataAccess.Repositories.PlacementTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPlanRepository, DataAccess.Repositories.PlanRepository>();
            services.AddScoped<DataAccess.Interfaces.IPlanSectionRepository, DataAccess.Repositories.PlanSectionRepository>();
            services.AddScoped<DataAccess.Interfaces.IPlanSectionResourceRepository, DataAccess.Repositories.PlanSectionResourceRepository>();
            services.AddScoped<DataAccess.Interfaces.IPlanSectionTypeRepository, DataAccess.Repositories.PlanSectionTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPlanStatusTypeRepository, DataAccess.Repositories.PlanStatusTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPlanTypeRepository, DataAccess.Repositories.PlanTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPOPClaimActivityBridgeRepository, DataAccess.Repositories.POPClaimActivityBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPOPClaimRepository, DataAccess.Repositories.POPClaimRepository>();
            services.AddScoped<DataAccess.Interfaces.IPOPClaimEmploymentBridgeRepository, DataAccess.Repositories.POPClaimEmploymentBridgeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPOPClaimHighWageRepository, DataAccess.Repositories.POPClaimHighWageRepository>();
            services.AddScoped<DataAccess.Interfaces.IPOPClaimStatusRepository, DataAccess.Repositories.POPClaimStatusRepository>();
            services.AddScoped<DataAccess.Interfaces.IPOPClaimStatusTypeRepository, DataAccess.Repositories.POPClaimStatusTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPOPClaimTypeRepository, DataAccess.Repositories.POPClaimTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IPullDownDateRepository, DataAccess.Repositories.PullDownDateRepository>();
            services.AddScoped<DataAccess.Interfaces.IRuleReasonRepository, DataAccess.Repositories.RuleReasonRepository>();
            services.AddScoped<DataAccess.Interfaces.ISimulatedDateRepository, DataAccess.Repositories.SimulatedDateRepository>();
            services.AddScoped<DataAccess.Interfaces.IStateRepository, DataAccess.Repositories.StateRepository>();
            services.AddScoped<DataAccess.Interfaces.IStatusRepository, DataAccess.Repositories.StatusRepository>();
            services.AddScoped<DataAccess.Interfaces.ISupportiveServiceRepository, DataAccess.Repositories.SupportiveServiceRepository>();
            services.AddScoped<DataAccess.Interfaces.ISpecialInitiativeRepository, DataAccess.Repositories.SpecialInitiativeRepository>();
            services.AddScoped<DataAccess.Interfaces.ISupportiveServiceTypeRepository, DataAccess.Repositories.SupportiveServiceTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.ITimeLimitRepository, DataAccess.Repositories.TimeLimitRepository>();
            services.AddScoped<DataAccess.Interfaces.ITransactionRepository, DataAccess.Repositories.TransactionRepository>();
            services.AddScoped<DataAccess.Interfaces.ITransactionTypeRepository, DataAccess.Repositories.TransactionTypeRepository>();
            services.AddScoped<DataAccess.Interfaces.IWageHourRepository, DataAccess.Repositories.WageHourRepository>();
            services.AddScoped<DataAccess.Interfaces.IWeeklyHoursWorkedRepository, DataAccess.Repositories.WeeklyHoursWorkedRepository>();
            services.AddScoped<DataAccess.Interfaces.IWorkerContactInfoRepository, DataAccess.Repositories.WorkerContactInfoRepository>();
            services.AddScoped<DataAccess.Interfaces.IWorkerRepository, DataAccess.Repositories.WorkerRepository>();
            services.AddScoped<DataAccess.Interfaces.IWorkerTaskCategoryRepository, DataAccess.Repositories.WorkerTaskCategoryRepository>();
            services.AddScoped<DataAccess.Interfaces.IWorkerTaskListRepository, DataAccess.Repositories.WorkerTaskListRepository>();
            services.AddScoped<DataAccess.Interfaces.IWorkerTaskStatusRepository, DataAccess.Repositories.WorkerTaskStatusRepository>();
            services.AddScoped<Model.Interface.Repository.IRuleReasonRepository, Model.Repository.Repository>();
            services.AddScoped<IDevOpsDomain, DevOpsDomain>();

            // (Phase II) EP - Business layers
            services.AddScoped<IActionNeededDomain, ActionNeededDomain>();
            services.AddScoped<IActivityDomain, ActivityDomain>();
            services.AddScoped<IAuxiliaryDomain, AuxiliaryDomain>();
            services.AddScoped<ICareerAssessmentDomain, CareerAssessmentDomain>();
            services.AddScoped<ICityDomain, CityDomain>();
            services.AddScoped<IDrugScreeningDomain, DrugScreeningDomain>();
            services.AddScoped<IEmergencyAssistanceDomain, EmergencyAssistanceDomain>();
            services.AddScoped<IEmployabilityPlanDomain, EmployabilityPlanDomain>();
            services.AddScoped<IEmploymentPlanDomain, EmploymentPlanDomain>();
            services.AddScoped<IEmploymentVerificationDomain, EmploymentVerificationDomain>();
            services.AddScoped<IEpEmploymentsDomain, EpEmploymentsDomain>();
            services.AddScoped<IFileUploadDomain, FileUploadDomain>();
            services.AddScoped<IGoalDomain, GoalDomain>();
            services.AddScoped<IJobReadinessDomain, JobReadinessDomain>();
            services.AddScoped<IOrganizationInformationDomain, OrganizationInformationDomain>();
            services.AddScoped<IOverUnderPaymentEmail, OverUnderPaymentEmail>();
            services.AddScoped<IParticipantDomain, ParticipantDomain>();
            services.AddScoped<IParticipantActivityDomain, ParticipantActivityDomain>();
            services.AddScoped<IParticipationTrackingDomain, ParticipationTrackingDomain>();
            services.AddScoped<IPinCommentDomain, PinCommentDomain>();
            services.AddScoped<IPlanDomain, PlanDomain>();
            services.AddScoped<IPOPClaimDomain, POPClaimDomain>();
            services.AddScoped<IReportDomain, ReportDomain>();
            services.AddScoped<ISmtpEmail, SmtpEmail>();
            services.AddScoped<ISupportiveServicesDomain, SupportiveServicesDomain>();
            services.AddScoped<ITransactionDomain, TransactionDomain>();
            services.AddScoped<IWorkerContactInfoDomain, WorkerDomain>();
            services.AddScoped<IWorkerTaskListDomain, WorkerTaskListDomain>();
            services.AddScoped<IWeeklyHoursWorkedDomain, WeeklyHoursWorkedDomain>();

            // (Phase II) CDO
            services.AddScoped<ISimulatedDateDomain, SimulatedDateDomain>();

            //// ----------------------------------------------------
            //// Register AutoMapper Profiles Here
            //// ----------------------------------------------------
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //// ----------------------------------------------------
            //// Connected Services (CARES/MCI/CWW/GoogleApi/Tableau etc...)
            //// ----------------------------------------------------

            _log.Info("".PadRight(20, '-'));
            _log.Info("Configuring ConnectedServices");

            _log.Info("\t- WCF SOAP message logging");
            WebServicesLogger.Init(cs);

            _log.Info("\t- WCF SOAP performance logging");
            WebPerfLogger.Init(cs);

            //load configs from connectedServices.json
            var wcfSection   = Configuration.GetSection("wcfSoap");
            var wcfConfigs   = wcfSection.Get<List<WcfSoapConfig>>();
            var wcfEnvConfig = wcfConfigs.FirstOrDefault(i => i.Env == dbc.Catalog); // keys off of database name

            if (wcfEnvConfig == null)
            {
                _log.Error("WCF configuration is invalid or missing");
                throw new ConfigurationErrorsException("WCF configuration is invalid or missing");
            }

            // MCI only support SOAP 1.1
            _log.Info("\t\t- MCI Service");
            var mciSvcCfg = wcfEnvConfig.Services.FirstOrDefault(i => i.Name.ToLower() == "mci");

            if (null == mciSvcCfg)
            {
                _log.Error("CWW Indv Service configuration is invalid or missing");
                throw new ConfigurationErrorsException("CWW Indv Service configuration is invalid or missing");
            }

            mciSvcCfg.Pwd = (wcfEnvConfig.Env == "WWPTRN" ? Configuration["WWP_MCI_CRED2"] : Configuration["WWP_MCI_CRED"]);

            _log.Info($"\t\t\t- Uid: {mciSvcCfg.Uid}");
            _log.Info($"\t\t\t- Pwd: [** masked **]");
            _log.Info($"\t\t\t- To : {mciSvcCfg.To}");

            var mciEndpoint = new EndpointAddress(wcfEnvConfig.Endpoint);

            var mciBinding = new BasicHttpBinding
                             {
                                 Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport, Transport = { ClientCredentialType = HttpClientCredentialType.None } }
                             };

            var mciChFac = new ChannelFactory<IMciService>(mciBinding, mciEndpoint);
            mciChFac.Endpoint.Behaviors.Add(new EndpointBehavior(new MessageInspector(mciSvcCfg.Uid, mciSvcCfg.Pwd, mciSvcCfg.To)));

            services.AddSingleton(mciChFac);
            services.AddScoped(c => c.GetService<ChannelFactory<IMciService>>().CreateChannel());

            // ---------------------------------------------------------------------------
            // CWW only supports SOAP 1.2 - so we need a newer binding type for 1.2 support
            // ---------------------------------------------------------------------------

            var cwwEndpointUri = new Uri(wcfEnvConfig.Endpoint);

            var cwwBinding = new WSHttpBinding
                             {
                                 Name      = "cwwBinding",
                                 Security  = { Mode = SecurityMode.Transport },
                                 Namespace = "MyGoofyNameSpace"
                             };

            // CWW Individual Service
            _log.Info("\t\t- CWW Individual Service");
            var cwwIndvSvcCfg = wcfEnvConfig.Services.FirstOrDefault(i => i.Name.ToLower() == "cwwindvservice");

            if (null == cwwIndvSvcCfg)
            {
                _log.Error("CWW Indv Service configuration is invalid or missing");
                throw new ConfigurationErrorsException("CWW Indv Service configuration is invalid or missing");
            }

            _log.Info($"\t\t\t- Uid: {cwwIndvSvcCfg.Uid}");
            _log.Info($"\t\t\t- Pwd: [** masked **]");
            _log.Info($"\t\t\t- To : {cwwIndvSvcCfg.To}");

            cwwIndvSvcCfg.Pwd = (wcfEnvConfig.Env == "WWPTRN" ? Configuration["WWP_CWW_INDV_CRED2"] : Configuration["WWP_CWW_INDV_CRED"]);
            var cwwIndSvcChFac = new ChannelFactory<ICwwIndService>(cwwBinding, new EndpointAddress(cwwIndvSvcCfg.To));
            cwwIndSvcChFac.Endpoint.Behaviors.Add(new EndpointBehavior(new MessageInspector(cwwIndvSvcCfg.Uid, cwwIndvSvcCfg.Pwd)));
            cwwIndSvcChFac.Endpoint.Behaviors.Add(new ClientViaBehavior(cwwEndpointUri));

            services.AddSingleton(cwwIndSvcChFac);
            services.AddScoped(c => c.GetService<ChannelFactory<ICwwIndService>>().CreateChannel());

            // CWW Key Security Service
            _log.Info("\t\t- CWW Key Security Service");
            var cwwKeySvcCfg = wcfEnvConfig.Services.FirstOrDefault(i => i.Name.ToLower() == "cwwkeysecservice");

            if (null == cwwKeySvcCfg)
            {
                _log.Error("CWW KeySec Service configuration is invalid or missing");
                throw new ConfigurationErrorsException("CWW KeySec Service configuration is invalid or missing");
            }

            _log.Info($"\t\t\t- Uid: {cwwKeySvcCfg.Uid}");
            _log.Info($"\t\t\t- Pwd: [** masked **]");
            _log.Info($"\t\t\t- To : {cwwKeySvcCfg.To}");

            cwwKeySvcCfg.Pwd = (wcfEnvConfig.Env == "WWPTRN" ? Configuration["WWP_CWW_KEYSEC_CRED2"] : Configuration["WWP_CWW_KEYSEC_CRED"]);
            var cwwKeySecSvcChFac = new ChannelFactory<ICwwKeySecService>(cwwBinding, new EndpointAddress(cwwKeySvcCfg.To));
            cwwKeySecSvcChFac.Endpoint.Behaviors.Add(new EndpointBehavior(new MessageInspector(cwwKeySvcCfg.Uid, cwwKeySvcCfg.Pwd)));
            cwwKeySecSvcChFac.Endpoint.Behaviors.Add(new ClientViaBehavior(cwwEndpointUri));

            services.AddSingleton(cwwKeySecSvcChFac);
            services.AddScoped(c => c.GetService<ChannelFactory<ICwwKeySecService>>().CreateChannel());


            // ECF Upload Service
            _log.Info("\t\t- ECFUpload Service");
            var ecfSection   = Configuration.GetSection("ecf");
            var ecfConfigs   = ecfSection.Get<List<FileUploadConfig>>();
            var ecfEnvConfig = ecfConfigs.FirstOrDefault(i => i.Env == dbc.Catalog); // keys off of database name

            if (null == ecfEnvConfig)
            {
                _log.Error($"\t\t- ECF Configuration is missing in connectedServices.json file");
            }

            services.AddSingleton<IFileUploadConfig>(ecfEnvConfig);

            // Finalist
            _log.Info("\t- Finalist");
            var finalistSection   = Configuration.GetSection("finalist");
            var finalistConfig    = finalistSection.Get<FinalistConfig>();
            var finalistEnvConfig = finalistConfig.EndPoints.FirstOrDefault(i => i.Env == dbc.Catalog); // keys off of database name

            if (finalistEnvConfig == null)
            {
                _log.Error($"\t\t- Finalist configuration is missing in connectedServices.json file");
                throw new ConfigurationErrorsException("Finalist Service configuration is invalid or missing");
            }

            _log.Info($"\t\t- using Finalist EndPoint: {finalistEnvConfig.EndPoint}");

            var finalistBinding = new BasicHttpBinding
                                  {
                                      Name      = "finalistBinding",
                                      Security  = { Mode = BasicHttpSecurityMode.None },
                                      Namespace = "FinalistNameSpace"
                                  };

            var finalistChFac = new ChannelFactory<USPostalAddress>(finalistBinding, new EndpointAddress(finalistEnvConfig.EndPoint));
            finalistChFac.Endpoint.Behaviors.Add(new ClientViaBehavior(new Uri(finalistEnvConfig.EndPoint)));

            services.AddSingleton(finalistChFac);
            services.AddScoped(c => c.GetService<ChannelFactory<USPostalAddress>>().CreateChannel());
            services.AddScoped<IFinalistService, FinalistService>();

            // GoogleApi services
            _log.Info("\t- GoogleApi");
            var googleSection = Configuration.GetSection("google");
            var googleConfig  = googleSection.Get<GoogleConfig>();

            if (null == googleConfig)
            {
                _log.Error($"\t\t- GoogleApi configuration is missing in connectedServices.json file");
            }

            _log.Info($"\t\t- using Google Api Key: {googleConfig?.ApiKey}");

            services.AddSingleton<IGoogleConfig>(googleConfig);
            services.AddScoped<IGoogleApi, GoogleApi>();

            // EntSec
            _log.Info("\t- EntSec");
            var esSection   = Configuration.GetSection("entSec");
            var esConfigs   = esSection.Get<List<EntSecConfig>>();
            var esEnvConfig = esConfigs.FirstOrDefault(i => i.Env == dbc.Catalog); // keys off of database name

            if (esEnvConfig == null)
            {
                _log.Error($"\t\t- EntSec configuration is missing in connectedServices.json file");
                throw new ConfigurationErrorsException("EntSec Service configuration is invalid or missing");
            }

            services.AddSingleton<IEntSecConfig>(esEnvConfig);

            _log.Info($"\t\t- Env      : {esEnvConfig.Env}");
            _log.Info($"\t\t- Endpoint : {esEnvConfig.Endpoint}");
            _log.Info($"\t\t- Username : {esEnvConfig.InteropApplicationKey}");
            _log.Info($"\t\t- SiteId   : {esEnvConfig.ApplicationKey}");

            // Email Addr
            _log.Info("\t- Email");
            var emailSection = Configuration.GetSection("emailAddr");
            var emailConfig  = emailSection.Get<EmailConfig>();

            if (emailConfig == null)
            {
                _log.Error($"\t\t- Email Address configuration is missing in connectedServices.json file");
                throw new ConfigurationErrorsException("Email Address Service configuration is invalid or missing");
            }

            services.AddSingleton<IEmailConfig>(emailConfig);

            _log.Info($"\t\t- SmtpServer : {emailConfig.SmtpServer}");
            _log.Info($"\t\t- SmtpPort   : {emailConfig.SmtpPort}");
            _log.Info($"\t\t- From       : {emailConfig.From}");

            // Tableau services
            _log.Info("\t- TableauApi");
            var tableauSection   = Configuration.GetSection("tableau");
            var tableauConfigs   = tableauSection.Get<List<TableauConfig>>();
            var tableauEnvConfig = tableauConfigs.FirstOrDefault(i => i.Env == dbc.Catalog); // keys off of database name

            if (tableauEnvConfig == null)
            {
                _log.Error($"\t\tTableau configuration is missing for {dbc.Catalog}  in connectedServices.json file");
                //throw new SettingsPropertyNotFoundException(nameof(TableauConfig));   // don't throw exception just yet... or should we?
            }

            services.AddSingleton<ITableauConfig>(tableauEnvConfig);
            services.AddScoped<ITableauApi, TableauApi>();

            if (null != tableauEnvConfig) // this keeps ReSharper happy
            {
                _log.Info($"\t\t- Env      : {tableauEnvConfig.Env}");
                _log.Info($"\t\t- Endpoint : {tableauEnvConfig.Endpoint}");
                _log.Info($"\t\t- Username : {tableauEnvConfig.UserName}");
                _log.Info($"\t\t- SiteId   : {tableauEnvConfig.SiteId}");
            }

            _log.Info("".PadRight(20, '-'));
            _log.Info("Registering Dcf.Wwp components");

            var wwpServices = services.Where(i => i.ServiceType.Namespace != null && (i.ServiceType.Namespace.ToLower().StartsWith("dcf.")))
                                      .Select(i => i)
                                      .OrderBy(i => i.ServiceType.Name)
                                      .GroupBy(i => i.Lifetime)
                                      .ToList();

            foreach (var grouping in wwpServices)
            {
                _log.Debug($"\t\t- Lifetime: {grouping.Key}");

                foreach (var component in grouping)
                {
                    var svc = component.ServiceType?.Name;
                    var imp = (component.ImplementationType?.Name ?? "{Lambda Expression}");
                    var ns  = component.ImplementationType?.Namespace ?? component.ServiceType?.Namespace;
                    var tmp = $"\t\t\t - <{ svc}, { imp}>".PadRight(75, ' ') + $"- [{ns}]";
                    _log.Debug(tmp);
                }
            }

            // done ...
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

            _log.Info("".PadRight(25, '-'));
            _log.Info("Configuring .NET pipeline middleware");
            _log.Debug($"\tenv.ApplicationName: {env.ApplicationName}");
            _log.Debug($"\tenv.ContentRootPath: {env.ContentRootPath}");

            _log.Info("Environment variables:");

#if (USE_ENV_VARS2)
            _log.Info($"\tWWP_DB_SERVER2: {Configuration["WWP_DB_SERVER2"]}");
            _log.Info($"\tWWP_DB_NAME2  : {Configuration["WWP_DB_NAME2"]}");
            _log.Info($"\tWWP_DB_USER2  : {Configuration["WWP_DB_USER2"]}");
            _log.Info($"\tWWP_DB_PASS2  : [*** masked ***]");
#else
            _log.Info($"\tWWP_DB_SERVER: {Configuration["WWP_DB_SERVER"]}");
            _log.Info($"\tWWP_DB_NAME  : {Configuration["WWP_DB_NAME"]}");
            _log.Info($"\tWWP_DB_USER  : {Configuration["WWP_DB_USER"]}");
            _log.Info($"\tWWP_DB_PASS  : [*** masked ***]");
#endif
            var dbConfiguration = app.ApplicationServices.GetService<IDatabaseConfiguration>();

            _log.Info("Database Configuration from EnvVars above (should be identical to values above)");
            _log.Info($"\tSERVER     : {dbConfiguration.Server}");
            _log.Info($"\tDBNAME     : {dbConfiguration.Catalog}");
            _log.Info($"\tDBUSER     : {dbConfiguration.UserId}");
            _log.Info($"\tDBPASS     : [*** masked ***]");
            _log.Info($"\tMaxPoolSize: {dbConfiguration.MaxPoolSize}");

            // register DbContext configuration to use the retrysettings in the WwpEntitiesConfiguration
            //DbConfiguration.SetConfiguration(new WwpEnttitesDbConfiguration());  // removed this 03/19/2019 temporarily. If you set this, it'll break EPContext
            //DbConfiguration.SetConfiguration(new EFConfig()); // removed this 03/20/2019 temporarily, too. 

            app.UseDeveloperExceptionPage();

            //ConfigureAuth(app);

            _log.Info("Configuring Serilog logger");
            var serilogConnStr = WwpEntities.CreateSqlConnectionString(dbConfiguration.Server, dbConfiguration.Catalog, dbConfiguration.UserId, dbConfiguration.Password, "WWP-Logging", dbConfiguration.MaxPoolSize);
            var columnOptions  = new ColumnOptions();
            columnOptions.Store.Add(StandardColumn.LogEvent);
            columnOptions.Properties.UsePropertyKeyAsElementName = true;
            columnOptions.Level.StoreAsEnum                      = true;

            Log.Logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .Enrich.WithCorrelationId()
                         .MinimumLevel.ControlledBy(LevelSwitch)
                         .Destructure.ToMaximumDepth(15)
#if !DEBUG
                         .WriteTo.RollingFile("logs/log-{Hour}.txt", fileSizeLimitBytes: 500 * 1024 * 1024, levelSwitch: Startup.LevelSwitch)
#endif
                         .WriteTo.LiterateConsole()
                         .WriteTo.MSSqlServer(serilogConnStr, "LogEvent", columnOptions: columnOptions, schemaName: "wwp")
                         .CreateLogger();

            var filterSetting = new FilterLoggerSettings
                                {
                                    { "Microsoft", LogLevel.Error },
                                    { "System", LogLevel.Error }
                                };

            loggerFactory = loggerFactory.WithFilter(filterSetting);

            loggerFactory.AddConsole(new ConsoleLoggerSettings { IncludeScopes = true });
            loggerFactory.AddDebug();
            loggerFactory.AddEventLog(LogLevel.Critical);

            //Startup.sb.Clear();
            SelfLog.Enable(sw);

            appLifetime.ApplicationStopped.Register(() => { Log.CloseAndFlush(); });
            Log.Logger.Debug("Logging configured");
            _log.Info($"\t logging to {dbConfiguration.Catalog} on {dbConfiguration.Server} at Level: {LevelSwitch.MinimumLevel}");

            app.UseApplicationContext();

            // http://blog.nbellocam.me/2016/03/21/routing-angular-2-asp-net-core/
            // https://github.com/nbellocam/Angular2ASPNETCoreBaseApp/blob/routing-without-mvc/src/MyAngular2BaseApp/Startup.cs#L49
            app.Use(async (context, next) =>
                    {
                        await next();

                        if (context.Response.StatusCode == 404 && context.Request.Headers["Accept"].Any(x => x.ToLower().Contains("text/html")) && !Path.HasExtension(context.Request.Path.Value))
                        {
                            context.Request.Path = "/index.html";
                            await next();
                        }
                    });

            // To better enable Angular, we'll enable default files:
            // https://docs.asp.net/en/latest/fundamentals/static-files.html#serving-a-default-document
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();

            _log.Debug("\t\t- AuthenticationMiddleware");
            app.UseMiddleware<AuthenticationMiddleware>();

            _log.Debug("\t\t- ApiKeyMiddleware");
            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseCors("AllowAll");
            app.UseRequestResponseLogging();
            app.UseNoCacheCacheControl();

            app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();
            app.UseGlobalExceptionHandler();
            app.UseMvc();

            _endTime = DateTime.Now;
            var elapsed = _endTime - _startTime;

            _log.Info("");
            _log.Info($"{env.ApplicationName} initialized in {elapsed.Milliseconds} ms");
            _log.Info($"{env.ApplicationName} Ready for requests.");
            _log.Info("Have a great day ;)");
            _log.Info("END STARTUP LOG".PadRight(45, '*'));
        }

        #region Authentication

        // The secret key every token will be signed with.
        // Keep this safe on the server!
        // *Moved to environment variable
        //private static readonly string secretKey = "A19ACB534DC4A4BD5FA16AF2B7CFB7BB1D42B48D5CF7EF1A5D72BCA8EF";

        private void ConfigureAuth(IServiceCollection services, TokenValidationParameters tvp)
        {
            //* We are going to user Bearer schema in the header instead of cookies
            //* The app will maintain the token in local storage and send with each 
            //* request

            _log.Info("".PadRight(20, '-'));
            _log.Info("Configuring Authentication");

            _log.Debug("\t- TokenValidationParameters");
            //var tokenValidationParameters = app.ApplicationServices.GetService<TokenValidationParameters>();

            _log.Debug($"\t\t- ValidateIssuerSigningKey = {tvp.ValidateIssuerSigningKey}");
            _log.Debug($"\t\t- IssuerSigningKey         = {tvp.IssuerSigningKey}");
            _log.Debug($"\t\t- ValidateIssuer           = {tvp.ValidateIssuer}");
            _log.Debug($"\t\t- ValidIssuer              = {tvp.ValidIssuer}");
            _log.Debug($"\t\t- ValidateAudience         = {tvp.ValidateAudience}");
            _log.Debug($"\t\t- ValidAudience            = {tvp.ValidAudience}");
            _log.Debug($"\t\t- ValidateLifetime         = {tvp.ValidateLifetime}");
            _log.Debug($"\t\t- ClockSkew                = {tvp.ClockSkew.TotalMinutes} (mins)");

            _log.Debug("\t- JwtBearerEvents");

            //TODO: Change events to log something helpful somewhere
            var jwtEvents = new JwtBearerEvents();

            jwtEvents.OnAuthenticationFailed = context =>
                                               {
                                                   Debug.WriteLine("JWT Authentication failed.");
                                                   return Task.WhenAll();
                                               };

            jwtEvents.OnChallenge = context =>
                                    {
                                        Debug.WriteLine("JWT Authentication challenged.");
                                        return Task.WhenAll();
                                    };

            jwtEvents.OnMessageReceived = context =>
                                          {
                                              Debug.WriteLine("JWT Message received.");
                                              return Task.WhenAll();
                                          };

            jwtEvents.OnTokenValidated = context =>
                                         {
                                             Debug.WriteLine("JWT Message Token validated.");
                                             return Task.WhenAll();
                                         };

            services.AddAuthentication(o =>
                                       {
                                           o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           o.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                                       }).AddJwtBearer(o =>
                                                       {
                                                           o.TokenValidationParameters = tvp;
                                                           o.Events                    = jwtEvents;
                                                       });

            _log.Debug("\t- authentication middleware and pipeline");
        }

        #endregion Authentication

        #endregion
    }
}
