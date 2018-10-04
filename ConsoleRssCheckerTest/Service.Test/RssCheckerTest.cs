namespace ConsoleRssCheckerTest.Service.Test
{
    using ConsoleRssChecker.Repository;
    using ConsoleRssChecker.Service;
    using ConsoleRssChecker.Settings;
    using ConsoleRssChecker.Utility;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class RssCheckerTest
    {
        Mock<ILogger<RssChecker>> _loggerMock;
        Mock<IOptions<AppSettings>> _configMock;
        Mock<IRssParser> _parserMock;
        Mock<ICompanyRssRepository> _repoMock;
        Mock<IInactiveCompanyRepository> _companyMock;
        private IRssChecker _rssChecker;


        [TestInitialize]
        public void Setup()
        {
            IEnumerable<Tuple<string,string>> companyRssTuples = new List<Tuple<string,string>>()
            {
                new Tuple<string, string>("company1","url1"),
                new Tuple<string, string>("company2","url2")
            };
            var appSettings = new AppSettings()
            {
                InactiveDays = 3
            };

            _loggerMock = new Mock<ILogger<RssChecker>>();
            _configMock = new Mock<IOptions<AppSettings>>();
            _parserMock = new Mock<IRssParser>();
            _repoMock = new Mock<ICompanyRssRepository>();
            _companyMock = new Mock<IInactiveCompanyRepository>();

            _configMock.Setup(s => s.Value).Returns(appSettings);

            _rssChecker = new RssChecker(_loggerMock.Object, _configMock.Object, _parserMock.Object, _repoMock.Object, _companyMock.Object);

            _repoMock.Setup(s => s.GetAllCompanyRss()).ReturnsAsync(companyRssTuples);
        }

        [TestMethod]
        public async Task GetInactiveCompanyList_OnExecute_ReturnsAll()
        {
            _parserMock.Setup(s => s.GetLastBuildDate(It.IsAny<string>()))
                .ReturnsAsync(DateTime.Now.AddDays(-5));

            var output = await _rssChecker.CreateInactiveCompanyList();

            Assert.IsNotNull(output);
            Assert.AreEqual(2, output.Count());
        }

        [TestMethod]
        public async Task GetInactiveCompanyList_OnExecute_ReturnsNone()
        {
            _parserMock.Setup(s => s.GetLastBuildDate(It.IsAny<string>()))
                .ReturnsAsync(DateTime.Now.AddDays(-1));

            var output = await _rssChecker.CreateInactiveCompanyList();

            Assert.IsNotNull(output);
            Assert.AreEqual(0, output.Count());
        }
    }
}
