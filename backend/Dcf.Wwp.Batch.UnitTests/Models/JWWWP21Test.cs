using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Batch.Interfaces;
using Dcf.Wwp.Batch.Models;
using Dcf.Wwp.Batch.UnitTests.Infrastructure;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dcf.Wwp.Batch.UnitTests.Models
{
    [TestClass]
    public class JWWWP21Test
    {
        #region Properties

        private MockBaseJob               _mockBaseJob;
        private IWwpPathConfig            _wwpPathConfig;
        private MockHttpWebRequestWrapper _mockHttpWebRequestWrapper;
        private JWWWP21                   _jwwwp21;

        #endregion

        #region Methods

        [TestInitialize]
        public void Initialize()
        {
            var cb = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("Dcf.Wwp.Batch.json")
                     .Build();

            var wwpPath        = cb.GetSection("WWPPath");
            var wwpPathConfig  = wwpPath.Get<List<WwpPathConfig>>();

            _wwpPathConfig             = wwpPathConfig.FirstOrDefault(i => i.Env == "WWPDEV");
            _mockBaseJob               = new MockBaseJob { JobName = nameof(JWWWP21Test) };
            _mockHttpWebRequestWrapper = new MockHttpWebRequestWrapper();
            _jwwwp21                   = new JWWWP21(_mockBaseJob, _wwpPathConfig, _mockHttpWebRequestWrapper);
        }

        [TestMethod]
        public void Run_CallsBaseJobRunSproc()
        {
            _jwwwp21.Run();

            Assert.IsTrue(_mockBaseJob.HasRunSprocBeenCalled);
        }

        [TestMethod]
        public void Run_WhenSprocResultCountIsZero_ReturnsEmptyDataTable()
        {
            _mockBaseJob.SprocReturnCount = 0;

            Assert.IsTrue(_jwwwp21.Run().Rows.Count == 0);
            Assert.AreEqual(1, _mockBaseJob.SprocRunCount);
        }

        [TestMethod]
        public void Run_WhenSprocResultCountIsGreaterThanZero()
        {
            var appender = new log4net.Appender.MemoryAppender();

            BasicConfigurator.Configure(appender);

            _jwwwp21.Run();

            Assert.IsTrue(appender.GetEvents().Select(i => i.Level.Name == "INFO" && i.RenderedMessage == "Updating wwp.SpecialInitiative").Any(), "LogsUpdateTableStatement");
            Assert.IsTrue(_mockBaseJob.HasExecSqlBeenCalled,                                                                                       "CallsBaseJobExecSql");
        }

        [TestMethod]
        public void Run_WhenSprocResultCountIsGreaterThanZero_CreatesHttpWebRequest()
        {
            _jwwwp21.Run();

            Assert.IsTrue(_mockHttpWebRequestWrapper.RequestUri.AbsoluteUri.Contains($"{_wwpPathConfig.Path}api/update-placement"), "Check URL");
            Assert.AreEqual("application/json", _mockHttpWebRequestWrapper.ContentType, "Check Content Type");
            Assert.AreEqual("PUT",              _mockHttpWebRequestWrapper.Method,      "Check Method");
            Assert.IsTrue(_mockHttpWebRequestWrapper.HasGetRequestStreamBeenCalled, "Check if GetRequestStream Method is Called");
            Assert.IsTrue(_mockHttpWebRequestWrapper.HasGetResponseBeenCalled,      "Check if GetResponse Method is Called");
            Assert.AreEqual(2, _mockBaseJob.SprocRunCount, "Calls RunSproc 2 Times");
            Assert.AreEqual(2, _mockBaseJob.SqlRunCount,   "Calls ExecSql 2 Times");
        }

        #endregion
    }
}
