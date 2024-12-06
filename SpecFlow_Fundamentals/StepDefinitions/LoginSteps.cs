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
            { "incorrect", new Credentials(BaseConfig.IncorrectUser, BaseConfig.Password) },
            { "error user", new Credentials(BaseConfig.ErrorUser, BaseConfig.Password) },
            { "visual user", new Credentials(BaseConfig.VisualUser, BaseConfig.Password) }
        };

        public LoginSteps(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        [StepDefinition(@"I am on the login page")]
        public void IAmOnTheLoginPage()
        {
            LoginPage.Load();
        }

        [StepDefinition(@"I enter the (default|locked out|problem|performance glitch|incorrect|error user|visual user) username and (password|incorrect password)")]
        public void IEnterTheUsernameAndPassword(string username, string password)
        {
            LoginPage
                .FillUsername(SpecificCredentials[username].Username)
                .FillPassword(SpecificCredentials[username].Password);
        }

        [StepDefinition(@"I click the login button")]
        public void IClockOnLoginButton()
        {
            LoginPage.ClickLogin();
        }

        [StepDefinition(@"I should be redirected to the inventory page")]
        public void IShouldBeRedirectedToTheInventoryPage()
        {
            //TODO: add InventoryPage.IsLoaded() check
            Assert.That(LoginPage.GetWebDriver().Url.EndsWith("/inventory.html"), "Browser is not on inventory page");
        }

        [StepDefinition(@"I should see the error message")]
        public void IShouldSeeAErrorMessage()
        {
            Assert.That(LoginPage.IsLoginErrorMessageDisplayed());
        }
       
        [StepDefinition(@"the error message shoud contain the text ""([^""]*)""")]
        public void ThenTheErrorMessageShoudContainTheText(string expectedMessage)
        {
           Assert.That(expectedMessage, Is.EqualTo(LoginPage.GetLogInErrorMessage()), "Error massage is not as expected");       
        }



    }
}
