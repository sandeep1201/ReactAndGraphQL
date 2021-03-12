using System;
using System.Globalization;
using System.Linq;
using System.Text;
using ContentManagerFileUploadService;
using Dcf.Wwp.ConnectedServices.Documents;
using Dcf.Wwp.Model.Interface.FileUpload;

namespace Dcf.Wwp.ConnectedServices.FileUpload
{
    public class FileUpload
    {
        /// <summary>
        /// Gets the proxy to use to interact with the remote web service
        /// </summary>
        /// <param name="mtomRequired">if set to <c>true</c> [mtom required].</param>
        /// <param name="cmServiceRequest"></param>
        /// <returns></returns>
        private static CMWebService GetProxy(bool mtomRequired, ref ICMServiceRequest cmServiceRequest)
        {
            var result = new CMWebService { Url = cmServiceRequest.CMServiceURL };

            if (mtomRequired)
                result.RequireMtom = true;
            return result;
        }

        /// <summary>
        /// Sets authentication parameters and returns them for authenticating to the web service
        /// </summary>
        /// <param name="cmServiceRequest"></param>
        /// <returns></returns>
        public static AuthenticationData GetAuthentication(ref ICMServiceRequest cmServiceRequest)
        {
            var serverDefinition = new ServerDef
                                   {
                                       ServerName = cmServiceRequest.CMServerName
                                   };

            var authenticationLogin = new AuthenticationDataLoginData
                                      {
                                          UserID   = cmServiceRequest.CMUserID,
                                          Password = cmServiceRequest.CMUserPwd,
                                      };

            var authentication = new AuthenticationData
                                 {
                                     LoginData = authenticationLogin,
                                     ServerDef = serverDefinition
                                 };

            return authentication;
        }


        /// <summary>
        /// Formats query criteria as a search parameter for passing to the search service.
        /// Passing the first param as String
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        private static string GetSearchCriteria(string inputString, string fieldName, ref string prefix)
        {
            if (string.IsNullOrEmpty(inputString)) return string.Empty;

            var output = $"{prefix}@{fieldName}=\"{inputString}\"";
            prefix = " AND ";
            return output;
        }

        /// <summary>
        /// Formats query criteria as a search parameter for passing to the search service.
        /// Passing the first param as String
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        protected static string GetRangeSearchCriteria(string inputString, string fieldName, ref string prefix)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                var output = string.Format("{0}@{1}>=\"{2}\"", prefix, fieldName, inputString);
                prefix = " AND ";
                return output;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Performs a search against the CM repository
        /// </summary>
        /// <returns></returns>
        private static RunQueryReply SearchMetaData(
            ref ICMServiceRequest cmServiceRequest,
            string                hfsCaseNumber       = "",
            string                hfsDocCode          = "",
            string                hfsDocType          = "",
            string                hfsSsn              = "",
            string                hfsPin              = "",
            string                hfsLastName         = "",
            string                hfsFirstName        = "",
            string                hfsDob              = "",
            string                hfsCounty           = "",
            string                ecfReceiveDate      = "",
            string                hfsScannedDate      = "",
            string                hfsACCTRACKNM       = "",
            string                hfsComments         = "",
            string                hfsCorrCntlNum      = "",
            string                hfsACCPIN           = "",
            string                hfsRFANUM           = "",
            string                hfsRFAPIN           = "",
            string                entDocLangType      = "",
            string                hfsProcessStatus    = "",
            string                hfsBatchName        = "",
            string                hfsBatchClass       = "",
            string                hfsIXStationId      = "",
            string                hfsIndexDate        = "",
            bool                  throwIfEmpty        = false,
            bool                  retrieveAttachments = true)
        {
            if (ecfReceiveDate == null) throw new ArgumentNullException(nameof(ecfReceiveDate));
            var parameterInfo = new StringBuilder();
            ecfReceiveDate = DateTime.Now.AddYears(-2).ToString("MM/dd/yyyy");
            parameterInfo.AppendFormat("hfsCaseNumber={0}",    hfsCaseNumber);
            parameterInfo.AppendFormat("/hfsDocCode={0}",      hfsDocCode);
            parameterInfo.AppendFormat("/hfsDocType={0}",      hfsDocType);
            parameterInfo.AppendFormat("/hfsSsn={0}",          hfsSsn);
            parameterInfo.AppendFormat("/hfsPin={0}",          hfsPin);
            parameterInfo.AppendFormat("/hfsLastName={0}",     hfsLastName);
            parameterInfo.AppendFormat("/hfsFirstName={0}",    hfsFirstName);
            parameterInfo.AppendFormat("/hfsDob={0}",          hfsDob);
            parameterInfo.AppendFormat("/hfsCounty={0}",       hfsCounty);
            parameterInfo.AppendFormat("/ecfReceiveDate={0}",  ecfReceiveDate);
            parameterInfo.AppendFormat("/hfsScannedDate={0}",  hfsScannedDate);
            parameterInfo.AppendFormat("/hfsACCTRACKNM={0}",   hfsACCTRACKNM);
            parameterInfo.AppendFormat("/hfsComments={0}",     hfsComments);
            parameterInfo.AppendFormat("/hfsCorrCntlNum={0}",  hfsCorrCntlNum);
            parameterInfo.AppendFormat("/hfsACCPIN={0}",       hfsACCPIN);
            parameterInfo.AppendFormat("hfsRFANUM={0}",        hfsRFANUM);
            parameterInfo.AppendFormat("hfsRFAPIN={0}",        hfsRFAPIN);
            parameterInfo.AppendFormat("/entDocLangType={0}",  entDocLangType);
            parameterInfo.AppendFormat("hfsProcessStatus={0}", hfsProcessStatus);
            parameterInfo.AppendFormat("hfsBatchName={0}",     hfsBatchName);
            parameterInfo.AppendFormat("hfsBatchClass={0}",    hfsBatchClass);
            parameterInfo.AppendFormat("hfsIXStationId={0}",   hfsIXStationId);
            parameterInfo.AppendFormat("hfsIndexDate={0}",     hfsIndexDate);

            var s       = GetProxy(retrieveAttachments, ref cmServiceRequest);
            var request = new RunQueryRequest { AuthenticationData = GetAuthentication(ref cmServiceRequest) };

            var queryString = new StringBuilder();
            var prefix      = string.Empty; //Only send Decimal fields if they aren't 0 


            //Send string fields unless empty 
            queryString.Append(GetSearchCriteria(hfsCaseNumber,    "HFS_CaseNumber",    ref prefix));
            queryString.Append(GetSearchCriteria(hfsIndexDate,     "HFS_IndexDate",     ref prefix));
            queryString.Append(GetSearchCriteria(hfsIXStationId,   "HFS_IX_Station_ID", ref prefix));
            queryString.Append(GetSearchCriteria(hfsProcessStatus, "HFS_ProcessStatus", ref prefix));
            queryString.Append(GetSearchCriteria(hfsRFANUM,        "HFS_RFA_NUM",       ref prefix));
            queryString.Append(GetSearchCriteria(hfsRFAPIN,        "HFS_RFA_PIN",       ref prefix));
            queryString.Append(GetSearchCriteria(hfsDocCode,       "HFS_Doc_Code",      ref prefix));
            queryString.Append(GetSearchCriteria(hfsDocType,       "HFS_Doc_Type",      ref prefix));
            queryString.Append(GetSearchCriteria(hfsSsn,           "HFS_SSN",           ref prefix));
            queryString.Append(GetSearchCriteria(hfsPin,           "HFS_PIN",           ref prefix));
            queryString.Append(GetSearchCriteria(hfsLastName,      "HFS_Last_Name",     ref prefix));
            queryString.Append(GetSearchCriteria(hfsFirstName,     "HFS_First_Name",    ref prefix));
            queryString.Append(GetSearchCriteria(hfsDob,           "HFS_DOB",           ref prefix));
            queryString.Append(GetSearchCriteria(hfsCounty,        "HFS_County",        ref prefix));
            queryString.Append(GetRangeSearchCriteria(ecfReceiveDate, "ECF_ReceiveDate", ref prefix));
            queryString.Append(GetSearchCriteria(hfsScannedDate, "HFS_ScannedDate",   ref prefix));
            queryString.Append(GetSearchCriteria(hfsComments,    "HFS_Comments",      ref prefix));
            queryString.Append(GetSearchCriteria(hfsCorrCntlNum, "HFS_CorrCntlNum",   ref prefix));
            queryString.Append(GetSearchCriteria(entDocLangType, "Ent_Doc_Lang_Type", ref prefix));

            //queryString.Append(GetSearchCriteria(dcfDocNum, "DCF_Doc_Num", ref prefix));

            if (hfsCorrCntlNum != "")
            {
                queryString.Append(GetSearchCriteria(hfsCorrCntlNum, "HFS_CorrCntlNum", ref prefix));
            }

            if (queryString.Length == 0)
            {
                throw new ArgumentException("No query criteria specified.  At least one criterion must be passed");
            }

            var qc = new QueryCriteria
                     {
                         QueryString = $"/'ECF_Document'[{queryString}]"
                     };
            request.QueryCriteria  = qc;
            request.retrieveOption = new RunQueryRequestRetrieveOption();
            request.retrieveOption = RunQueryRequestRetrieveOption.CONTENT;
            request.contentOption  = new RunQueryRequestContentOption();
            request.contentOption  = retrieveAttachments ? RunQueryRequestContentOption.ATTACHMENTS : RunQueryRequestContentOption.URL;
            var reply = s.RunQuery(request);
            if (reply.RequestStatus.success)
            {
                if (reply.mtomRef != null && reply.mtomRef.Length >= 1)
                {
                    return reply;
                }

                return throwIfEmpty ? null : reply;
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Unable to retrieve WWP Documents  (Parms = " + parameterInfo + ") from Content Manager. ");
            sb.AppendLine();

            if (reply.RequestStatus.ErrorData == null || reply.RequestStatus.ErrorData.Length <= 0) return null;
            reply.RequestStatus.ErrorData.ToList().ForEach(i =>
                                                           {
                                                               sb.AppendFormat("Error: ReturnCode: {0} Message: {1} StackTrace: {2} ", i.returnCode, i.errorMessage, i.stackTrace);
                                                               sb.AppendLine();
                                                           });

            return null;
        }


        /// <summary>
        /// Retrieves the specified WWP Doc from CM
        /// </summary>
        /// <param name="cmServiceRequest"></param>
        /// <param name="documentNumber">The Correspondence Control Number.</param>
        /// <param name="validationPID"></param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Unable to retrieve WWP Doc ( + ParameterInfo.ToString() + ).  Reason:  + Reply.ResultSet.count +  worksheets returned.</exception>
        public static byte[] Retrieve(ref ICMServiceRequest cmServiceRequest, string documentNumber, string validationPID = "")
        {
            var parameterInfo = new StringBuilder();
            parameterInfo.AppendFormat("/hfsCorrCntlNum={0}", documentNumber);
            parameterInfo.AppendFormat("/hfsDocCode={0}",     "WEP");
            var reply = SearchMetaData(ref cmServiceRequest, hfsCorrCntlNum: documentNumber, throwIfEmpty: true);

            if (reply?.ResultSet == null || reply.ResultSet.count <= 0) throw new ApplicationException($"Unable to retrieve document ({parameterInfo}).  Reason: {reply?.ResultSet?.count} documents returned.");

            var matchDocumentIndex = 0;

            if (reply.ResultSet.count <= 1) return reply.mtomRef[matchDocumentIndex].Value;
            // TODO: Edge case: more than one result so match on validationPID
            if (validationPID.Length > 0)
            {
            }

            return reply.mtomRef[matchDocumentIndex].Value;
        }

        /// <summary>
        /// Retrieves the specified WWP doc from CM
        /// </summary>
        /// <param name="cmServiceRequest"></param>
        /// <param name="pinNumber"></param>
        /// <param name="validationPID"></param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Unable to retrieve WWP Doc ( + ParameterInfo.ToString() + ).  Reason:  + Reply.ResultSet.count +  worksheets returned.</exception>
        public static RunQueryReply RetrieveAllDocuments(ref ICMServiceRequest cmServiceRequest, string pinNumber, string validationPID = "")
        {
            var parameterInfo = new StringBuilder();
            parameterInfo.AppendFormat("/hfsPin={0}",     pinNumber);
            parameterInfo.AppendFormat("/hfsDocCode={0}", "WEP");
            var reply = SearchMetaData(ref cmServiceRequest, hfsPin: pinNumber, hfsDocCode: "WEP", throwIfEmpty: true);

            if (reply?.ResultSet == null || (reply.ResultSet.count <= 0)) return null;
            if (reply.ResultSet.count <= 1) return reply;
            // TODO: Edge case: more than one result so match on validationPID
            if (validationPID.Length > 0)
            {
            }

            return reply;
        }

        /// <summary>
        /// Stores the specified document into CM
        /// </summary>
        /// <returns></returns>
        public static string Store(
            ref ICMServiceRequest cmServiceRequest,
            byte[]                inputDocument,
            string                hfsCaseNumber,
            string                hfsClaimNumber,
            string                categoryCode,
            string                hfsDocCode,
            string                hfsDocType,
            string                dcfDocName,
            decimal               dcfDocNum           = 0,
            string                hfsCorrCntlNum      = "",
            string                hfsIndexDate        = "",
            string                hfsIXStationId      = "",
            string                hfsProcessStatus    = "",
            string                hfsRFANum           = "",
            string                hfsRFAPin           = "",
            string                hfsSsn              = "",
            string                hfsPin              = "",
            string                hfsLastName         = "",
            string                hfsFirstName        = "",
            string                hfsDob              = "",
            string                hfsCounty           = "",
            string                ecfReceiveDate      = "",
            string                hfsScannedDate      = "",
            string                hfsComments         = "",
            string                entDocLangType      = "",
            bool                  throwIfEmpty        = false,
            bool                  retrieveAttachments = true)
        {
            if (hfsDocCode == null) throw new ArgumentNullException(nameof(hfsDocCode));
            if (hfsDocType == null) throw new ArgumentNullException(nameof(hfsDocType));
            var    service      = GetProxy(true, ref cmServiceRequest);
            string fileMimeType = "";
            var    request      = new CreateItemRequest { AuthenticationData = GetAuthentication(ref cmServiceRequest) };

            if (dcfDocName.LastIndexOf('.') > 0)
            {
                var fileExtension = dcfDocName.Substring(dcfDocName.LastIndexOf('.') + 1);
                fileMimeType = new ContentManagerMIMETypes().GetMIMEType(fileExtension);
            }

            if (entDocLangType == "")
            {
                entDocLangType = "EN";
            }

            hfsDocCode = "WEP";

            hfsDocType = "W-2/FSET Employability Plan";

            var myItem = new ECF_Document()
                         {
                             HFS_CaseNumber    = hfsCaseNumber.PadLeft(10, '0'),
                             HFS_Doc_Code      = hfsDocCode,
                             HFS_Doc_Type      = hfsDocType,
                             HFS_CorrCntlNum   = "W" + dcfDocNum,
                             ENT_Doc_Lang_Type = entDocLangType
                         };


            if (hfsSsn != "")
            {
                myItem.HFS_SSN = hfsSsn;
            }

            if (hfsPin != "")
            {
                myItem.HFS_PIN = hfsPin;
            }

            if (hfsLastName != "")
            {
                myItem.HFS_Last_Name = hfsLastName;
            }

            if (hfsFirstName != "")
            {
                myItem.HFS_First_Name = hfsFirstName;
            }

            if (hfsDob != "")
            {
                myItem.HFS_DOB = hfsDob;
            }

            if (hfsCounty != "")
            {
                myItem.HFS_County = hfsCounty;
            }

            if (ecfReceiveDate == "")
            {
                myItem.ECF_ReceiveDate = DateTime.Now.ToShortDateString();
            }

            if (hfsScannedDate == "")
            {
                myItem.HFS_ScannedDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            myItem.HFS_ScannedDateSpecified = true;

            if (hfsIndexDate == "")
            {
                myItem.HFS_IndexDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }

            if (hfsComments != "")
            {
                myItem.HFS_Comments = hfsComments;
            }


            var          iBase = new ICMBASE();
            const string label = "File";
            myItem.ICMBASE = new[] { iBase };
            iBase.resourceObject = new LobObjectType
                                   {
                                       label    = new LobObjectTypeLabel { name = label },
                                       MIMEType = fileMimeType
                                   };
            request.Item = new CreateItemRequestItem
                           {
                               ItemXML = new ItemXML()
                           };
            request.Item.ItemXML.ECF_Document = myItem;
            var attachment = new MTOMAttachment[1];
            attachment[0] = new MTOMAttachment
                            {
                                ID       = label,
                                MimeType = fileMimeType,
                                Value    = inputDocument
                            };
            request.mtomRef = attachment;

            var reply = service.CreateItem(request);
            

            if (reply.RequestStatus.success == false)
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Unable to store Document #B{0} into Content Manager on " + DateTime.Now + ". ", dcfDocNum);
                sb.AppendLine();
                if (reply.RequestStatus.ErrorData == null || reply.RequestStatus.ErrorData.Length <= 0)
                {
                    //Logger.Log.Error(sb.ToString());
                    throw new ApplicationException(sb.ToString());
                }

                for (var i = 0; i <= reply.RequestStatus.ErrorData.Length - 1; i++)
                {
                    sb.AppendFormat("Error {0}: ReturnCode: {1} Message: {2} StackTrace: {3} ", i, reply.RequestStatus.ErrorData[i].returnCode, reply.RequestStatus.ErrorData[i].errorMessage, reply.RequestStatus.ErrorData[i].stackTrace);
                    sb.AppendLine();
                }

                //Logger.Log.Error(sb.ToString());
                throw new ApplicationException(sb.ToString());
            }
            else
            {
                return reply.Item.URI;
            }
        }
    }
}
