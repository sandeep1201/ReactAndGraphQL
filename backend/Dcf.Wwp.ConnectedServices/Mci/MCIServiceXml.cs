
namespace Dcf.Wwp.ConnectedServices.Mci
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://MCIService.services.business.mci.wisconsin.gov", ConfigurationName = "MCIServiceSoap")]
    public interface IMCIService
    {
        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        AddAliasResponse AddAlias(AddAliasRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        System.Threading.Tasks.Task<AddAliasResponse> AddAliasAsync(AddAliasRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        [MCIFormatMessage]
        ClearResponse Clear(ClearRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        System.Threading.Tasks.Task<ClearResponse> ClearAsync(ClearRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        DeleteResponse Delete(DeleteRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        System.Threading.Tasks.Task<DeleteResponse> DeleteAsync(DeleteRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        EstablishResponse Establish(EstablishRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        System.Threading.Tasks.Task<EstablishResponse> EstablishAsync(EstablishRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        RemoveAliasResponse RemoveAlias(RemoveAliasRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        System.Threading.Tasks.Task<RemoveAliasResponse> RemoveAliasAsync(RemoveAliasRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        ReverifyResponse Reverify(ReverifyRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        System.Threading.Tasks.Task<ReverifyResponse> ReverifyAsync(ReverifyRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        [MCIFormatMessage]
        SearchByDemographicsResponse SearchByDemographics(SearchByDemographicsRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        System.Threading.Tasks.Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        SelectResponse Select(SelectRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        System.Threading.Tasks.Task<SelectResponse> SelectAsync(SelectRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        UpdateResponse Update(UpdateRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        System.Threading.Tasks.Task<UpdateResponse> UpdateAsync(UpdateRequest1 request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        RetrieveAlertsResponse RetrieveAlerts(RetrieveAlertsRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        System.Threading.Tasks.Task<RetrieveAlertsResponse> RetrieveAlertsAsync(RetrieveAlertsRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DemographicDetails))]
        AlternateClearResponse AlternateClear(AlternateClearRequest request);

        [System.ServiceModel.OperationContractAttribute(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        System.Threading.Tasks.Task<AlternateClearResponse> AlternateClearAsync(AlternateClearRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class AliasRequest
    {

        private long mCIIDField;

        private string userNameField;

        private decimal[] aliasSSNField;

        private Name[] aliasNameField;

        private long sSNField;

        private string reasonForRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string UserName
        {
            get { return this.userNameField; }
            set { this.userNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        public decimal[] AliasSSN
        {
            get { return this.aliasSSNField; }
            set { this.aliasSSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        public Name[] AliasName
        {
            get { return this.aliasNameField; }
            set { this.aliasNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public long SSN
        {
            get { return this.sSNField; }
            set { this.sSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class Name
    {

        private string firstNameField;

        private string lastNameField;

        private string middleInitialField;

        private string suffixNameField;

        private NameType nameTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string FirstName
        {
            get { return this.firstNameField; }
            set { this.firstNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string LastName
        {
            get { return this.lastNameField; }
            set { this.lastNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MiddleInitial
        {
            get { return this.middleInitialField; }
            set { this.middleInitialField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string SuffixName
        {
            get { return this.suffixNameField; }
            set { this.suffixNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public NameType NameType
        {
            get { return this.nameTypeField; }
            set { this.nameTypeField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum NameType
    {

        /// <remarks/>
        PrimaryName,

        /// <remarks/>
        Alias,

        /// <remarks/>
        MaidenName,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class AlternateClearanceResult
    {

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class ValidationError
    {

        private string fieldNameField;

        private string errorCodeField;

        private string errorMessageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string FieldName
        {
            get { return this.fieldNameField; }
            set { this.fieldNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ErrorCode
        {
            get { return this.errorCodeField; }
            set { this.errorCodeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ErrorMessage
        {
            get { return this.errorMessageField; }
            set { this.errorMessageField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class RetrieveAlertResult
    {

        private ValidationError[] errorsField;

        private Alert[] alertsField;

        private bool moreAlertsExistField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        public Alert[] Alerts
        {
            get { return this.alertsField; }
            set { this.alertsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public bool MoreAlertsExist
        {
            get { return this.moreAlertsExistField; }
            set { this.moreAlertsExistField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class Alert : DemographicDetails
    {

        private int alertSequenceNumberField;

        private decimal[] aliasSSNField;

        private string alertCodeField;

        private System.DateTime alertDateField;

        private string alertUserIDField;

        private long correctedSSNField;

        private System.DateTime dODField;

        private string mCISSNVerificationCodeField;

        private string errorConditionCodeField;

        private string soundexField;

        private System.DateTime sSNMatchDateField;

        private long mCIIDField;

        private string reasonForRequestField;

        private bool title16Field;

        private bool title2Field;

        private string discrepancyCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int AlertSequenceNumber
        {
            get { return this.alertSequenceNumberField; }
            set { this.alertSequenceNumberField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        public decimal[] AliasSSN
        {
            get { return this.aliasSSNField; }
            set { this.aliasSSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string AlertCode
        {
            get { return this.alertCodeField; }
            set { this.alertCodeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public System.DateTime AlertDate
        {
            get { return this.alertDateField; }
            set { this.alertDateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string AlertUserID
        {
            get { return this.alertUserIDField; }
            set { this.alertUserIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public long CorrectedSSN
        {
            get { return this.correctedSSNField; }
            set { this.correctedSSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public System.DateTime DOD
        {
            get { return this.dODField; }
            set { this.dODField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string MCISSNVerificationCode
        {
            get { return this.mCISSNVerificationCodeField; }
            set { this.mCISSNVerificationCodeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string ErrorConditionCode
        {
            get { return this.errorConditionCodeField; }
            set { this.errorConditionCodeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string Soundex
        {
            get { return this.soundexField; }
            set { this.soundexField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime SSNMatchDate
        {
            get { return this.sSNMatchDateField; }
            set { this.sSNMatchDateField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public bool Title16
        {
            get { return this.title16Field; }
            set { this.title16Field = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public bool Title2
        {
            get { return this.title2Field; }
            set { this.title2Field = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string DiscrepancyCode
        {
            get { return this.discrepancyCodeField; }
            set { this.discrepancyCodeField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AlternateClearanceRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Alert))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UpdateRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReverifyRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Match))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DemographicMatch))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DemographicSearchMatch))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClearanceMatch))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClearanceRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SelectRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EstablishRequest))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class DemographicDetails
    {

        private Name primaryNameField;

        private long sSNField;

        private System.DateTime dOBField;

        private string genderField;

        private string sSNVerificationCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public Name PrimaryName
        {
            get { return this.primaryNameField; }
            set { this.primaryNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public long SSN
        {
            get { return this.sSNField; }
            set { this.sSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.DateTime DOB
        {
            get { return this.dOBField; }
            set { this.dOBField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Gender
        {
            get { return this.genderField; }
            set { this.genderField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string SSNVerificationCode
        {
            get { return this.sSNVerificationCodeField; }
            set { this.sSNVerificationCodeField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class AlternateClearanceRequest : DemographicDetails
    {

        private long mCIIDField;

        private string referralApplicationField;

        private string userNameField;

        private string reasonForRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ReferralApplication
        {
            get { return this.referralApplicationField; }
            set { this.referralApplicationField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string UserName
        {
            get { return this.userNameField; }
            set { this.userNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class UpdateRequest : DemographicDetails
    {

        private long mCIIDField;

        private OldNameType storePreviousNameField;

        private OldSSNType storePreviousSSNField;

        private string userNameField;

        private string reasonForRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public OldNameType StorePreviousName
        {
            get { return this.storePreviousNameField; }
            set { this.storePreviousNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public OldSSNType StorePreviousSSN
        {
            get { return this.storePreviousSSNField; }
            set { this.storePreviousSSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string UserName
        {
            get { return this.userNameField; }
            set { this.userNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum OldNameType
    {

        /// <remarks/>
        History,

        /// <remarks/>
        Alias,

        /// <remarks/>
        MaidenName,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum OldSSNType
    {

        /// <remarks/>
        History,

        /// <remarks/>
        Alias,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class ReverifyRequest : DemographicDetails
    {

        private long mCIIDField;

        private string userNameField;

        private string reasonForRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string UserName
        {
            get { return this.userNameField; }
            set { this.userNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DemographicMatch))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DemographicSearchMatch))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClearanceMatch))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class Match : DemographicDetails
    {

        private short sequenceNumberField;

        private long mCIIDField;

        private string sSNVerificationCodeDescriptionField;

        private bool isMCIIDKnownField;

        private bool isSequenceNumberKnownField;

        private string recordTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public short SequenceNumber
        {
            get { return this.sequenceNumberField; }
            set { this.sequenceNumberField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string SSNVerificationCodeDescription
        {
            get { return this.sSNVerificationCodeDescriptionField; }
            set { this.sSNVerificationCodeDescriptionField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool IsMCIIDKnown
        {
            get { return this.isMCIIDKnownField; }
            set { this.isMCIIDKnownField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool IsSequenceNumberKnown
        {
            get { return this.isSequenceNumberKnownField; }
            set { this.isSequenceNumberKnownField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string RecordType
        {
            get { return this.recordTypeField; }
            set { this.recordTypeField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DemographicSearchMatch))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClearanceMatch))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class DemographicMatch : Match
    {

        private string matchTypeField;

        private short matchScoreField;

        private Name aliasNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string MatchType
        {
            get { return this.matchTypeField; }
            set { this.matchTypeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public short MatchScore
        {
            get { return this.matchScoreField; }
            set { this.matchScoreField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public Name AliasName
        {
            get { return this.aliasNameField; }
            set { this.aliasNameField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class DemographicSearchMatch : DemographicMatch
    {

        private bool otherNamesExistField;

        private bool aliasSSNsExistField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool OtherNamesExist
        {
            get { return this.otherNamesExistField; }
            set { this.otherNamesExistField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool AliasSSNsExist
        {
            get { return this.aliasSSNsExistField; }
            set { this.aliasSSNsExistField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class ClearanceMatch : DemographicMatch
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SelectRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(EstablishRequest))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class ClearanceRequest : DemographicDetails
    {

        private short minimumMatchScoreField;

        private string aliasSSNField;

        private Name[] aliasNameField;

        private string userNameField;

        private string reasonForRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public short MinimumMatchScore
        {
            get { return this.minimumMatchScoreField; }
            set { this.minimumMatchScoreField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AliasSSN
        {
            get { return this.aliasSSNField; }
            set { this.aliasSSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        public Name[] AliasName
        {
            get { return this.aliasNameField; }
            set { this.aliasNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string UserName
        {
            get { return this.userNameField; }
            set { this.userNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class SelectRequest : ClearanceRequest
    {

        private long mCIIDField;

        private short sequenceNumberField;

        private string requestIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public short SequenceNumber
        {
            get { return this.sequenceNumberField; }
            set { this.sequenceNumberField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string RequestID
        {
            get { return this.requestIDField; }
            set { this.requestIDField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class EstablishRequest : ClearanceRequest
    {

        private string requestIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RequestID
        {
            get { return this.requestIDField; }
            set { this.requestIDField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class RetrieveAlertRequest
    {

        private short alertSequenceNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public short AlertSequenceNumber
        {
            get { return this.alertSequenceNumberField; }
            set { this.alertSequenceNumberField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class UpdateResult
    {

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class SelectResult
    {

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class DemographicSearchResult
    {

        private ValidationError[] errorsField;

        private DemographicSearchMatch[] matchField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        public DemographicSearchMatch[] Match
        {
            get { return this.matchField; }
            set { this.matchField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class DemographicSearchRequest
    {

        private Name primaryNameField;

        private long sSNField;

        private System.DateTime dOBField;

        private string genderField;

        private short minimumMatchScoreField;

        private bool includeAliasSSNField;

        private Name[] aliasNameField;

        private string userNameField;

        private string reasonForRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public Name PrimaryName
        {
            get { return this.primaryNameField; }
            set { this.primaryNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public long SSN
        {
            get { return this.sSNField; }
            set { this.sSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.DateTime DOB
        {
            get { return this.dOBField; }
            set { this.dOBField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Gender
        {
            get { return this.genderField; }
            set { this.genderField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public short MinimumMatchScore
        {
            get { return this.minimumMatchScoreField; }
            set { this.minimumMatchScoreField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public bool IncludeAliasSSN
        {
            get { return this.includeAliasSSNField; }
            set { this.includeAliasSSNField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        public Name[] AliasName
        {
            get { return this.aliasNameField; }
            set { this.aliasNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string UserName
        {
            get { return this.userNameField; }
            set { this.userNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class ReverifyResult
    {

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class EstablishResult
    {

        private long mCIIDField;

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class DeleteResult
    {

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class DeleteRequest
    {

        private long mCIIDField;

        private string userNameField;

        private string reasonForRequestField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string UserName
        {
            get { return this.userNameField; }
            set { this.userNameField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ReasonForRequest
        {
            get { return this.reasonForRequestField; }
            set { this.reasonForRequestField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class ClearanceResult
    {

        private ClearanceMatch[] matchField;

        private string requestIDField;

        private long mCIIDField;

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ClearanceMatch[] Match
        {
            get { return this.matchField; }
            set { this.matchField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string RequestID
        {
            get { return this.requestIDField; }
            set { this.requestIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public long MCIID
        {
            get { return this.mCIIDField; }
            set { this.mCIIDField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public partial class AliasResult
    {

        private ValidationError[] errorsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        public ValidationError[] Errors
        {
            get { return this.errorsField; }
            set { this.errorsField = value; }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AddAlias", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class AddAliasRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasRequest RequestDetails;

        public AddAliasRequest()
        {
        }

        public AddAliasRequest(AliasRequest RequestDetails)
        {
            this.RequestDetails = RequestDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AddAliasResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class AddAliasResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasResult AddAliasResult;

        public AddAliasResponse()
        {
        }

        public AddAliasResponse(AliasResult AddAliasResult)
        {
            this.AddAliasResult = AddAliasResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "Clear", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class ClearRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ClearanceRequest RequestDetails;

        public ClearRequest()
        {
        }

        public ClearRequest(ClearanceRequest RequestDetails)
        {
            this.RequestDetails = RequestDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ClearResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class ClearResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ClearanceResult ClearResult;

        public ClearResponse()
        {
        }

        public ClearResponse(ClearanceResult ClearResult)
        {
            this.ClearResult = ClearResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "Delete", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class DeleteRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DeleteRequest DeleteDetails;

        public DeleteRequest1()
        {
        }

        public DeleteRequest1(DeleteRequest DeleteDetails)
        {
            this.DeleteDetails = DeleteDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "DeleteResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class DeleteResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DeleteResult DeleteResult;

        public DeleteResponse()
        {
        }

        public DeleteResponse(DeleteResult DeleteResult)
        {
            this.DeleteResult = DeleteResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "Establish", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class EstablishRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public EstablishRequest EstablishDetails;

        public EstablishRequest1()
        {
        }

        public EstablishRequest1(EstablishRequest EstablishDetails)
        {
            this.EstablishDetails = EstablishDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "EstablishResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class EstablishResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public EstablishResult EstablishResult;

        public EstablishResponse()
        {
        }

        public EstablishResponse(EstablishResult EstablishResult)
        {
            this.EstablishResult = EstablishResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "RemoveAlias", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class RemoveAliasRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasRequest RequestDetails;

        public RemoveAliasRequest()
        {
        }

        public RemoveAliasRequest(AliasRequest RequestDetails)
        {
            this.RequestDetails = RequestDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "RemoveAliasResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class RemoveAliasResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AliasResult RemoveAliasResult;

        public RemoveAliasResponse()
        {
        }

        public RemoveAliasResponse(AliasResult RemoveAliasResult)
        {
            this.RemoveAliasResult = RemoveAliasResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "Reverify", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class ReverifyRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ReverifyRequest ReverifyDetails;

        public ReverifyRequest1()
        {
        }

        public ReverifyRequest1(ReverifyRequest ReverifyDetails)
        {
            this.ReverifyDetails = ReverifyDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "ReverifyResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class ReverifyResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ReverifyResult ReverifyResult;

        public ReverifyResponse()
        {
        }

        public ReverifyResponse(ReverifyResult ReverifyResult)
        {
            this.ReverifyResult = ReverifyResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SearchByDemographics", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class SearchByDemographicsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DemographicSearchRequest SearchDetails;

        public SearchByDemographicsRequest()
        {
        }

        public SearchByDemographicsRequest(DemographicSearchRequest SearchDetails)
        {
            this.SearchDetails = SearchDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SearchByDemographicsResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class SearchByDemographicsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DemographicSearchResult SearchByDemographicsResult;

        public SearchByDemographicsResponse()
        {
        }

        public SearchByDemographicsResponse(DemographicSearchResult SearchByDemographicsResult)
        {
            this.SearchByDemographicsResult = SearchByDemographicsResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "Select", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class SelectRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SelectRequest RequestDetails;

        public SelectRequest1()
        {
        }

        public SelectRequest1(SelectRequest RequestDetails)
        {
            this.RequestDetails = RequestDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "SelectResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class SelectResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SelectResult SelectResult;

        public SelectResponse()
        {
        }

        public SelectResponse(SelectResult SelectResult)
        {
            this.SelectResult = SelectResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "Update", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class UpdateRequest1
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public UpdateRequest UpdateDetails;

        public UpdateRequest1()
        {
        }

        public UpdateRequest1(UpdateRequest UpdateDetails)
        {
            this.UpdateDetails = UpdateDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "UpdateResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class UpdateResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public UpdateResult UpdateResult;

        public UpdateResponse()
        {
        }

        public UpdateResponse(UpdateResult UpdateResult)
        {
            this.UpdateResult = UpdateResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "RetrieveAlerts", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class RetrieveAlertsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RetrieveAlertRequest RequestDetails;

        public RetrieveAlertsRequest()
        {
        }

        public RetrieveAlertsRequest(RetrieveAlertRequest RequestDetails)
        {
            this.RequestDetails = RequestDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "RetrieveAlertsResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class RetrieveAlertsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RetrieveAlertResult RetrieveAlertsResult;

        public RetrieveAlertsResponse()
        {
        }

        public RetrieveAlertsResponse(RetrieveAlertResult RetrieveAlertsResult)
        {
            this.RetrieveAlertsResult = RetrieveAlertsResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AlternateClear", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class AlternateClearRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AlternateClearanceRequest RequestDetails;

        public AlternateClearRequest()
        {
        }

        public AlternateClearRequest(AlternateClearanceRequest RequestDetails)
        {
            this.RequestDetails = RequestDetails;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "AlternateClearResponse", WrapperNamespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", IsWrapped = true)]
    public partial class AlternateClearResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AlternateClearanceResult AlternateClearResult;

        public AlternateClearResponse()
        {
        }

        public AlternateClearResponse(AlternateClearanceResult AlternateClearResult)
        {
            this.AlternateClearResult = AlternateClearResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMCIServiceSoapChannel : IMCIService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MCIService : System.ServiceModel.ClientBase<IMCIService>, IMCIService
    {

        public MCIService()
        {
        }

        public MCIService(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public MCIService(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public MCIService(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public MCIService(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        public AddAliasResponse AddAlias(AddAliasRequest request)
        {
            return base.Channel.AddAlias(request);
        }

        public System.Threading.Tasks.Task<AddAliasResponse> AddAliasAsync(AddAliasRequest request)
        {
            return base.Channel.AddAliasAsync(request);
        }

        public ClearResponse Clear(ClearRequest request)
        {
            return base.Channel.Clear(request);
        }

        public System.Threading.Tasks.Task<ClearResponse> ClearAsync(ClearRequest request)
        {
            return base.Channel.ClearAsync(request);
        }

        public DeleteResponse Delete(DeleteRequest1 request)
        {
            return base.Channel.Delete(request);
        }

        public System.Threading.Tasks.Task<DeleteResponse> DeleteAsync(DeleteRequest1 request)
        {
            return base.Channel.DeleteAsync(request);
        }

        public EstablishResponse Establish(EstablishRequest1 request)
        {
            return base.Channel.Establish(request);
        }

        public System.Threading.Tasks.Task<EstablishResponse> EstablishAsync(EstablishRequest1 request)
        {
            return base.Channel.EstablishAsync(request);
        }

        public RemoveAliasResponse RemoveAlias(RemoveAliasRequest request)
        {
            return base.Channel.RemoveAlias(request);
        }

        public System.Threading.Tasks.Task<RemoveAliasResponse> RemoveAliasAsync(RemoveAliasRequest request)
        {
            return base.Channel.RemoveAliasAsync(request);
        }

        public ReverifyResponse Reverify(ReverifyRequest1 request)
        {
            return base.Channel.Reverify(request);
        }

        public System.Threading.Tasks.Task<ReverifyResponse> ReverifyAsync(ReverifyRequest1 request)
        {
            return base.Channel.ReverifyAsync(request);
        }

        public SearchByDemographicsResponse SearchByDemographics(SearchByDemographicsRequest request)
        {
            return base.Channel.SearchByDemographics(request);
        }

        public System.Threading.Tasks.Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request)
        {
            return base.Channel.SearchByDemographicsAsync(request);
        }

        public SelectResponse Select(SelectRequest1 request)
        {
            return base.Channel.Select(request);
        }

        public System.Threading.Tasks.Task<SelectResponse> SelectAsync(SelectRequest1 request)
        {
            return base.Channel.SelectAsync(request);
        }

        public UpdateResponse Update(UpdateRequest1 request)
        {
            return base.Channel.Update(request);
        }

        public System.Threading.Tasks.Task<UpdateResponse> UpdateAsync(UpdateRequest1 request)
        {
            return base.Channel.UpdateAsync(request);
        }

        public RetrieveAlertsResponse RetrieveAlerts(RetrieveAlertsRequest request)
        {
            return base.Channel.RetrieveAlerts(request);
        }

        public System.Threading.Tasks.Task<RetrieveAlertsResponse> RetrieveAlertsAsync(RetrieveAlertsRequest request)
        {
            return base.Channel.RetrieveAlertsAsync(request);
        }

        public AlternateClearResponse AlternateClear(AlternateClearRequest request)
        {
            return base.Channel.AlternateClear(request);
        }

        public System.Threading.Tasks.Task<AlternateClearResponse> AlternateClearAsync(AlternateClearRequest request)
        {
            return base.Channel.AlternateClearAsync(request);
        }
    }
}