using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using QuestGame.WebApi;

namespace QuestGame.TestProject.IntegrationTests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void QuestGame_Authorize1_Success()
        {
            var driver = ChromeDriver; // new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:9243/Home/Login");

            var login = driver.FindElementById("loginTextBox");
            login.SendKeys("admin@admin.com");

            var password = driver.FindElementById("passwordTextBox");
            password.SendKeys("qwerty");

            var button = driver.FindElementById("enterButton");
            button.Click();

            Thread.Sleep(5000);

            var name = driver.FindElementById("userName");

            Assert.IsTrue(name.Text.Contains("admin@admin.com"));

            driver.Close();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // Start IISExpress
            StartIIS();

            this.ChromeDriver = new ChromeDriver();
        }


        [TestCleanup]
        public void TestCleanup()
        {
            // Ensure IISExpress is stopped
            if (_iisProcess.HasExited == false)
            {
                _iisProcess.Kill();
            }

            this.ChromeDriver.Quit();
        }

        public ChromeDriver ChromeDriver { get; set; }
        const int iisPort = 7085;
        private string _applicationName = "QuestGame.WebApi";
        private Process _iisProcess;

        private void StartIIS()
        {
            var applicationPath = GetApplicationPath(_applicationName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:{0} /port:{1}", applicationPath, iisPort);
            _iisProcess.Start();
        }


        protected virtual string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return Path.Combine(solutionFolder, applicationName);
        }

        public string GetAbsoluteUrl(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return String.Format("http://localhost:{0}{1}", iisPort, relativeUrl);
        }


        [TestMethod]
        public void QuestGame_Authorize_Success()
        {
            //var dr = FirefoxDriver();
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:9243/Home/Login");

            var login = driver.FindElementById("loginTextBox");
            login.SendKeys("admin@admin.com");

            var password = driver.FindElementById("passwordTextBox");
            password.SendKeys("qwerty");

            var button = driver.FindElementById("enterButton");
            button.Click();

            Thread.Sleep(5000);

            var name = driver.FindElementById("userName");

            Assert.IsTrue(name.Text.Contains("admin@admin.com"));

            driver.Close();
        }
    }
}
