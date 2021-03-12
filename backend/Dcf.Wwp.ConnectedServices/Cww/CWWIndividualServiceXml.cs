using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;
//TODO: cleanup
namespace Dcf.Wwp.ConnectedServices.Cww
{
    #region Interfaces

    public interface ICwwIndividualServiceChannel : ICwwIndividualSvc, IClientChannel
    {
    }

    [ServiceContract(Namespace = "http://individual.services.business.cares.wisconsin.gov/", ConfigurationName = "Dcf.Wwp.ConnectedServices.Cww.Xml.CWWIndividualService")]
    public interface ICwwIndividualSvc
    {
        [XmlSerializerFormat]
        [OperationContract(Action = "", ReplyAction = "*")]
        GetIndvDemographicsResponse GetIndvDemographics(GetIndvDemographicsRequest request);

        [OperationContract(Action = "", ReplyAction = "*")]
        Task<GetIndvDemographicsResponse> GetIndvDemographicsAsync(GetIndvDemographicsRequest request);

        [XmlSerializerFormat]
        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/InsertIndvKeyDemographics", ReplyAction = "*")]
        InsertIndvKeyDemographicsResponse InsertIndvKeyDemographics(InsertIndvKeyDemographicsRequest request);

        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/InsertIndvKeyDemographics", ReplyAction = "*")]
        Task<InsertIndvKeyDemographicsResponse> InsertIndvKeyDemographicsAsync(InsertIndvKeyDemographicsRequest request);

        [XmlSerializerFormat]
        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/UpdateIndvKeyDemographics", ReplyAction = "*")]
        UpdateIndvKeyDemographicsResponse UpdateIndvKeyDemographics(UpdateIndvKeyDemographicsRequest request);

        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/UpdateIndvKeyDemographics", ReplyAction = "*")]
        Task<UpdateIndvKeyDemographicsResponse> UpdateIndvKeyDemographicsAsync(UpdateIndvKeyDemographicsRequest request);

        [XmlSerializerFormat]
        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/UpdateRaceEthnicityInformation", ReplyAction = "*")]
        UpdateRaceEthnicityInformationResponse UpdateRaceEthnicityInformation(UpdateRaceEthnicityInformationRequest request);

        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/UpdateRaceEthnicityInformation", ReplyAction = "*")]
        Task<UpdateRaceEthnicityInformationResponse> UpdateRaceEthnicityInformationAsync(UpdateRaceEthnicityInformationRequest request);

        [XmlSerializerFormat]
        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/UpdateFEPInformation", ReplyAction = "*")]
        UpdateFEPInformationResponse UpdateFEPInformation(UpdateFEPInformationRequest request);

        [OperationContract(Action = "http://individual.services.business.cares.wisconsin.gov/UpdateFEPInformation", ReplyAction = "*")]
        Task<UpdateFEPInformationResponse> UpdateFEPInformationAsync(UpdateFEPInformationRequest request);
    }

    #endregion

    #region Client

    public partial class CwwIndividualService : ClientBase<ICwwIndividualSvc>, ICwwIndividualSvc
    {
        #region Ctors

        public CwwIndividualService()
        {
        }

        public CwwIndividualService(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        public CwwIndividualService(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public CwwIndividualService(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public CwwIndividualService(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        #endregion

        #region Operations

        public GetIndvDemographicsResponse GetIndvDemographics(GetIndvDemographicsRequest request)
        {
            return Channel.GetIndvDemographics(request);
        }

        public Task<GetIndvDemographicsResponse> GetIndvDemographicsAsync(GetIndvDemographicsRequest request)
        {
            return Channel.GetIndvDemographicsAsync(request);
        }

        public InsertIndvKeyDemographicsResponse InsertIndvKeyDemographics(InsertIndvKeyDemographicsRequest request)
        {
            return Channel.InsertIndvKeyDemographics(request);
        }

        public Task<InsertIndvKeyDemographicsResponse> InsertIndvKeyDemographicsAsync(InsertIndvKeyDemographicsRequest request)
        {
            return Channel.InsertIndvKeyDemographicsAsync(request);
        }

        public UpdateIndvKeyDemographicsResponse UpdateIndvKeyDemographics(UpdateIndvKeyDemographicsRequest request)
        {
            return Channel.UpdateIndvKeyDemographics(request);
        }

        public Task<UpdateIndvKeyDemographicsResponse> UpdateIndvKeyDemographicsAsync(UpdateIndvKeyDemographicsRequest request)
        {
            return Channel.UpdateIndvKeyDemographicsAsync(request);
        }

        public UpdateRaceEthnicityInformationResponse UpdateRaceEthnicityInformation(UpdateRaceEthnicityInformationRequest request)
        {
            return Channel.UpdateRaceEthnicityInformation(request);
        }

        public Task<UpdateRaceEthnicityInformationResponse> UpdateRaceEthnicityInformationAsync(UpdateRaceEthnicityInformationRequest request)
        {
            return Channel.UpdateRaceEthnicityInformationAsync(request);
        }

        public UpdateFEPInformationResponse UpdateFEPInformation(UpdateFEPInformationRequest request)
        {
            return Channel.UpdateFEPInformation(request);
        }

        public Task<UpdateFEPInformationResponse> UpdateFEPInformationAsync(UpdateFEPInformationRequest request)
        {
            return Channel.UpdateFEPInformationAsync(request);
        }

        #endregion
    }

    #endregion

    #region Message Contracts

    [MessageContract(WrapperName = "GetIndvDemoRequest", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class GetIndvDemographicsRequest
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PINNumber;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExternalAgencyId;

        public GetIndvDemographicsRequest()
        {
        }

        public GetIndvDemographicsRequest(string pinNumber, string externalAgencyId)
        {
            PINNumber        = pinNumber;
            ExternalAgencyId = externalAgencyId;
        }
    }

    [MessageContract(WrapperName = "GetIndvDemoResponse", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class GetIndvDemographicsResponse
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 2)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("ValidationError", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public ValidationErrorType[] Errors;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Status;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public IndividualType Individual;

        public GetIndvDemographicsResponse()
        {
        }

        public GetIndvDemographicsResponse(string Status, IndividualType Individual, ValidationErrorType[] Errors)
        {
            this.Status     = Status;
            this.Individual = Individual;
            this.Errors     = Errors;
        }
    }

    [MessageContract(WrapperName = "InsertIndvDemoRequest", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class InsertIndvKeyDemographicsRequest
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public IndividualType Individual;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WorkerId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 2)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExternalAgencyId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 3)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string IPAddress;

        public InsertIndvKeyDemographicsRequest()
        {
        }

        public InsertIndvKeyDemographicsRequest(IndividualType Individual, string WorkerId, string ExternalAgencyId, string IPAddress)
        {
            this.Individual       = Individual;
            this.WorkerId         = WorkerId;
            this.ExternalAgencyId = ExternalAgencyId;
            this.IPAddress        = IPAddress;
        }
    }

    [MessageContract(WrapperName = "InsertIndvDemoResponse", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class InsertIndvKeyDemographicsResponse
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 2)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("ValidationError", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public ValidationErrorType[] Errors;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Status;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PINNumber;

        public InsertIndvKeyDemographicsResponse()
        {
        }

        public InsertIndvKeyDemographicsResponse(string Status, string PINNumber, ValidationErrorType[] Errors)
        {
            this.Status    = Status;
            this.PINNumber = PINNumber;
            this.Errors    = Errors;
        }
    }

    [MessageContract(WrapperName = "UpdateIndvDemoRequest", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class UpdateIndvKeyDemographicsRequest
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public IndividualType Individual;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WorkerId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 2)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExternalAgencyId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 3)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string IPAddress;

        public UpdateIndvKeyDemographicsRequest()
        {
        }

        public UpdateIndvKeyDemographicsRequest(IndividualType Individual, string WorkerId, string ExternalAgencyId, string IPAddress)
        {
            this.Individual       = Individual;
            this.WorkerId         = WorkerId;
            this.ExternalAgencyId = ExternalAgencyId;
            this.IPAddress        = IPAddress;
        }
    }

    [MessageContract(WrapperName = "UpdateIndvDemoResponse", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class UpdateIndvKeyDemographicsResponse
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("ValidationError", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public ValidationErrorType[] Errors;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Status;

        public UpdateIndvKeyDemographicsResponse()
        {
        }

        public UpdateIndvKeyDemographicsResponse(string Status, ValidationErrorType[] Errors)
        {
            this.Status = Status;
            this.Errors = Errors;
        }
    }

    [MessageContract(WrapperName = "UpdateRaceEthnicityRequest", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class UpdateRaceEthnicityInformationRequest
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 5)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string IPAddress;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 6)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public RaceType Race;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 7)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public EthnicityType Ethnicity;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 2)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string HistorySeqNum;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 3)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WorkerId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 4)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExternalAgencyId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PINNumber;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string MCIId;

        public UpdateRaceEthnicityInformationRequest()
        {
        }

        public UpdateRaceEthnicityInformationRequest(string PINNumber, string MCIId, string HistorySeqNum, string WorkerId, string ExternalAgencyId, string IPAddress, RaceType Race, EthnicityType Ethnicity)
        {
            this.PINNumber        = PINNumber;
            this.MCIId            = MCIId;
            this.HistorySeqNum    = HistorySeqNum;
            this.WorkerId         = WorkerId;
            this.ExternalAgencyId = ExternalAgencyId;
            this.IPAddress        = IPAddress;
            this.Race             = Race;
            this.Ethnicity        = Ethnicity;
        }
    }

    [MessageContract(WrapperName = "UpdateRaceEthnicityResponse", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class UpdateRaceEthnicityInformationResponse
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("ValidationError", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public ValidationErrorType[] Errors;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Status;

        public UpdateRaceEthnicityInformationResponse()
        {
        }

        public UpdateRaceEthnicityInformationResponse(string Status, ValidationErrorType[] Errors)
        {
            this.Status = Status;
            this.Errors = Errors;
        }
    }

    [MessageContract(WrapperName = "UpdateFEPInfoRequest", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class UpdateFEPInformationRequest
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PINNumber;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PreviousFEPId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 2)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CurrentFEPId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 3)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string WorkerId;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 4)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExternalAgencyId;

        public UpdateFEPInformationRequest()
        {
        }

        public UpdateFEPInformationRequest(string PINNumber, string PreviousFEPId, string CurrentFEPId, string WorkerId, string ExternalAgencyId)
        {
            this.PINNumber        = PINNumber;
            this.PreviousFEPId    = PreviousFEPId;
            this.CurrentFEPId     = CurrentFEPId;
            this.WorkerId         = WorkerId;
            this.ExternalAgencyId = ExternalAgencyId;
        }
    }

    [MessageContract(WrapperName = "UpdateFEPInfoResponse", WrapperNamespace = "http://individual.services.business.cares.wisconsin.gov/", IsWrapped = true)]
    public partial class UpdateFEPInformationResponse
    {
        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 2)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("ValidationError", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public ValidationErrorType[] Errors;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 0)] [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Status;

        [MessageBodyMember(Namespace = "http://individual.services.business.cares.wisconsin.gov/", Order = 1)] [XmlArray(Form = XmlSchemaForm.Unqualified)] [XmlArrayItem("PINNumber", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] UpdatedPINs;

        public UpdateFEPInformationResponse()
        {
        }

        public UpdateFEPInformationResponse(string Status, string[] UpdatedPINs, ValidationErrorType[] Errors)
        {
            this.Status      = Status;
            this.UpdatedPINs = UpdatedPINs;
            this.Errors      = Errors;
        }
    }

    #endregion

    #region Data Types

    [Serializable]
    [XmlType(Namespace = "http://individual.services.business.cares.wisconsin.gov/")]
    public partial class IndividualType
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0)]
        public string FirstName { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 1)]

        public string MiddleInitial { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 2)]
        public string LastName { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 3)]
        public string Suffix { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 4)]
        public string Gender { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 5)]
        public string SSNNumber { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 6)]
        public string SSNVerificationCode { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date", Order = 7)]
        public DateTime DOB { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 8)]
        public string MCIId { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 9)]
        public string PINNumber { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 10)]
        public string HistorySeqNum { get; set; }

        [XmlArray(Form                  = XmlSchemaForm.Unqualified, Order      = 11)]
        [XmlArrayItem("AliasName", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public AliasNameType[] AliasNames { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 12)]
        public RaceType Race { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 13)]
        public EthnicityType Ethnicity { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://individual.services.business.cares.wisconsin.gov/")]
    public partial class AliasNameType
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0)]
        public string AliasFirstName { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 1)]
        public string AliasMiddleInitial { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 2)]
        public string AliasLastName { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 3)]
        public string AliasSuffix { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 4)]
        public string AliasType { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://individual.services.business.cares.wisconsin.gov/")]
    public partial class EthnicityType
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0)]
        public string Hispanic { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://individual.services.business.cares.wisconsin.gov/")]
    public partial class RaceType
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 0)]
        public string AmericanIndianOrAlaskan { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 1)]
        public string Asian { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 2)]
        public string BlackOrAfricanAmerican { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 3)]
        public string HawaiianOrPacificIslander { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, Order = 4)]
        public string White { get; set; }
    }

    #endregion
}
