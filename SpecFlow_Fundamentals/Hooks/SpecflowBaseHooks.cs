using BoDi;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SpecFlow_Fundamentals.StepDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow_Fundamentals.Hooks
{
    [Binding]
    public class SpecflowBaseHooks
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver;

        public SpecflowBaseHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Initialize your WebDriver instance here (e.g., ChromeDriver)
            _driver = new ChromeDriver();

            // Pass the WebDriver instance to the steps class constructor
            var loginSteps = new LoginSteps(_driver);

            // Register the WebDriver instance in the container
            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
            _objectContainer.RegisterInstanceAs(loginSteps);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Close and quit the WebDriver instance after the scenario
            _driver?.Quit();
        }
    }
}
