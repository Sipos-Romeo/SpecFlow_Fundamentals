using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow_Fundamentals.Pages
{
    public class HomePage : BasePage
    {
        private readonly By UsernameField = By.Id("user-name");
        private readonly By PasswordFied = By.Id("password");
        private readonly By LoginBtn = By.Id("login-button");
        private readonly By ProductsText = By.ClassName("title");
        private readonly By SwagLabsAppLogo = By.ClassName("app_logo");
        public IWebElement ErrorMessage => GetElement(By.ClassName("error-message-container"));

        public HomePage(IWebDriver driver) : base(driver)
        {
        }
        public HomePage Load()
        {
            NavigateTo(new Uri(BaseConfig.BaseUrl + "/inventory.html"));
            IsLoaded();
            return this;
        }

        public HomePage IsLoaded()
        {
            WaitElementToBeVisible(LoginBtn);
            return this;
        }
    }
}
