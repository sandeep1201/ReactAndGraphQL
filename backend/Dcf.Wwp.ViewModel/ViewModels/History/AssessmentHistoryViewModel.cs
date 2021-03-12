using System;
using System.Text;
using System.Xml.Linq;
using Dcf.Wwp.Api.Library.ViewModels.ParticipantBarrierApp;
using Newtonsoft.Json;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.History
{
    public class AssessmentHistoryViewModel : BaseInformalAssessmentViewModel//, ParticipantBarrierDetailViewModel
    {
        public AssessmentHistoryViewModel(IRepository repository, IAuthUser authUser) : base(repository, authUser)
        {
        }

        public object GetSectionHistory(string section, string pin, int? id)
        {
            if (InformalAssessment == null)
                throw new InvalidOperationException("Assessment is empty for participant.");

            string storedProcedureName;
            string tableName;

            switch (section)
            {
                case "languages":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_LS]";
                    tableName           = "LanguageSection";
                    break;

                case "work-history":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_WHS]";
                    tableName           = "WorkHistorySection";
                    break;

                case "work-programs":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_WPS]";
                    tableName           = "WorkProgramSection";
                    break;

                case "education":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_EHS]";
                    tableName           = "EducationSection";
                    break;

                case "post-secondary":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_PSES]";
                    tableName           = "PostSecondaryEducationSection";
                    break;

                case "military":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_MSS]";
                    tableName           = "MilitaryTrainingSection";
                    break;

                case "housing":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_HS]";
                    tableName           = "HousingSection";
                    break;

                case "transportation":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_TS]";
                    tableName           = "TransportationSection";
                    break;

                case "legal-issues":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_LIS]";
                    tableName           = "LegalIssuesSection";
                    break;

                case "child-youth-supports":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_CYS]";
                    tableName           = "ChildYouthSection";
                    break;

                case "participant-barriers":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_PBS]";
                    tableName           = "BarrierSection";
                    break;

                case "family-barriers":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_FBS]";
                    tableName           = "FamilyBarriersSection";
                    break;

                case "non-custodial-parents-referral":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_NCPRS]";
                    tableName           = "NonCustodialParentsReferralSection";
                    break;

                case "non-custodial-parents":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_NCPS]";
                    tableName           = "NonCustodialParentsSection";
                    break;

                default:
                    throw new ArgumentException("Section is invalid.");
            }

            var jsonFromStoredProcedure = GetJsonFromStoredProcedure(storedProcedureName, tableName, pin, id: null);
            return jsonFromStoredProcedure;
        }

        public object GetAppSectionHistory(string section, string pin, int? id)
        {
            string storedProcedureName;
            string tableName;

            switch (section)
            {
                case "participant-barriers-app":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_PBA]";
                    tableName           = "BarrierDetail";
                    break;

                case "work-history-app":
                    storedProcedureName = "[wwp].[SP_ReadCDCHistory_WHA]";
                    tableName           = "EmploymentInformation";
                    break;

                default:
                    throw new ArgumentException("Section is invalid.");
            }

            var jsonFromStoredProcedure = GetJsonFromStoredProcedure(storedProcedureName, tableName, pin: null, id: id);
            return jsonFromStoredProcedure;
        }

        private string GetJsonFromStoredProcedure(string storedProcedureName, string tableName, string pin, int? id)
        {
            var xml = Repo.SectionHistory(storedProcedureName, tableName, pin, id);
            if (string.IsNullOrWhiteSpace(xml))
                return "[]";

            var xDoc = XDocument.Parse(xml);

            var jsonString = JsonConvert.SerializeXNode(xDoc, Formatting.None, true);
            jsonString     = CleanJsonFromXml(jsonString);
            dynamic json   = JsonConvert.DeserializeObject(jsonString);
            jsonString     = JsonConvert.SerializeObject(json[tableName]);

            // We have a case where if there is only a single history record, it is not
            // coming back in a JSON array.  To work around this, we'll simply wrap it
            // in brackets.
            if (jsonString.StartsWith("{") && jsonString.EndsWith("}"))
                jsonString = $"[{jsonString}]";

            return jsonString;
        }

        private string CleanJsonFromXml(string xmlJson)
        {
            StringBuilder sb = new StringBuilder(xmlJson);
            string newXmlJson = sb.Replace("\"true\"", "true")
                                  .Replace("\"false\"", "false")
                                  .Replace("\"null\"", "null")
                                  .Replace(",{\"NULL\":null}", "")
                                  .Replace(":{\"NULL\":null}", ":[]")
                                  .ToString();
            //xmlJson = xmlJson.Replace("\"true\"", "true");
            //xmlJson = xmlJson.Replace("\"false\"", "false");
            //return xmlJson.Replace("\"null\"", "null");

            return newXmlJson;
        }
    }

    internal class LookupValue
    {
        public int    Id    { get; set; }
        public string Value { get; set; }

        public static LookupValue Create(int id, string value)
        {
            return new LookupValue { Id = id, Value = value };
        }
    }
}
