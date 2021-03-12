using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Common
{
    /// <summary>
    /// Creates sequential Guids
    /// see mjhtodd/SequentialGuid
    /// </summary>
    public class SequentialGuidGenerator
    {
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();

        public static Guid Create(Guid? start = null)
        {
            //var randomBytes = new byte[10];
            //Rng.GetBytes(randomBytes);

            // An alternate method: use a normally-created GUID to get our initial
            // random data:
            Byte[] randomBytes = (start ?? Guid.NewGuid()).ToByteArray();
            // This is faster than using RNGCryptoServiceProvider, but I don't
            // recommend it because the .NET Framework makes no guarantee of the
            // randomness of GUID data, and future versions (or different
            // implementations like Mono) might use a different method.

            // Now we have the random basis for our GUID.  Next, we need to
            // create the six-byte block which will be our timestamp.

            // We start with the number of milliseconds that have elapsed since
            // DateTime.MinValue.  This will form the timestamp.  There's no use
            // being more specific than milliseconds, since DateTime.Now has
            // limited resolution.

            // Using millisecond resolution for our 48-bit timestamp gives us
            // about 5900 years before the timestamp overflows and cycles.
            // Hopefully this should be sufficient for most purposes. :)
            Int64 timestamp = DateTime.UtcNow.Ticks / 10000L;

            // Then get the bytes
            Byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            // Since we're converting from an Int64, we have to reverse on
            // little-endian systems.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            Byte[] guidBytes = new Byte[16];

            // For sequential-at-the-end versions (SQL SERVER), we copy the random data first,
            // followed by the timestamp.
            Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
            Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
            

            return new Guid(guidBytes);
        }

    }
}
