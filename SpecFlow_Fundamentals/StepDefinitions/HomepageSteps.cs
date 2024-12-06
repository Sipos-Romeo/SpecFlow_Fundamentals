using OpenQA.Selenium;
using SpecFlow_Fundamentals.Pages;
using SpecFlow_Fundamentals.TestData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow_Fundamentals.StepDefinitions
{
    [Binding, Scope(Tag = "UI")]
    public class HomepageSteps
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        private readonly IWebDriver _webDriver;
        private HomepageSteps Homepage => new HomepageSteps(_webDriver);
       
        public HomepageSteps(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

    }
}
