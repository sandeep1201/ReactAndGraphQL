using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
//TODO: cleanup
namespace Dcf.Wwp.ConnectedServices.Cww
{
    #region Operations
    
    [ServiceContract(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", ConfigurationName = "CwwKeySecurityService")]
    public interface ICwwKeySecuritySvc
    {
        //[CwwFormatMessage]
        [XmlSerializerFormat]
        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetKeySecurityInfo", ReplyAction = "*")]
        GetKeySecurityInfoResponse GetKeySecurityInfo(GetKeySecurityInfoRequest request);

        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetKeySecurityInfo", ReplyAction = "*")]
        Task<GetKeySecurityInfoResponse> GetKeySecurityInfoAsync(GetKeySecurityInfoRequest request);
        
        //[CwwFormatMessage]
        [XmlSerializerFormat]
        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetFEPSupervisor", ReplyAction = "*")]
        GetFEPSupervisorResponse GetFEPSupervisor(GetFEPSupervisorRequest request);

        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetFEPSupervisor", ReplyAction = "*")]
        Task<GetFEPSupervisorResponse> GetFEPSupervisorAsync(GetFEPSupervisorRequest request);
    }

    #endregion

    #region Message Contracts

    [Serializable]
    [XmlType(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/")]
    public class ValidationErrorType
    {
        //private string fieldNameField;

        //private string errorCodeField;

        //private string errorMessageField;

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0)]
        public string FieldName { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 1)]
        public string ErrorCode { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 2)]
        public string ErrorMessage { get; set; }
    }

    [MessageContract(WrapperName = "GetKeySecurityInfoRequest", WrapperNamespace = "http://keysecurity.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public class GetKeySecurityInfoRequest
    {
        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PINNumber;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExternalAgencyId;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 2)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FEPID;

        public GetKeySecurityInfoRequest()
        {
        }

        public GetKeySecurityInfoRequest(string PINNumber, string ExternalAgencyId, string FEPID)
        {
            this.PINNumber        = PINNumber;
            this.ExternalAgencyId = ExternalAgencyId;
            this.FEPID            = FEPID;
        }
    }

    [MessageContract(WrapperName = "GetKeySecurityInfoResponse", WrapperNamespace = "http://keysecurity.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public class GetKeySecurityInfoResponse
    {
        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Status;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CaseCofidentailStatus;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 2)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FEPSupervisor;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 3)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("ValidationError", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public ValidationErrorType[] Errors;

        public GetKeySecurityInfoResponse()
        {
        }

        public GetKeySecurityInfoResponse(string Status, string CaseCofidentailStatus, string FEPSupervisor, ValidationErrorType[] Errors)
        {
            this.Status                = Status;
            this.CaseCofidentailStatus = CaseCofidentailStatus;
            this.FEPSupervisor         = FEPSupervisor;
            this.Errors                = Errors;
        }
    }

    [MessageContract(WrapperName = "GetFEPSupervisorRequest", WrapperNamespace = "http://keysecurity.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public class GetFEPSupervisorRequest
    {
        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExternalAgencyId;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FEPID;

        public GetFEPSupervisorRequest()
        {
        }

        public GetFEPSupervisorRequest(string ExternalAgencyId, string FEPID)
        {
            this.ExternalAgencyId = ExternalAgencyId;
            this.FEPID            = FEPID;
        }
    }

    [MessageContract(WrapperName = "GetFEPSupervisorResponse", WrapperNamespace = "http://keysecurity.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public class GetFEPSupervisorResponse
    {
        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Status;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FEPSupervisor;

        [MessageBodyMember(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", Order = 2)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("ValidationError", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public ValidationErrorType[] Errors;

        public GetFEPSupervisorResponse()
        {
        }

        public GetFEPSupervisorResponse(string Status, string FEPSupervisor, ValidationErrorType[] Errors)
        {
            this.Status        = Status;
            this.FEPSupervisor = FEPSupervisor;
            this.Errors        = Errors;
        }
    }

    #endregion

    #region Client and/or Channel Interface

    public interface ICWWKeySecurityServiceChannel : ICwwKeySecuritySvc, IClientChannel
    {
    }

    public  class CWWKeySecurityService : ClientBase<ICwwKeySecuritySvc>, ICwwKeySecuritySvc
    {
        public CWWKeySecurityService()
        {
        }

        public CWWKeySecurityService(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        public CWWKeySecurityService(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public CWWKeySecurityService(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public CWWKeySecurityService(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        public GetKeySecurityInfoResponse GetKeySecurityInfo(GetKeySecurityInfoRequest request)
        {
            return Channel.GetKeySecurityInfo(request);
        }

        public Task<GetKeySecurityInfoResponse> GetKeySecurityInfoAsync(GetKeySecurityInfoRequest request)
        {
            return Channel.GetKeySecurityInfoAsync(request);
        }

        public GetFEPSupervisorResponse GetFEPSupervisor(GetFEPSupervisorRequest request)
        {
            return Channel.GetFEPSupervisor(request);
        }

        public Task<GetFEPSupervisorResponse> GetFEPSupervisorAsync(GetFEPSupervisorRequest request)
        {
            return Channel.GetFEPSupervisorAsync(request);
        }
    }

    #endregion
}
