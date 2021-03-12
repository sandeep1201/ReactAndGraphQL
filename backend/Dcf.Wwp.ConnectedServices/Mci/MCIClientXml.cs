#region Credits

// WsConsoleApp - Dcf.Wwp.ConnectedServices - MCIClientXml.cs / 08/16/2018 / sbv00 / Scott V.

#endregion

using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dcf.Wwp.ConnectedServices.Mci.Xml
{
    #region Interfaces

    public interface IMCIClientXmlChannel : IMCIClientXml, IClientChannel { }

    [ServiceContract(Namespace = "http://MCIService.services.business.mci.wisconsin.gov", ConfigurationName = "Dcf.Wwp.ConnectedServices.Mci")]
    public interface IMCIClientXml
    {
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        AddAliasResponse AddAlias(AddAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        Task<AddAliasResponse> AddAliasAsync(AddAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        ClearResponse Clear(ClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        Task<ClearResponse> ClearAsync(ClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        DeleteResponse Delete(DeleteRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        Task<DeleteResponse> DeleteAsync(DeleteRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        EstablishResponse Establish(EstablishRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        Task<EstablishResponse> EstablishAsync(EstablishRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        RemoveAliasResponse RemoveAlias(RemoveAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        Task<RemoveAliasResponse> RemoveAliasAsync(RemoveAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        ReverifyResponse Reverify(ReverifyRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        Task<ReverifyResponse> ReverifyAsync(ReverifyRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        SearchByDemographicsResponse SearchByDemographics(SearchByDemographicsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        SelectResponse Select(SelectRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        Task<SelectResponse> SelectAsync(SelectRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        UpdateResponse Update(UpdateRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        Task<UpdateResponse> UpdateAsync(UpdateRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        RetrieveAlertsResponse RetrieveAlerts(RetrieveAlertsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        Task<RetrieveAlertsResponse> RetrieveAlertsAsync(RetrieveAlertsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        [XmlSerializerFormat]
        [ServiceKnownType(typeof(DemographicDetails))]
        AlternateClearResponse AlternateClear(AlternateClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        Task<AlternateClearResponse> AlternateClearAsync(AlternateClearRequest request);
    }

    #endregion

    #region Client code

    public partial class MCIClientXml : ClientBase<IMCIClientXml>, IMCIClientXml
    {
        //public MCIClientXml() {}

        public MCIClientXml()
        {
        }

        public MCIClientXml(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        public MCIClientXml(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public MCIClientXml(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public MCIClientXml(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        public AddAliasResponse AddAlias(AddAliasRequest request) => Channel.AddAlias(request);

        public Task<AddAliasResponse> AddAliasAsync(AddAliasRequest request) => Channel.AddAliasAsync(request);

        public AlternateClearResponse AlternateClear(AlternateClearRequest request) => Channel.AlternateClear(request);

        public Task<AlternateClearResponse> AlternateClearAsync(AlternateClearRequest request) => Channel.AlternateClearAsync(request);

        public ClearResponse Clear(ClearRequest request) => Channel.Clear(request);

        public Task<ClearResponse> ClearAsync(ClearRequest request) => Channel.ClearAsync(request);

        public DeleteResponse Delete(DeleteRequest1 request) => Channel.Delete(request);

        public Task<DeleteResponse> DeleteAsync(DeleteRequest1 request) => Channel.DeleteAsync(request);

        public EstablishResponse Establish(EstablishRequest1 request) => Channel.Establish(request);

        public Task<EstablishResponse> EstablishAsync(EstablishRequest1 request) => Channel.EstablishAsync(request);

        public RemoveAliasResponse RemoveAlias(RemoveAliasRequest request) => Channel.RemoveAlias(request);

        public Task<RemoveAliasResponse> RemoveAliasAsync(RemoveAliasRequest request) => Channel.RemoveAliasAsync(request);

        public RetrieveAlertsResponse RetrieveAlerts(RetrieveAlertsRequest request) => Channel.RetrieveAlerts(request);

        public Task<RetrieveAlertsResponse> RetrieveAlertsAsync(RetrieveAlertsRequest request) => Channel.RetrieveAlertsAsync(request);

        public ReverifyResponse Reverify(ReverifyRequest1 request) => Channel.Reverify(request);

        public Task<ReverifyResponse> ReverifyAsync(ReverifyRequest1 request) => Channel.ReverifyAsync(request);

        public SearchByDemographicsResponse SearchByDemographics(SearchByDemographicsRequest request) => Channel.SearchByDemographics(request);

        //public Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request) => Channel.SearchByDemographicsAsync(request);
        public async Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request) => await Channel.SearchByDemographicsAsync(request);

        public SelectResponse Select(SelectRequest1 request) => Channel.Select(request);

        public Task<SelectResponse> SelectAsync(SelectRequest1 request) => Channel.SelectAsync(request);

        public UpdateResponse Update(UpdateRequest1 request) => Channel.Update(request);

        public async Task<UpdateResponse> UpdateAsync(UpdateRequest1 request) => await Channel.UpdateAsync(request);
    }

    #endregion

    #region Message Contracts

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

    #endregion

    #region Data Types

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Alert : DemographicDetails
    {
        [XmlElement(Order = 0)]
        public int AlertSequenceNumber { get; set; }

        [XmlArray(Order = 1)]
        public decimal[] AliasSSN { get; set; }

        [XmlElement(Order = 2)]
        public string AlertCode { get; set; }

        [XmlElement(Order = 3)]
        public DateTime AlertDate { get; set; }

        [XmlElement(Order = 4)]
        public string AlertUserID { get; set; }

        [XmlElement(Order = 5)]
        public long CorrectedSSN { get; set; }

        [XmlElement(Order = 6)]
        public DateTime DOD { get; set; }

        [XmlElement(Order = 7)]
        public string MCISSNVerificationCode { get; set; }

        [XmlElement(Order = 8)]
        public string ErrorConditionCode { get; set; }

        [XmlElement(Order = 9)]
        public string Soundex { get; set; }

        [XmlElement(Order = 10)]
        public DateTime SSNMatchDate { get; set; }

        [XmlElement(Order = 11)]
        public long MCIID { get; set; }

        [XmlElement(Order = 12)]
        public string ReasonForRequest { get; set; }

        [XmlElement(Order = 13)]
        public bool Title16 { get; set; }

        [XmlElement(Order = 14)]
        public bool Title2 { get; set; }

        [XmlElement(Order = 15)]
        public string DiscrepancyCode { get; set; }
    }

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

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AliasResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearanceRequest : DemographicDetails
    {
        [XmlElement(Order = 0)]
        public long MCIID { get; set; }

        [XmlElement(Order = 1)]
        public string ReferralApplication { get; set; }

        [XmlElement(Order = 2)]
        public string UserName { get; set; }

        [XmlElement(Order = 3)]
        public string ReasonForRequest { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearanceResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceMatch : DemographicMatch
    {
    }

    [Serializable]
    [XmlInclude(typeof(SelectRequest))]
    [XmlInclude(typeof(EstablishRequest))]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceRequest : DemographicDetails
    {
        [XmlElement(Order = 0)]
        public short MinimumMatchScore { get; set; }

        [XmlElement(Order = 1)]
        public string AliasSSN { get; set; }

        [XmlArray(Order = 2)]
        public Name[] AliasName { get; set; }

        [XmlElement(Order = 3)]
        public string UserName { get; set; }

        [XmlElement(Order = 4)]
        public string ReasonForRequest { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceResult
    {
        [XmlArray(Order = 0)]
        public ClearanceMatch[] Match { get; set; }

        [XmlElement(Order = 1)]
        public string RequestID { get; set; }

        [XmlElement(Order = 2)]
        public long MCIID { get; set; }

        [XmlArray(Order = 3)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteRequest
    {
        [XmlElement(Order = 0)]
        public long MCIID { get; set; }

        [XmlElement(Order = 1)]
        public string UserName { get; set; }

        [XmlElement(Order = 2)]
        public string ReasonForRequest { get; set; }
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
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicDetails
    {
        [XmlElement(Order = 0)]
        public Name PrimaryName { get; set; }

        [XmlElement(Order = 1)]
        public long SSN { get; set; }

        [XmlElement(Order = 2)]
        public DateTime DOB { get; set; }

        [XmlElement(Order = 3)]
        public string Gender { get; set; }

        [XmlElement(Order = 4)]
        public string SSNVerificationCode { get; set; }
    }

    [Serializable]
    [XmlInclude(typeof(DemographicSearchMatch))]
    [XmlInclude(typeof(ClearanceMatch))]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicMatch : Match
    {
        [XmlElement(Order = 0)]
        public string MatchType { get; set; }

        [XmlElement(Order = 1)]
        public short MatchScore { get; set; }

        [XmlElement(Order = 2)]
        public Name AliasName { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchMatch : DemographicMatch
    {
        [XmlElement(Order = 0)]
        public bool OtherNamesExist { get; set; }

        [XmlElement(Order = 1)]
        public bool AliasSSNsExist { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchRequest
    {
        [XmlElement(Order = 0)]
        public Name PrimaryName { get; set; }

        [XmlElement(Order = 1)]
        public long SSN { get; set; }

        [XmlElement(Order = 2)]
        public DateTime DOB { get; set; }

        [XmlElement(Order = 3)]
        public string Gender { get; set; }

        [XmlElement(Order = 4)]
        public short MinimumMatchScore { get; set; }

        [XmlElement(Order = 5)]
        public bool IncludeAliasSSN { get; set; }

        [XmlArray(Order = 6)]
        public Name[] AliasName { get; set; }

        [XmlElement(Order = 7)]
        public string UserName { get; set; }

        [XmlElement(Order = 8)]
        public string ReasonForRequest { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }

        [XmlArray(Order = 1)]
        public DemographicSearchMatch[] Match { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class EstablishRequest : ClearanceRequest
    {
        [XmlElement(Order = 0)]
        public string RequestID { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class EstablishResult
    {
        [XmlElement(Order = 0)]
        public long MCIID { get; set; }

        [XmlArray(Order = 1)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlInclude(typeof(DemographicMatch))]
    [XmlInclude(typeof(DemographicSearchMatch))]
    [XmlInclude(typeof(ClearanceMatch))]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Match : DemographicDetails
    {
        [XmlElement(Order = 0)]
        public short SequenceNumber { get; set; }

        [XmlElement(Order = 1)]
        public long MCIID { get; set; }

        [XmlElement(Order = 2)]
        public string SSNVerificationCodeDescription { get; set; }

        [XmlElement(Order = 3)]
        public bool IsMCIIDKnown { get; set; }

        [XmlElement(Order = 4)]
        public bool IsSequenceNumberKnown { get; set; }

        [XmlElement(Order = 5)]
        public string RecordType { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Name
    {
        [XmlElement(Order = 0)]
        public string FirstName { get; set; }

        [XmlElement(Order = 1)]
        public string LastName { get; set; }

        [XmlElement(Order = 2)]
        public string MiddleInitial { get; set; }

        [XmlElement(Order = 3)]
        public string SuffixName { get; set; }

        [XmlElement(Order = 4)]
        public NameType NameType { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertRequest
    {
        [XmlElement(Order = 0)]
        public short AlertSequenceNumber { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }

        [XmlArray(Order = 1)]
        public Alert[] Alerts { get; set; }

        [XmlElement(Order = 2)]
        public bool MoreAlertsExist { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyRequest : DemographicDetails
    {
        [XmlElement(Order = 0)]
        public long MCIID { get; set; }

        [XmlElement(Order = 1)]
        public string UserName { get; set; }

        [XmlElement(Order = 2)]
        public string ReasonForRequest { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectRequest : ClearanceRequest
    {
        [XmlElement(Order = 0)]
        public long MCIID { get; set; }

        [XmlElement(Order = 1)]
        public short SequenceNumber { get; set; }

        [XmlElement(Order = 2)]
        public string RequestID { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateRequest : DemographicDetails
    {
        [XmlElement(Order = 0)]
        public long MCIID { get; set; }

        [XmlElement(Order = 1)]
        public OldNameType StorePreviousName { get; set; }

        [XmlElement(Order = 2)]
        public OldSSNType StorePreviousSSN { get; set; }

        [XmlElement(Order = 3)]
        public string UserName { get; set; }

        [XmlElement(Order = 4)]
        public string ReasonForRequest { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateResult
    {
        [XmlArray(Order = 0)]
        public ValidationError[] Errors { get; set; }
    }

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ValidationError
    {
        [XmlElement(Order = 0)]
        public string FieldName { get; set; }

        [XmlElement(Order = 1)]
        public string ErrorCode { get; set; }

        [XmlElement(Order = 2)]
        public string ErrorMessage { get; set; }
    }

    #endregion

    #region Enums

    [Serializable]
    [XmlType(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum NameType
    {
        PrimaryName,
        Alias,
        MaidenName
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

    #endregion
}
