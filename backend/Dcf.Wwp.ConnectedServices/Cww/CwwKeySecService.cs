using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Dcf.Wwp.ConnectedServices.Cww
{
    #region Interface / Operation Contracts

    public interface ICwwKeySecServiceChannel : ICwwKeySecService, IClientChannel
    {
    }

    [ServiceContract(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/", ConfigurationName = "CWWKeySecurityService")]
    public interface ICwwKeySecService
    {
        [XmlSerializerFormat]
        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetKeySecurityInfo", ReplyAction = "*")]
        GetKeySecurityInfoResponse GetKeySecurityInfo(GetKeySecurityInfoRequest request);

        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetKeySecurityInfo", ReplyAction = "*")]
        Task<GetKeySecurityInfoResponse> GetKeySecurityInfoAsync(GetKeySecurityInfoRequest request);

        [XmlSerializerFormat]
        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetFEPSupervisor", ReplyAction = "*")]
        GetFEPSupervisorResponse GetFEPSupervisor(GetFEPSupervisorRequest request);

        [OperationContract(Action = "http://keysecurity.services.business.cares.wisconsin.gov/GetFEPSupervisor", ReplyAction = "*")]
        Task<GetFEPSupervisorResponse> GetFEPSupervisorAsync(GetFEPSupervisorRequest request);
    }

    #endregion

    #region Client

    public  class CWWKeySecServiceClient : ClientBase<ICwwKeySecService>, ICwwKeySecService
    {
        public CWWKeySecServiceClient()
        {
        }

        public CWWKeySecServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        public CWWKeySecServiceClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public CWWKeySecServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public CWWKeySecServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
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

    #region Message Contracts

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

    [Serializable]
    [XmlType(Namespace = "http://keysecurity.services.business.cares.wisconsin.gov/")]
    public class ValidationErrorType
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0)]
        public string FieldName { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 1)]
        public string ErrorCode { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 2)]
        public string ErrorMessage { get; set; }
    }

    #endregion
}
