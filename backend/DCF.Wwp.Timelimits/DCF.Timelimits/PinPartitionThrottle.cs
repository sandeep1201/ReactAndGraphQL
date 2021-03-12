using System;
using System.Collections.Generic;

namespace DCF.Timelimits
{
    public class PinPartitionThrottle : PartitionThrottle<Decimal>
    {

        public PinPartitionThrottle(int partitionQueueSize, int? maxQueueSize = default(int?), TimeSpan? timeout = default(TimeSpan?)) : base(partitionQueueSize, maxQueueSize, timeout)
        {

        }

        protected override int GetQueueKey(decimal identifier)
        {
            if (identifier < 1000000000) return 0;
            if (identifier < 2000000000) return 1;
            if (identifier < 3000000000) return 2;
            if (identifier < 4000000000) return 3;
            if (identifier < 5000000000) return 4;
            if (identifier < 6000000000) return 5;
            if (identifier < 7000000000) return 6;
            if (identifier < 8000000000) return 7;
            if (identifier < 9000000000) return 8;


            return 9;
        }

        
    }
}