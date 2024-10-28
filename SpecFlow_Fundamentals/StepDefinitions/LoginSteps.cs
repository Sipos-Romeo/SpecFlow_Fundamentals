using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlow_Fundamentals.Pages;
using SpecFlow_Fundamentals.TestData.Models;

namespace SpecFlow_Fundamentals.StepDefinitions
{
    [Binding, Scope(Tag = "Authentcation")]
    public class LoginSteps
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly IWebDriver _webDriver;
        private LoginPage LoginPage => new LoginPage(_webDriver);
        private static Dictionary<string, Credentials> SpecificCredentials => new Dictionary<string, Credentials>
        {
            { "default", new Credentials(BaseConfig.DefaultUser, BaseConfig.Password) },
            { "locked out", new Credentials(BaseConfig.LockedOutUser, BaseConfig.Password) },
            { "problem", new Credentials(BaseConfig.ProblemUser, BaseConfig.Password) },
            { "performance glitch", new Credentials(BaseConfig.PerformanceGlitchUser, BaseConfig.Password) },
            { "incorrect", new Credentials(BaseConfig.IncorrectUser, BaseConfig.IncorrectPassword) },
        };

        public LoginSteps(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        [Given(@"I am on the login page")]
        public void IAmOnTheLoginPage()
        {
            LoginPage.Load();
        }

        [When(@"I enter the (default|locked out|problem|performance glitch|incorrect) username and (password|incorrect password)")]
        public void IEnterTheUsernameAndPassword(string username, string password)
        {
            LoginPage
                .FillUsername(SpecificCredentials[username].Username)
                .FillPassword(SpecificCredentials[username].Password);
        }

        [When(@"I click the login button")]
        public void IClockOnLoginButton()
        {
            LoginPage.ClickLogin();
        }

        [Then(@"I should be redirected to the inventory page")]
        public void IShouldBeRedirectedToTheInventoryPage()
        {
            //TODO: add InventoryPage.IsLoaded() check
            Assert.True(LoginPage.GetWebDriver().Url.EndsWith("/inventory.html"), "Browser is not on inventory page");
        }

        [Then(@"I should see the error message")]
        public void IShouldSeeAErrorMessage()
        {
            Assert.True(LoginPage.IsLoginErrorMessageDisplayed());
        }
       
        [Then(@"the error message shoud contain the text ""([^""]*)""")]
        public void ThenTheErrorMessageShoudContainTheText(string expectedMessage)
        {
           Assert.AreEqual(expectedMessage, LoginPage.GetLogInErrorMessage());
        }

    }
}
