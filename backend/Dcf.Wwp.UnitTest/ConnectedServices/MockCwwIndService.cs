using System;
using System.Threading.Tasks;
using Dcf.Wwp.ConnectedServices.Cww;

namespace Dcf.Wwp.UnitTest.ConnectedServices
{
    public class MockCwwIndService : ICwwIndService
    {
        public bool invokeException = false;

        public GetIndvDemographicsResponse GetIndvDemographics(GetIndvDemographicsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<GetIndvDemographicsResponse> GetIndvDemographicsAsync(GetIndvDemographicsRequest request)
        {
            throw new NotImplementedException();
        }

        public InsertIndvKeyDemographicsResponse InsertIndvKeyDemographics(InsertIndvKeyDemographicsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<InsertIndvKeyDemographicsResponse> InsertIndvKeyDemographicsAsync(InsertIndvKeyDemographicsRequest request)
        {
            throw new NotImplementedException();
        }

        public UpdateIndvKeyDemographicsResponse UpdateIndvKeyDemographics(UpdateIndvKeyDemographicsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateIndvKeyDemographicsResponse> UpdateIndvKeyDemographicsAsync(UpdateIndvKeyDemographicsRequest request)
        {
            throw new NotImplementedException();
        }

        public UpdateRaceEthnicityInformationResponse UpdateRaceEthnicityInformation(UpdateRaceEthnicityInformationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateRaceEthnicityInformationResponse> UpdateRaceEthnicityInformationAsync(UpdateRaceEthnicityInformationRequest request)
        {
            throw new NotImplementedException();
        }

        public UpdateFEPInformationResponse UpdateFEPInformation(UpdateFEPInformationRequest request)
        {
            if (invokeException)
            {
                throw new ArgumentNullException();
            }
            else
            {
                return new UpdateFEPInformationResponse();
            }
        }

        public Task<UpdateFEPInformationResponse> UpdateFEPInformationAsync(UpdateFEPInformationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
