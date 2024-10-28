using FluentAssertions.Common;
using OpenQA.Selenium;
using SpecFlow_Fundamentals.Helpers;
using SpecFlow_Fundamentals.TestData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow_Fundamentals.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By UsernameField = By.Id("user-name");
        private readonly By PasswordFied = By.Id("password");
        private readonly By LoginBtn = By.Id("login-button");
        public IWebElement ErrorMessage => GetElement(By.ClassName("error-message-container"));

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public LoginPage Load()
        {
            NavigateTo(new Uri(BaseConfig.BaseUrl));
            IsLoaded();
            return this;
        }

        public LoginPage IsLoaded()
        {
            WaitElementToBeVisible(LoginBtn);
            return this;
        }

        public LoginPage FillUsername(string username)
        {
            SendText(UsernameField, username);
            return this;
        }

        public LoginPage FillPassword(string password)
        {
            SendText(PasswordFied, password);
            return this;
        }

        public LoginPage ClickLogin()
        {
            Click(LoginBtn);
            return this;
        }

        public  bool IsLoginErrorMessageDisplayed()
        {
            WaitElementToBeDisplayed(ErrorMessage);
            return ErrorMessage.Displayed;
        }
        public string GetLogInErrorMessage()
        {
            WaitElementToBeDisplayed(ErrorMessage);
            return ErrorMessage.Text;
        }
    }
}
