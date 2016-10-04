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
using QuestGame.TestProject.Constants;

namespace QuestGame.TestProject.IntegrationTests
{
    [TestClass]
    public class AccountTest
    {
        private static Random rnd = new Random();
        private const string baseUrl = @"http://localhost:9243/";

        [TestMethod]
        public void QuestGame_Authorize_Success()
        {
            using (var driver = new ChromeDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                driver.Navigate().GoToUrl(baseUrl + WebMethods.HomeLogin);

                var login = driver.FindElementById("loginTextBox");
                login.SendKeys("admin@admin.com");

                var password = driver.FindElementById("passwordTextBox");
                password.SendKeys("qwerty");

                var button = driver.FindElementById("enterButton");
                button.Click();

                var name = driver.FindElementById("userName");

                Assert.IsTrue(name.Text.Contains("admin@admin.com"));
            } 
        }

        [TestMethod]
        public void QuestGame_Registration_Succes()
        {
            var rndEmail = GetRndEmail();
            const string password = "qwerty";

            using (var driver = new ChromeDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                driver.Navigate().GoToUrl(baseUrl + WebMethods.HomeRegister);

                // Регистрация     
                var loginElement = driver.FindElementById("Email");
                loginElement.SendKeys(rndEmail);

                var passwordElement = driver.FindElementById("Password");
                passwordElement.SendKeys(password);

                var confirmPassword = driver.FindElementById("ConfirmPassword");
                confirmPassword.SendKeys(password);

                var button = driver.FindElementById("registerButton");
                button.Click();

                // Логинимся
                driver.Navigate().GoToUrl(baseUrl + WebMethods.HomeLogin);

                var login1 = driver.FindElementById("loginTextBox");
                login1.SendKeys(rndEmail);

                var password1 = driver.FindElementById("passwordTextBox");
                password1.SendKeys("qwerty");

                var button1 = driver.FindElementById("enterButton");
                button1.Click();

                var name = driver.FindElementById("userName");

                Assert.IsTrue(name.Text.Contains(rndEmail));   
            }                     
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
