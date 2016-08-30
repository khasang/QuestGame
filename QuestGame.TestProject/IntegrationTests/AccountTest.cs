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
        private static Random rnd = new Random();

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

        [TestMethod]
        public void QuestGame_Registration_Succes()
        {
            var rndEmail = GetRndEmail();

            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:9243/Home/Register");

            // Регистрация

            var login = driver.FindElementById("Email");
            login.SendKeys(rndEmail);

            var password = driver.FindElementById("Password");
            password.SendKeys("qwerty");

            var confirmPassword = driver.FindElementById("ConfirmPassword");
            confirmPassword.SendKeys("qwerty");

            var button = driver.FindElementById("registerButton");
            button.Click();

            Thread.Sleep(5000);

            // Логинимся

            driver.Navigate().GoToUrl("http://localhost:9243/Home/Login");

            var login1 = driver.FindElementById("loginTextBox");
            login1.SendKeys(rndEmail);

            var password1 = driver.FindElementById("passwordTextBox");
            password1.SendKeys("qwerty");

            var button1 = driver.FindElementById("enterButton");
            button1.Click();

            Thread.Sleep(5000);

            var name = driver.FindElementById("userName");

            Assert.IsTrue(name.Text.Contains(rndEmail));

            driver.Close();
        }

        private string GetRndEmail()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < 8; i++)
                text.Append("abcdefghijklmnopqrstuvwxyz"[rnd.Next(0, 26)]);
            text.Append("@admin.com");

            return text.ToString();
        }
    }
}
