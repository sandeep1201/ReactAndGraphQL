using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dcf.Wwp.ConnectedServices.Mci
{
    #region Interface / Operation Contracts

    public interface IMCIServiceChannel : IMciService, IClientChannel
    {
    }

    [ServiceContract(Namespace = "http://MCIService.services.business.mci.wisconsin.gov", ConfigurationName = "MCIServiceSoap")]
    public interface IMciService
    {
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        AddAliasResponse AddAlias(AddAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        Task<AddAliasResponse> AddAliasAsync(AddAliasRequest request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        ClearResponse Clear(ClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        Task<ClearResponse> ClearAsync(ClearRequest request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        DeleteResponse Delete(DeleteRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        Task<DeleteResponse> DeleteAsync(DeleteRequest1 request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        EstablishResponse Establish(EstablishRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        Task<EstablishResponse> EstablishAsync(EstablishRequest1 request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        RemoveAliasResponse RemoveAlias(RemoveAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        Task<RemoveAliasResponse> RemoveAliasAsync(RemoveAliasRequest request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        ReverifyResponse Reverify(ReverifyRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        Task<ReverifyResponse> ReverifyAsync(ReverifyRequest1 request);

        //[MciFormatMessage]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        SearchByDemographicsResponse SearchByDemographics(SearchByDemographicsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        SelectResponse Select(SelectRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        Task<SelectResponse> SelectAsync(SelectRequest1 request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        UpdateResponse Update(UpdateRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        Task<UpdateResponse> UpdateAsync(UpdateRequest1 request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        RetrieveAlertsResponse RetrieveAlerts(RetrieveAlertsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        Task<RetrieveAlertsResponse> RetrieveAlertsAsync(RetrieveAlertsRequest request);

        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        AlternateClearResponse AlternateClear(AlternateClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        Task<AlternateClearResponse> AlternateClearAsync(AlternateClearRequest request);
    }

    #endregion

    #region Client

    [DebuggerStepThrough]
    public class MCIService : ClientBase<IMciService>, IMciService
    {
        public MCIService()
        {
        }

        //public MCIService(string endpointConfigurationName) :
        //    base(endpointConfigurationName)
        //{
        //}

        //public MCIService(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        //{
        //}

        //public MCIService(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        //{
        //}

        public MCIService(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        public AddAliasResponse AddAlias(AddAliasRequest request) => Channel.AddAlias(request);

        public Task<AddAliasResponse> AddAliasAsync(AddAliasRequest request) => Channel.AddAliasAsync(request);

        public ClearResponse Clear(ClearRequest request) => Channel.Clear(request);

        public Task<ClearResponse> ClearAsync(ClearRequest request) => Channel.ClearAsync(request);

        public DeleteResponse Delete(DeleteRequest1 request) => Channel.Delete(request);

        public Task<DeleteResponse> DeleteAsync(DeleteRequest1 request) => Channel.DeleteAsync(request);

        public EstablishResponse Establish(EstablishRequest1 request) => Channel.Establish(request);

        public Task<EstablishResponse> EstablishAsync(EstablishRequest1 request) => Channel.EstablishAsync(request);

        public RemoveAliasResponse RemoveAlias(RemoveAliasRequest request) => Channel.RemoveAlias(request);

        public Task<RemoveAliasResponse> RemoveAliasAsync(RemoveAliasRequest request) => Channel.RemoveAliasAsync(request);

        public ReverifyResponse Reverify(ReverifyRequest1 request) => Channel.Reverify(request);

        public Task<ReverifyResponse> ReverifyAsync(ReverifyRequest1 request) => Channel.ReverifyAsync(request);

        public SearchByDemographicsResponse SearchByDemographics(SearchByDemographicsRequest request) => Channel.SearchByDemographics(request);

        public Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request) => Channel.SearchByDemographicsAsync(request);

        public SelectResponse Select(SelectRequest1 request) => Channel.Select(request);

        public Task<SelectResponse> SelectAsync(SelectRequest1 request) => Channel.SelectAsync(request);

        public UpdateResponse Update(UpdateRequest1 request) => Channel.Update(request);

        public Task<UpdateResponse> UpdateAsync(UpdateRequest1 request) => Channel.UpdateAsync(request);

        public RetrieveAlertsResponse RetrieveAlerts(RetrieveAlertsRequest request) => Channel.RetrieveAlerts(request);

        public Task<RetrieveAlertsResponse> RetrieveAlertsAsync(RetrieveAlertsRequest request) => Channel.RetrieveAlertsAsync(request);

        public AlternateClearResponse AlternateClear(AlternateClearRequest request) => Channel.AlternateClear(request);

        public Task<AlternateClearResponse> AlternateClearAsync(AlternateClearRequest request) => Channel.AlternateClearAsync(request);
    }

    #endregion

    #region Message Contracts

    [DebuggerStepThrough]
    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AliasRequest
    {
        [XmlElement(Order = 0)]
        public long MCIID { get; set; }

        [XmlElement(Order = 1)]
        public string UserName { get; set; }

        [XmlArray(Order = 2)]
        public decimal[] AliasSSN { get; set; }

        [XmlArray(Order = 3)]
        public Name[] AliasName { get; set; }

        [XmlElement(Order = 4)]
        public long SSN { get; set; }

        [XmlElement(Order = 5)]
        public string ReasonForRequest { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Name
    {
        private string firstNameField;

        private string lastNameField;

        private string middleInitialField;

        private string suffixNameField;

        private NameType nameTypeField;

        [XmlElement(Order = 0)]
        public string FirstName
        {
            get => firstNameField;
            set => firstNameField = value;
        }

        [XmlElement(Order = 1)]
        public string LastName
        {
            get => lastNameField;
            set => lastNameField = value;
        }

        [XmlElement(Order = 2)]
        public string MiddleInitial
        {
            get => middleInitialField;
            set => middleInitialField = value;
        }

        [XmlElement(Order = 3)]
        public string SuffixName
        {
            get => suffixNameField;
            set => suffixNameField = value;
        }

        [XmlElement(Order = 4)]
        public NameType NameType
        {
            get => nameTypeField;
            set => nameTypeField = value;
        }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum NameType
    {
        PrimaryName,
        Alias,
        MaidenName
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearanceResult
    {
        private ValidationError[] errorsField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ValidationError
    {
        private string fieldNameField;

        private string errorCodeField;

        private string errorMessageField;

        [XmlElement(Order = 0)]
        public string FieldName
        {
            get => fieldNameField;
            set => fieldNameField = value;
        }

        [XmlElement(Order = 1)]
        public string ErrorCode
        {
            get => errorCodeField;
            set => errorCodeField = value;
        }

        [XmlElement(Order = 2)]
        public string ErrorMessage
        {
            get => errorMessageField;
            set => errorMessageField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertResult
    {
        private ValidationError[] errorsField;

        private Alert[] alertsField;

        private bool moreAlertsExistField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }

        [XmlArray(Order = 1)]
        public Alert[] Alerts
        {
            get => alertsField;
            set => alertsField = value;
        }

        [XmlElement(Order = 2)]
        public bool MoreAlertsExist
        {
            get => moreAlertsExistField;
            set => moreAlertsExistField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Alert : DemographicDetails
    {
        private int alertSequenceNumberField;

        private decimal[] aliasSSNField;

        private string alertCodeField;

        private DateTime alertDateField;

        private string alertUserIDField;

        private long correctedSSNField;

        private DateTime dODField;

        private string mCISSNVerificationCodeField;

        private string errorConditionCodeField;

        private string soundexField;

        private DateTime sSNMatchDateField;

        private long mCIIDField;

        private string reasonForRequestField;

        private bool title16Field;

        private bool title2Field;

        private string discrepancyCodeField;

        [XmlElement(Order = 0)]
        public int AlertSequenceNumber
        {
            get => alertSequenceNumberField;
            set => alertSequenceNumberField = value;
        }

        [XmlArray(Order = 1)]
        public decimal[] AliasSSN
        {
            get => aliasSSNField;
            set => aliasSSNField = value;
        }

        [XmlElement(Order = 2)]
        public string AlertCode
        {
            get => alertCodeField;
            set => alertCodeField = value;
        }

        [XmlElement(Order = 3)]
        public DateTime AlertDate
        {
            get => alertDateField;
            set => alertDateField = value;
        }

        [XmlElement(Order = 4)]
        public string AlertUserID
        {
            get => alertUserIDField;
            set => alertUserIDField = value;
        }

        [XmlElement(Order = 5)]
        public long CorrectedSSN
        {
            get => correctedSSNField;
            set => correctedSSNField = value;
        }

        [XmlElement(Order = 6)]
        public DateTime DOD
        {
            get => dODField;
            set => dODField = value;
        }

        [XmlElement(Order = 7)]
        public string MCISSNVerificationCode
        {
            get => mCISSNVerificationCodeField;
            set => mCISSNVerificationCodeField = value;
        }

        [XmlElement(Order = 8)]
        public string ErrorConditionCode
        {
            get => errorConditionCodeField;
            set => errorConditionCodeField = value;
        }

        [XmlElement(Order = 9)]
        public string Soundex
        {
            get => soundexField;
            set => soundexField = value;
        }

        [XmlElement(Order = 10)]
        public DateTime SSNMatchDate
        {
            get => sSNMatchDateField;
            set => sSNMatchDateField = value;
        }

        [XmlElement(Order = 11)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlElement(Order = 12)]
        public string ReasonForRequest
        {
            get => reasonForRequestField;
            set => reasonForRequestField = value;
        }

        [XmlElement(Order = 13)]
        public bool Title16
        {
            get => title16Field;
            set => title16Field = value;
        }

        [XmlElement(Order = 14)]
        public bool Title2
        {
            get => title2Field;
            set => title2Field = value;
        }

        [XmlElement(Order = 15)]
        public string DiscrepancyCode
        {
            get => discrepancyCodeField;
            set => discrepancyCodeField = value;
        }
    }

    [XmlInclude(typeof(AlternateClearanceRequest))]
    [XmlInclude(typeof(Alert))]
    [XmlInclude(typeof(UpdateRequest))]
    [XmlInclude(typeof(ReverifyRequest))]
    [XmlInclude(typeof(Match))]
    [XmlInclude(typeof(DemographicMatch))]
    [XmlInclude(typeof(DemographicSearchMatch))]
    [XmlInclude(typeof(ClearanceMatch))]
    [XmlInclude(typeof(ClearanceRequest))]
    [XmlInclude(typeof(SelectRequest))]
    [XmlInclude(typeof(EstablishRequest))]
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicDetails
    {
        private Name primaryNameField;

        private long sSNField;

        private DateTime dOBField;

        private string genderField;

        private string sSNVerificationCodeField;

        [XmlElement(Order = 0)]
        public Name PrimaryName
        {
            get => primaryNameField;
            set => primaryNameField = value;
        }

        [XmlElement(Order = 1)]
        public long SSN
        {
            get => sSNField;
            set => sSNField = value;
        }

        [XmlElement(Order = 2)]
        public DateTime DOB
        {
            get => (new DateTime(dOBField.Year, dOBField.Month, dOBField.Day)); // this serializes as '<DOB>2018-10-26T00:00:00</DOB>' for Deloitte 
            set => dOBField = value;
        }

        [XmlElement(Order = 3)]
        public string Gender
        {
            get => genderField;
            set => genderField = value;
        }

        [XmlElement(Order = 4)]
        public string SSNVerificationCode
        {
            get => sSNVerificationCodeField;
            set => sSNVerificationCodeField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearanceRequest : DemographicDetails
    {
        private long mCIIDField;

        private string referralApplicationField;

        private string userNameField;

        private string reasonForRequestField;

        [XmlElement(Order = 0)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlElement(Order = 1)]
        public string ReferralApplication
        {
            get => referralApplicationField;
            set => referralApplicationField = value;
        }

        [XmlElement(Order = 2)]
        public string UserName
        {
            get => userNameField;
            set => userNameField = value;
        }

        [XmlElement(Order = 3)]
        public string ReasonForRequest
        {
            get => reasonForRequestField;
            set => reasonForRequestField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateRequest : DemographicDetails
    {
        private long mCIIDField;

        private OldNameType storePreviousNameField;

        private OldSSNType storePreviousSSNField;

        private string userNameField;

        private string reasonForRequestField;

        [XmlElement(Order = 0)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlElement(Order = 1)]
        public OldNameType StorePreviousName
        {
            get => storePreviousNameField;
            set => storePreviousNameField = value;
        }

        [XmlElement(Order = 2)]
        public OldSSNType StorePreviousSSN
        {
            get => storePreviousSSNField;
            set => storePreviousSSNField = value;
        }

        [XmlElement(Order = 3)]
        public string UserName
        {
            get => userNameField;
            set => userNameField = value;
        }

        [XmlElement(Order = 4)]
        public string ReasonForRequest
        {
            get => reasonForRequestField;
            set => reasonForRequestField = value;
        }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum OldNameType
    {
        History,

        Alias,

        MaidenName
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum OldSSNType
    {
        History,

        Alias
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyRequest : DemographicDetails
    {
        private long mCIIDField;

        private string userNameField;

        private string reasonForRequestField;

        [XmlElement(Order = 0)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlElement(Order = 1)]
        public string UserName
        {
            get => userNameField;
            set => userNameField = value;
        }

        [XmlElement(Order = 2)]
        public string ReasonForRequest
        {
            get => reasonForRequestField;
            set => reasonForRequestField = value;
        }
    }

    [XmlInclude(typeof(DemographicMatch))]
    [XmlInclude(typeof(DemographicSearchMatch))]
    [XmlInclude(typeof(ClearanceMatch))]
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Match : DemographicDetails
    {
        private short sequenceNumberField;

        private long mCIIDField;

        private string sSNVerificationCodeDescriptionField;

        private bool isMCIIDKnownField;

        private bool isSequenceNumberKnownField;

        private string recordTypeField;

        [XmlElement(Order = 0)]
        public short SequenceNumber
        {
            get => sequenceNumberField;
            set => sequenceNumberField = value;
        }

        [XmlElement(Order = 1)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlElement(Order = 2)]
        public string SSNVerificationCodeDescription
        {
            get => sSNVerificationCodeDescriptionField;
            set => sSNVerificationCodeDescriptionField = value;
        }

        [XmlElement(Order = 3)]
        public bool IsMCIIDKnown
        {
            get => isMCIIDKnownField;
            set => isMCIIDKnownField = value;
        }

        [XmlElement(Order = 4)]
        public bool IsSequenceNumberKnown
        {
            get => isSequenceNumberKnownField;
            set => isSequenceNumberKnownField = value;
        }

        [XmlElement(Order = 5)]
        public string RecordType
        {
            get => recordTypeField;
            set => recordTypeField = value;
        }
    }

    [XmlInclude(typeof(DemographicSearchMatch))]
    [XmlInclude(typeof(ClearanceMatch))]
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicMatch : Match
    {
        private string matchTypeField;

        private short matchScoreField;

        private Name aliasNameField;

        [XmlElement(Order = 0)]
        public string MatchType
        {
            get => matchTypeField;
            set => matchTypeField = value;
        }

        [XmlElement(Order = 1)]
        public short MatchScore
        {
            get => matchScoreField;
            set => matchScoreField = value;
        }

        [XmlElement(Order = 2)]
        public Name AliasName
        {
            get => aliasNameField;
            set => aliasNameField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchMatch : DemographicMatch
    {
        private bool otherNamesExistField;

        private bool aliasSSNsExistField;

        [XmlElement(Order = 0)]
        public bool OtherNamesExist
        {
            get => otherNamesExistField;
            set => otherNamesExistField = value;
        }

        [XmlElement(Order = 1)]
        public bool AliasSSNsExist
        {
            get => aliasSSNsExistField;
            set => aliasSSNsExistField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceMatch : DemographicMatch
    {
    }

    [XmlInclude(typeof(SelectRequest))]
    [XmlInclude(typeof(EstablishRequest))]
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceRequest : DemographicDetails
    {
        private short minimumMatchScoreField;

        private string aliasSSNField;

        private Name[] aliasNameField;

        private string userNameField;

        private string reasonForRequestField;

        [XmlElement(Order = 0)]
        public short MinimumMatchScore
        {
            get => minimumMatchScoreField;
            set => minimumMatchScoreField = value;
        }

        [XmlElement(Order = 1)]
        public string AliasSSN
        {
            get => aliasSSNField;
            set => aliasSSNField = value;
        }

        [XmlArray(Order = 2)]
        public Name[] AliasName
        {
            get => aliasNameField;
            set => aliasNameField = value;
        }

        [XmlElement(Order = 3)]
        public string UserName
        {
            get => userNameField;
            set => userNameField = value;
        }

        [XmlElement(Order = 4)]
        public string ReasonForRequest
        {
            get => reasonForRequestField;
            set => reasonForRequestField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectRequest : ClearanceRequest
    {
        private long mCIIDField;

        private short sequenceNumberField;

        private string requestIDField;

        [XmlElement(Order = 0)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlElement(Order = 1)]
        public short SequenceNumber
        {
            get => sequenceNumberField;
            set => sequenceNumberField = value;
        }

        [XmlElement(Order = 2)]
        public string RequestID
        {
            get => requestIDField;
            set => requestIDField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class EstablishRequest : ClearanceRequest
    {
        [XmlElement(Order = 0)]
        public string RequestID { get; set; }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertRequest
    {
        private short alertSequenceNumberField;

        [XmlElement(Order = 0)]
        public short AlertSequenceNumber
        {
            get => alertSequenceNumberField;
            set => alertSequenceNumberField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateResult
    {
        private ValidationError[] errorsField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectResult
    {
        private ValidationError[] errorsField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchResult
    {
        private ValidationError[] errorsField;

        private DemographicSearchMatch[] matchField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }

        [XmlArray(Order = 1)]
        public DemographicSearchMatch[] Match
        {
            get => matchField;
            set => matchField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchRequest
    {
        private Name primaryNameField;

        private long sSNField;

        private DateTime dOBField;

        private string genderField;

        private short minimumMatchScoreField;

        private bool includeAliasSSNField;

        private Name[] aliasNameField;

        private string userNameField;

        private string reasonForRequestField;

        [XmlElement(Order = 0)]
        public Name PrimaryName
        {
            get => primaryNameField;
            set => primaryNameField = value;
        }

        [XmlElement(Order = 1)]
        public long SSN
        {
            get => sSNField;
            set => sSNField = value;
        }

        [XmlElement(Order = 2)]
        public DateTime DOB
        {
            get => (new DateTime(dOBField.Year, dOBField.Month, dOBField.Day)); // this serializes as '<DOB>2018-10-26T00:00:00</DOB>' for Deloitte 
            set => dOBField = value;
        }

        [XmlElement(Order = 3)]
        public string Gender
        {
            get => genderField;
            set => genderField = value;
        }

        [XmlElement(Order = 4)]
        public short MinimumMatchScore
        {
            get => minimumMatchScoreField;
            set => minimumMatchScoreField = value;
        }

        [XmlElement(Order = 5)]
        public bool IncludeAliasSSN
        {
            get => includeAliasSSNField;
            set => includeAliasSSNField = value;
        }

        [XmlArray(Order = 6)]
        public Name[] AliasName
        {
            get => aliasNameField;
            set => aliasNameField = value;
        }

        [XmlElement(Order = 7)]
        public string UserName
        {
            get => userNameField;
            set => userNameField = value;
        }

        [XmlElement(Order = 8)]
        public string ReasonForRequest
        {
            get => reasonForRequestField;
            set => reasonForRequestField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyResult
    {
        private ValidationError[] errorsField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class EstablishResult
    {
        private long mCIIDField;

        private ValidationError[] errorsField;

        [XmlElement(Order = 0)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlArray(Order = 1)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteResult
    {
        private ValidationError[] errorsField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteRequest
    {
        private long mCIIDField;

        private string userNameField;

        private string reasonForRequestField;

        [XmlElement(Order = 0)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlElement(Order = 1)]
        public string UserName
        {
            get => userNameField;
            set => userNameField = value;
        }

        [XmlElement(Order = 2)]
        public string ReasonForRequest
        {
            get => reasonForRequestField;
            set => reasonForRequestField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceResult
    {
        private ClearanceMatch[] matchField;

        private string requestIDField;

        private long mCIIDField;

        private ValidationError[] errorsField;

        [XmlArray(Order = 0)]
        public ClearanceMatch[] Match
        {
            get => matchField;
            set => matchField = value;
        }

        [XmlElement(Order = 1)]
        public string RequestID
        {
            get => requestIDField;
            set => requestIDField = value;
        }

        [XmlElement(Order = 2)]
        public long MCIID
        {
            get => mCIIDField;
            set => mCIIDField = value;
        }

        [XmlArray(Order = 3)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AliasResult
    {
        private ValidationError[] errorsField;

        [XmlArray(Order = 0)]
        public ValidationError[] Errors
        {
            get => errorsField;
            set => errorsField = value;
        }
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "AddAlias", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class AddAliasRequest
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasRequest RequestDetails;

        public AddAliasRequest()
        {
        }

        public AddAliasRequest(AliasRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "AddAliasResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class AddAliasResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasResult AddAliasResult;

        public AddAliasResponse()
        {
        }

        public AddAliasResponse(AliasResult AddAliasResult) => this.AddAliasResult = AddAliasResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "Clear", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class ClearRequest
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ClearanceRequest RequestDetails;

        public ClearRequest()
        {
        }

        public ClearRequest(ClearanceRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "ClearResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class ClearResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ClearanceResult ClearResult;

        public ClearResponse()
        {
        }

        public ClearResponse(ClearanceResult ClearResult) => this.ClearResult = ClearResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "Delete", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class DeleteRequest1
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DeleteRequest DeleteDetails;

        public DeleteRequest1()
        {
        }

        public DeleteRequest1(DeleteRequest DeleteDetails) => this.DeleteDetails = DeleteDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "DeleteResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class DeleteResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DeleteResult DeleteResult;

        public DeleteResponse()
        {
        }

        public DeleteResponse(DeleteResult DeleteResult) => this.DeleteResult = DeleteResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "Establish", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class EstablishRequest1
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public EstablishRequest EstablishDetails;

        public EstablishRequest1()
        {
        }

        public EstablishRequest1(EstablishRequest EstablishDetails) => this.EstablishDetails = EstablishDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "EstablishResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class EstablishResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public EstablishResult EstablishResult;

        public EstablishResponse()
        {
        }

        public EstablishResponse(EstablishResult EstablishResult) => this.EstablishResult = EstablishResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "RemoveAlias", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class RemoveAliasRequest
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasRequest RequestDetails;

        public RemoveAliasRequest()
        {
        }

        public RemoveAliasRequest(AliasRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "RemoveAliasResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class RemoveAliasResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasResult RemoveAliasResult;

        public RemoveAliasResponse()
        {
        }

        public RemoveAliasResponse(AliasResult RemoveAliasResult) => this.RemoveAliasResult = RemoveAliasResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "Reverify", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class ReverifyRequest1
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ReverifyRequest ReverifyDetails;

        public ReverifyRequest1()
        {
        }

        public ReverifyRequest1(ReverifyRequest ReverifyDetails) => this.ReverifyDetails = ReverifyDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "ReverifyResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class ReverifyResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ReverifyResult ReverifyResult;

        public ReverifyResponse()
        {
        }

        public ReverifyResponse(ReverifyResult ReverifyResult) => this.ReverifyResult = ReverifyResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "SearchByDemographics", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class SearchByDemographicsRequest
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DemographicSearchRequest SearchDetails;

        public SearchByDemographicsRequest()
        {
        }

        public SearchByDemographicsRequest(DemographicSearchRequest SearchDetails) => this.SearchDetails = SearchDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "SearchByDemographicsResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class SearchByDemographicsResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DemographicSearchResult SearchByDemographicsResult;

        public SearchByDemographicsResponse()
        {
        }

        public SearchByDemographicsResponse(DemographicSearchResult SearchByDemographicsResult) => this.SearchByDemographicsResult = SearchByDemographicsResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "Select", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class SelectRequest1
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SelectRequest RequestDetails;

        public SelectRequest1()
        {
        }

        public SelectRequest1(SelectRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "SelectResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class SelectResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SelectResult SelectResult;

        public SelectResponse()
        {
        }

        public SelectResponse(SelectResult SelectResult) => this.SelectResult = SelectResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "Update", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class UpdateRequest1
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public UpdateRequest UpdateDetails;

        public UpdateRequest1()
        {
        }

        public UpdateRequest1(UpdateRequest UpdateDetails) => this.UpdateDetails = UpdateDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "UpdateResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class UpdateResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public UpdateResult UpdateResult;

        public UpdateResponse()
        {
        }

        public UpdateResponse(UpdateResult UpdateResult) => this.UpdateResult = UpdateResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "RetrieveAlerts", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class RetrieveAlertsRequest
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RetrieveAlertRequest RequestDetails;

        public RetrieveAlertsRequest()
        {
        }

        public RetrieveAlertsRequest(RetrieveAlertRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "RetrieveAlertsResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class RetrieveAlertsResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RetrieveAlertResult RetrieveAlertsResult;

        public RetrieveAlertsResponse()
        {
        }

        public RetrieveAlertsResponse(RetrieveAlertResult RetrieveAlertsResult) => this.RetrieveAlertsResult = RetrieveAlertsResult;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "AlternateClear", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class AlternateClearRequest
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AlternateClearanceRequest RequestDetails;

        public AlternateClearRequest()
        {
        }

        public AlternateClearRequest(AlternateClearanceRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(WrapperName = "AlternateClearResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public class AlternateClearResponse
    {
        [MessageBodyMember(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AlternateClearanceResult AlternateClearResult;

        public AlternateClearResponse()
        {
        }

        public AlternateClearResponse(AlternateClearanceResult AlternateClearResult) => this.AlternateClearResult = AlternateClearResult;
    }

    #endregion
}

// http://www.w3.org/TR/xmlschema-2/#date