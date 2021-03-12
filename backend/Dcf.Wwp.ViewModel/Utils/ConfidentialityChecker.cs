using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace Dcf.Wwp.Api.Library.Utils
{
    public class ConfidentialityChecker : IConfidentialityChecker
    {
        #region Properties

        private readonly IMemoryCache      _memoryCache;
        private readonly ICwwKeySecService _cwwKeySvc;
        private readonly IRepository       _repo;

        public int Cached  { get; set; }
        public int Web     { get; set; }
        public int Retries { get; set; }

        public int Total => Web + Retries;

        #endregion

        #region Methods

        public ConfidentialityChecker(ICwwKeySecService cwwKeySvc, IMemoryCache memoryCache, IRepository repo)
        {
            _cwwKeySvc   = cwwKeySvc;
            _memoryCache = memoryCache;
            _repo        = repo;

            Cached  = 0;
            Web     = 0;
            Retries = 0;
        }

        public GetKeySecurityInfoResponse Check(decimal pin, IEnumerable<IConfidentialPinInformation> confidentialPinInfo = null, string fepMFId = null)
        {
            var callCww = true;

            fepMFId = fepMFId?.Trim();
            var pinKey = $"pin-{pin}";

            if (!_memoryCache.TryGetValue(pinKey, out GetKeySecurityInfoResponse response))
            {
                if (string.IsNullOrEmpty(fepMFId))
                {
                    var wwOrLfPrograms = _repo.GetLastWWOrLFInstance(pin).ToList();

                    if (wwOrLfPrograms.Any(i => i.ProgramCode == EnrolledProgram.W2ProgramCode || i.ProgramCode == EnrolledProgram.LFProgramCode))
                    {
                        var program = wwOrLfPrograms.FirstOrDefault();
                        fepMFId = program?.WorkerId;
                    }
                    else
                    {
                        fepMFId = null;
                    }
                }
                else
                {
                    if (confidentialPinInfo == null)
                    {
                        confidentialPinInfo = _repo.GetConfidentialPinInfo(pin);
                    }

                    var isWwpConfidential = confidentialPinInfo.Any(i => i.IsConfidential == true);
                    callCww = isWwpConfidential != true;
                }

                if (callCww)
                {
                    Web++;
                    response = CallCwwWebSvc(pin, fepMFId);

                    if (response?.Errors != null && response.Errors.Any())
                    {
                        Retries++;
                        response = CallCwwWebSvc(pin);
                    }

                    if (response?.CaseCofidentailStatus == "N")
                    {
                        var isConf = _repo.HasConfidentialInfomation(pin);
                        response.CaseCofidentailStatus = isConf ? "Y" : "N";
                    }
                }
                else
                {
                    var isConf = _repo.HasConfidentialInfomation(pin);

                    if (response == null)
                    {
                        response = new GetKeySecurityInfoResponse { Errors = new ValidationErrorType[0], Status = "S" };
                    }

                    response.CaseCofidentailStatus = isConf ? "Y" : "N";
                }

                _memoryCache.Set(pinKey, response, TimeSpan.FromMinutes(5));
            }
            else
            {
                Cached++;
            }

            return response;
        }

        public GetKeySecurityInfoResponse CallCwwWebSvc(decimal pin, string workerId = null)
        {
            GetKeySecurityInfoResponse res ;

            var req = new GetKeySecurityInfoRequest
                      {
                          ExternalAgencyId = "TANF", //TODO: make this a constant
                          PINNumber        = pin.ToString(CultureInfo.InvariantCulture).PadLeft(10, '0'),
                          FEPID            = workerId
                      };

            res = _cwwKeySvc.GetKeySecurityInfo(req);

            return res;
        }

        #endregion
    }
}
