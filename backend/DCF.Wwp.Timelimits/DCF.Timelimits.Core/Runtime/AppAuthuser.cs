using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface.Core;

namespace DCF.Timelimits.Processors
{
    public class AppAuthuser : IAuthUser
    {
        public string AgencyCode
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        public bool IsAuthenticated => true;

        public string Username
        {
            get => "WWP-BATCH";

            set => throw new NotImplementedException();
        }

        public string MainFrameId
        {
            get => "WWPBAT";

            set => throw new NotImplementedException();
        }

        public List<string> Authorizations { get; set; }

        public string WIUID
        {
            get => "WWP-BATCH";

            set => throw new NotImplementedException();
        }

        public DateTime? CDODate { get; set; }
    }
}
