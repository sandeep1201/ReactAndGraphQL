using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Dcf.Wwp.ConnectedServices.Mci.Dcs
{
    #region Interfaces

    public interface MCIClientDcsChannel : IMCIClientDcs, IClientChannel { }

    [ServiceContract(Namespace = "http://MCIService.services.business.mci.wisconsin.gov", ConfigurationName = "Dcf.Wwp.ConnectedServices.Mci.Dcs.MCIServiceSoap")]
    public interface IMCIClientDcs
    {
        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        AddAliasResponse AddAlias(AddAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AddAlias", ReplyAction = "*")]
        Task<AddAliasResponse> AddAliasAsync(AddAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        AlternateClearResponse AlternateClear(AlternateClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/AlternateClear", ReplyAction = "*")]
        Task<AlternateClearResponse> AlternateClearAsync(AlternateClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        ClearResponse Clear(ClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Clear", ReplyAction = "*")]
        Task<ClearResponse> ClearAsync(ClearRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        DeleteResponse Delete(DeleteRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Delete", ReplyAction = "*")]
        Task<DeleteResponse> DeleteAsync(DeleteRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        EstablishResponse Establish(EstablishRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Establish", ReplyAction = "*")]
        Task<EstablishResponse> EstablishAsync(EstablishRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        RemoveAliasResponse RemoveAlias(RemoveAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RemoveAlias", ReplyAction = "*")]
        Task<RemoveAliasResponse> RemoveAliasAsync(RemoveAliasRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        RetrieveAlertsResponse RetrieveAlerts(RetrieveAlertsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/RetrieveAlerts", ReplyAction = "*")]
        Task<RetrieveAlertsResponse> RetrieveAlertsAsync(RetrieveAlertsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        ReverifyResponse Reverify(ReverifyRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Reverify", ReplyAction = "*")]
        Task<ReverifyResponse> ReverifyAsync(ReverifyRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        SearchByDemographicsResponse SearchByDemographics(SearchByDemographicsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/SearchByDemographics", ReplyAction = "*")]
        Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        SelectResponse Select(SelectRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Select", ReplyAction = "*")]
        Task<SelectResponse> SelectAsync(SelectRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        UpdateResponse Update(UpdateRequest1 request);

        [OperationContract(Action = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov/Update", ReplyAction = "*")]
        Task<UpdateResponse> UpdateAsync(UpdateRequest1 request);
    }

    #endregion

    #region Client

    [DebuggerStepThrough]
    public class MCIClientDcs : ClientBase<IMCIClientDcs>, IMCIClientDcs
    {
        #region Ctors

        public MCIClientDcs() {}

        public MCIClientDcs(string endpointConfigurationName) : base(endpointConfigurationName) {}

        public MCIClientDcs(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress) {}

        public MCIClientDcs(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress) {}

        public MCIClientDcs(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) {}

        #endregion

        #region Operations
        
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

        public Task<SearchByDemographicsResponse> SearchByDemographicsAsync(SearchByDemographicsRequest request) => Channel.SearchByDemographicsAsync(request);

        public SelectResponse Select(SelectRequest1 request) => Channel.Select(request);

        public Task<SelectResponse> SelectAsync(SelectRequest1 request) => Channel.SelectAsync(request);

        public UpdateResponse Update(UpdateRequest1 request) => Channel.Update(request);

        public Task<UpdateResponse> UpdateAsync(UpdateRequest1 request) => Channel.UpdateAsync(request);

        #endregion
    }

    #endregion

    #region Message Contracts

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class AddAliasRequest
    {
        [MessageBodyMember(Name = "AddAlias", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AddAliasRequestBody Body;

        public AddAliasRequest() {}

        public AddAliasRequest(AddAliasRequestBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class AddAliasResponse
    {
        [MessageBodyMember(Name = "AddAliasResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AddAliasResponseBody Body;

        public AddAliasResponse() {}

        public AddAliasResponse(AddAliasResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class AlternateClearRequest
    {
        [MessageBodyMember(Name = "AlternateClear", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AlternateClearRequestBody Body;

        public AlternateClearRequest() {}

        public AlternateClearRequest(AlternateClearRequestBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class AlternateClearResponse
    {
        [MessageBodyMember(Name = "AlternateClearResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public AlternateClearResponseBody Body;

        public AlternateClearResponse() {}

        public AlternateClearResponse(AlternateClearResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class ClearRequest
    {
        [MessageBodyMember(Name = "Clear", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ClearRequestBody Body;

        public ClearRequest() {}

        public ClearRequest(ClearRequestBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class ClearResponse
    {
        [MessageBodyMember(Name = "ClearResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ClearResponseBody Body;

        public ClearResponse() {}

        public ClearResponse(ClearResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class DeleteRequest1
    {
        [MessageBodyMember(Name = "Delete", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DeleteRequest1Body Body;

        public DeleteRequest1() {}

        public DeleteRequest1(DeleteRequest1Body Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class DeleteResponse
    {
        [MessageBodyMember(Name = "DeleteResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public DeleteResponseBody Body;

        public DeleteResponse() {}

        public DeleteResponse(DeleteResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class EstablishRequest1
    {
        [MessageBodyMember(Name = "Establish", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public EstablishRequest1Body Body;

        public EstablishRequest1() {}

        public EstablishRequest1(EstablishRequest1Body Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class EstablishResponse
    {
        [MessageBodyMember(Name = "EstablishResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public EstablishResponseBody Body;

        public EstablishResponse() {}

        public EstablishResponse(EstablishResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class RemoveAliasRequest
    {
        [MessageBodyMember(Name = "RemoveAlias", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RemoveAliasRequestBody Body;

        public RemoveAliasRequest() {}

        public RemoveAliasRequest(RemoveAliasRequestBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class RemoveAliasResponse
    {
        [MessageBodyMember(Name = "RemoveAliasResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RemoveAliasResponseBody Body;

        public RemoveAliasResponse() {}

        public RemoveAliasResponse(RemoveAliasResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class ReverifyRequest1
    {
        [MessageBodyMember(Name = "Reverify", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ReverifyRequest1Body Body;

        public ReverifyRequest1() {}

        public ReverifyRequest1(ReverifyRequest1Body Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class ReverifyResponse
    {
        [MessageBodyMember(Name = "ReverifyResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public ReverifyResponseBody Body;

        public ReverifyResponse() {}

        public ReverifyResponse(ReverifyResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class SearchByDemographicsRequest
    {
        [MessageBodyMember(Name = "SearchByDemographics", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SearchByDemographicsRequestBody Body;

        public SearchByDemographicsRequest() {}

        public SearchByDemographicsRequest(SearchByDemographicsRequestBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class SearchByDemographicsResponse
    {
        [MessageBodyMember(Name = "SearchByDemographicsResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SearchByDemographicsResponseBody Body;

        public SearchByDemographicsResponse() { }

        public SearchByDemographicsResponse(SearchByDemographicsResponseBody Body) => this.Body = Body;
    }

    //[DebuggerStepThrough]
    //[MessageContract(IsWrapped = false)]
    //public class SearchByDemographicsResponse
    //{
    //    [MessageBodyMember(Name = "SearchByDemographicsResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
    //    public DemographicSearchResult SearchByDemographicsResult;

    //    public SearchByDemographicsResponseBody Body;

    //    //public SearchByDemographicsResponse() { }

    //    public SearchByDemographicsResponse(SearchByDemographicsResponseBody Body)
    //    {
    //        this.Body = Body;
    //        this.SearchByDemographicsResult = Body.SearchByDemographicsResult;
    //    }
    //}

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class SelectRequest1
    {
        [MessageBodyMember(Name = "Select", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SelectRequest1Body Body;

        public SelectRequest1() {}

        public SelectRequest1(SelectRequest1Body Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class SelectResponse
    {
        [MessageBodyMember(Name = "SelectResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public SelectResponseBody Body;

        public SelectResponse() {}

        public SelectResponse(SelectResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class UpdateRequest1
    {
        [MessageBodyMember(Name = "Update", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public UpdateRequest1Body Body;

        public UpdateRequest1() {}

        public UpdateRequest1(UpdateRequest1Body Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class UpdateResponse
    {
        [MessageBodyMember(Name = "UpdateResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public UpdateResponseBody Body;

        public UpdateResponse() {}

        public UpdateResponse(UpdateResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public UpdateResult UpdateResult;

        public UpdateResponseBody() {}

        public UpdateResponseBody(UpdateResult UpdateResult) => this.UpdateResult = UpdateResult;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class RetrieveAlertsRequest
    {
        [MessageBodyMember(Name = "RetrieveAlerts", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RetrieveAlertsRequestBody Body;

        public RetrieveAlertsRequest() {}

        public RetrieveAlertsRequest(RetrieveAlertsRequestBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertsRequestBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public RetrieveAlertRequest RequestDetails;

        public RetrieveAlertsRequestBody() {}

        public RetrieveAlertsRequestBody(RetrieveAlertRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [MessageContract(IsWrapped = false)]
    public class RetrieveAlertsResponse
    {
        [MessageBodyMember(Name = "RetrieveAlertsResponse", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", Order = 0)]
        public RetrieveAlertsResponseBody Body;

        public RetrieveAlertsResponse() {}

        public RetrieveAlertsResponse(RetrieveAlertsResponseBody Body) => this.Body = Body;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertsResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public RetrieveAlertResult RetrieveAlertsResult;

        public RetrieveAlertsResponseBody() {}

        public RetrieveAlertsResponseBody(RetrieveAlertResult RetrieveAlertsResult) => this.RetrieveAlertsResult = RetrieveAlertsResult;
    }

    #endregion

    #region Data Contracts

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "AliasRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AliasRequest : object, IExtensibleDataObject
    {
        [DataMember(IsRequired = true)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public ArrayOfDecimal AliasSSN { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public Name[] AliasName { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public long SSN { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 5)]
        public string ReasonForRequest { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [CollectionDataContract(Name = "ArrayOfDecimal", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov", ItemName = "decimal")]
    public class ArrayOfDecimal : List<decimal> {}

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "Name", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Name : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public string FirstName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string LastName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string MiddleInitial { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string SuffixName { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public NameType NameType { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "AliasResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AliasResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "ValidationError", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ValidationError : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public string FieldName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 1)]
        public string ErrorCode { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string ErrorMessage { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [KnownType(typeof(Match))]
    [KnownType(typeof(DemographicMatch))]
    [KnownType(typeof(ClearanceMatch))]
    [KnownType(typeof(DemographicSearchMatch))]
    [KnownType(typeof(ReverifyRequest))]
    [KnownType(typeof(UpdateRequest))]
    [KnownType(typeof(Alert))]
    [KnownType(typeof(AlternateClearanceRequest))]
    [KnownType(typeof(ClearanceRequest))]
    [KnownType(typeof(EstablishRequest))]
    [KnownType(typeof(SelectRequest))]
    [Serializable]
    [DataContract(Name = "DemographicDetails", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicDetails : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public Name PrimaryName { get; set; }

        [DataMember(IsRequired = true)]
        public long SSN { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public DateTime DOB { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public string Gender { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 4)]
        public string SSNVerificationCode { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [KnownType(typeof(DemographicMatch))]
    [KnownType(typeof(ClearanceMatch))]
    [KnownType(typeof(DemographicSearchMatch))]
    [Serializable]
    [DataContract(Name = "Match", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Match : DemographicDetails
    {
        [DataMember(IsRequired = true)]
        public short SequenceNumber { get; set; }

        [DataMember(IsRequired = true, Order = 1)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string SSNVerificationCodeDescription { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public bool IsMCIIDKnown { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public bool IsSequenceNumberKnown { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 5)]
        public string RecordType { get; set; }
    }

    [DebuggerStepThrough]
    [KnownType(typeof(ClearanceMatch))]
    [KnownType(typeof(DemographicSearchMatch))]
    [Serializable]
    [DataContract(Name = "DemographicMatch", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicMatch : Match
    {
        [DataMember(EmitDefaultValue = false)]
        public string MatchType { get; set; }

        [DataMember(IsRequired = true, Order = 1)]
        public short MatchScore { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public Name AliasName { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "ClearanceMatch", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceMatch : DemographicMatch {}

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "DemographicSearchMatch", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchMatch : DemographicMatch
    {
        [DataMember(IsRequired = true)]
        public bool OtherNamesExist { get; set; }

        [DataMember(IsRequired = true, Order = 1)]
        public bool AliasSSNsExist { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "ReverifyRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyRequest : DemographicDetails
    {
        [DataMember(IsRequired = true)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string ReasonForRequest { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "UpdateRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateRequest : DemographicDetails
    {
        [DataMember(IsRequired = true)]
        public long MCIID { get; set; }

        [DataMember(IsRequired = true)]
        public OldNameType StorePreviousName { get; set; }

        [DataMember(IsRequired = true)]
        public OldSSNType StorePreviousSSN { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 4)]
        public string ReasonForRequest { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "Alert", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class Alert : DemographicDetails
    {
        [DataMember(IsRequired = true)]
        public int AlertSequenceNumber { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public ArrayOfDecimal AliasSSN { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string AlertCode { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public DateTime AlertDate { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 4)]
        public string AlertUserID { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public long CorrectedSSN { get; set; }

        [DataMember(IsRequired = true, Order = 6)]
        public DateTime DOD { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 7)]
        public string MCISSNVerificationCode { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 8)]
        public string ErrorConditionCode { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 9)]
        public string Soundex { get; set; }

        [DataMember(IsRequired = true, Order = 10)]
        public DateTime SSNMatchDate { get; set; }

        [DataMember(IsRequired = true, Order = 11)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 12)]
        public string ReasonForRequest { get; set; }

        [DataMember(IsRequired = true, Order = 13)]
        public bool Title16 { get; set; }

        [DataMember(IsRequired = true, Order = 14)]
        public bool Title2 { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 15)]
        public string DiscrepancyCode { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "AlternateClearanceRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearanceRequest : DemographicDetails
    {
        [DataMember(IsRequired = true)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ReferralApplication { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public string ReasonForRequest { get; set; }
    }

    [DebuggerStepThrough]
    [KnownType(typeof(EstablishRequest))]
    [KnownType(typeof(SelectRequest))]
    [Serializable]
    [DataContract(Name = "ClearanceRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearanceRequest : DemographicDetails
    {
        [DataMember(IsRequired = true)]
        public short MinimumMatchScore { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 1)]
        public string AliasSSN { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public Name[] AliasName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 4)]
        public string ReasonForRequest { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "EstablishRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class EstablishRequest : ClearanceRequest
    {
        [DataMember(EmitDefaultValue = false)]
        public string RequestID { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "SelectRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectRequest : ClearanceRequest
    {
        [DataMember(IsRequired = true)]
        public long MCIID { get; set; }

        [DataMember(IsRequired = true)]
        public short SequenceNumber { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string RequestID { get; set; }
    }

    [DebuggerStepThrough]
    [DataContract(Name = "ClearanceResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    [Serializable]
    public class ClearanceResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ClearanceMatch[] Match { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string RequestID { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "DeleteRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteRequest : object, IExtensibleDataObject
    {
        [DataMember(IsRequired = true)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)]
        public string ReasonForRequest { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "DeleteResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [DataContract(Name = "EstablishResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    [Serializable]
    public class EstablishResult : object, IExtensibleDataObject
    {
        [DataMember(IsRequired = true)]
        public long MCIID { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 1)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "ReverifyResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "DemographicSearchRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchRequest : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public Name PrimaryName { get; set; }

        [DataMember(IsRequired = true)]
        public long SSN { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public DateTime DOB { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public string Gender { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public short MinimumMatchScore { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public bool IncludeAliasSSN { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 6)]
        public Name[] AliasName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 7)]
        public string UserName { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 8)]
        public string ReasonForRequest { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "DemographicSearchResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DemographicSearchResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DemographicSearchMatch[] Match { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "RetrieveAlertRequest", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertRequest : object, IExtensibleDataObject
    {
        [DataMember(IsRequired = true)]
        public short AlertSequenceNumber { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "RetrieveAlertResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RetrieveAlertResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 1)]
        public Alert[] Alerts { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public bool MoreAlertsExist { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AddAliasResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public AliasResult AddAliasResult;

        public AddAliasResponseBody() {}

        public AddAliasResponseBody(AliasResult AddAliasResult) => this.AddAliasResult = AddAliasResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AddAliasRequestBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public AliasRequest RequestDetails;

        public AddAliasRequestBody() {}

        public AddAliasRequestBody(AliasRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearRequestBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public AlternateClearanceRequest RequestDetails;

        public AlternateClearRequestBody() {}

        public AlternateClearRequestBody(AlternateClearanceRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public AlternateClearanceResult AlternateClearResult;

        public AlternateClearResponseBody() {}

        public AlternateClearResponseBody(AlternateClearanceResult AlternateClearResult) => this.AlternateClearResult = AlternateClearResult;
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "AlternateClearanceResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class AlternateClearanceResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearRequestBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public ClearanceRequest RequestDetails;

        public ClearRequestBody() {}

        public ClearRequestBody(ClearanceRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ClearResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public ClearanceResult ClearResult;

        public ClearResponseBody() {}

        public ClearResponseBody(ClearanceResult ClearResult) => this.ClearResult = ClearResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteRequest1Body
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public DeleteRequest DeleteDetails;

        public DeleteRequest1Body() {}

        public DeleteRequest1Body(DeleteRequest DeleteDetails) => this.DeleteDetails = DeleteDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class DeleteResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public DeleteResult DeleteResult;

        public DeleteResponseBody() {}

        public DeleteResponseBody(DeleteResult DeleteResult) => this.DeleteResult = DeleteResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class EstablishRequest1Body
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public EstablishRequest EstablishDetails;

        public EstablishRequest1Body() {}

        public EstablishRequest1Body(EstablishRequest EstablishDetails) => this.EstablishDetails = EstablishDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class EstablishResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public EstablishResult EstablishResult;

        public EstablishResponseBody() {}

        public EstablishResponseBody(EstablishResult EstablishResult) => this.EstablishResult = EstablishResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RemoveAliasRequestBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public AliasRequest RequestDetails;

        public RemoveAliasRequestBody() {}

        public RemoveAliasRequestBody(AliasRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class RemoveAliasResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public AliasResult RemoveAliasResult;

        public RemoveAliasResponseBody() {}

        public RemoveAliasResponseBody(AliasResult RemoveAliasResult) => this.RemoveAliasResult = RemoveAliasResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyRequest1Body
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public ReverifyRequest ReverifyDetails;

        public ReverifyRequest1Body() {}

        public ReverifyRequest1Body(ReverifyRequest ReverifyDetails) => this.ReverifyDetails = ReverifyDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class ReverifyResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public ReverifyResult ReverifyResult;

        public ReverifyResponseBody() {}

        public ReverifyResponseBody(ReverifyResult ReverifyResult) => this.ReverifyResult = ReverifyResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public SelectResult SelectResult;

        public SelectResponseBody() {}

        public SelectResponseBody(SelectResult SelectResult) => this.SelectResult = SelectResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SearchByDemographicsRequestBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public DemographicSearchRequest SearchDetails;

        public SearchByDemographicsRequestBody() {}

        public SearchByDemographicsRequestBody(DemographicSearchRequest SearchDetails) => this.SearchDetails = SearchDetails;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SearchByDemographicsResponseBody
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public DemographicSearchResult SearchByDemographicsResult;

        public SearchByDemographicsResponseBody() {}

        public SearchByDemographicsResponseBody(DemographicSearchResult SearchByDemographicsResult) => this.SearchByDemographicsResult = SearchByDemographicsResult;
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectRequest1Body
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public SelectRequest RequestDetails;

        public SelectRequest1Body() {}

        public SelectRequest1Body(SelectRequest RequestDetails) => this.RequestDetails = RequestDetails;
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "SelectResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class SelectResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [Serializable]
    [DataContract(Name = "UpdateResult", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateResult : object, IExtensibleDataObject
    {
        [DataMember(EmitDefaultValue = false)]
        public ValidationError[] Errors { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }

    [DebuggerStepThrough]
    [DataContract(Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public class UpdateRequest1Body
    {
        [DataMember(EmitDefaultValue = false, Order = 0)]
        public UpdateRequest UpdateDetails;

        public UpdateRequest1Body() {}

        public UpdateRequest1Body(UpdateRequest UpdateDetails) => this.UpdateDetails = UpdateDetails;
    }

    #region Enums

    [DataContract(Name = "NameType", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum NameType
    {
        [EnumMember] PrimaryName = 0,
        [EnumMember] Alias = 1,
        [EnumMember] MaidenName = 2
    }

    [DataContract(Name = "OldNameType", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum OldNameType
    {
        [EnumMember] History = 0,
        [EnumMember] Alias = 1,
        [EnumMember] MaidenName = 2
    }

    [DataContract(Name = "OldSSNType", Namespace = "http://MCIService.webservice.customEntities.business.mci.wisconsin.gov")]
    public enum OldSSNType
    {
        [EnumMember] History = 0,
        [EnumMember] Alias = 1
    }

    #endregion

    #endregion
}
