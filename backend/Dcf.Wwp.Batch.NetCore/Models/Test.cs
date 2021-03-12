using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    /// <summary>
    /// Dummy class to use to ensure DI is correctly setup
    /// </summary>
    public class Test : ITest
    {
        #region Properties

        public string Name => "Test Hello World Object";

        #endregion

        #region Methods

        public string SayHi()
        {
            return ("Hello World");
        }

        #endregion
    }
}
