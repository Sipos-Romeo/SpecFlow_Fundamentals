using NLog;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SpecFlow_Fundamentals.Pages.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logger = NLog.Logger;

namespace SpecFlow_Fundamentals.Pages
{
    public class BasePage
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IWebDriver _webDriver;

        public BasePage(IWebDriver webDriver)
        {
            _webDriver = webDriver ?? throw new ArgumentNullException("webDriver");
        }

        #region Getters 

        public IWebDriver GetWebDriver()
        {
            return _webDriver;
        }

        public WebDriverWait GetWebDriverWait(int timeOutSeconds = 12)
        {
            WebDriverWait webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(timeOutSeconds));
            webDriverWait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
            return webDriverWait;
        }

        public WebDriverWait GetWebDriverWait(int timeOutSeconds, params Type[] exceptionTypes)
        {
            WebDriverWait webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(timeOutSeconds));
            webDriverWait.IgnoreExceptionTypes(exceptionTypes);
            return webDriverWait;
        }
        
        public string GetUrl()
        {
            return _webDriver.Url;
        }

        public string GetTitle()
        {
            return _webDriver.Title;
        }

        public void NavigateTo(Uri url)
        {
            _webDriver.Navigate().GoToUrl(url);
        }

        public string GetText(IWebElement webElement)
        {
            return webElement.Text;
        }

        public string GetText(By locator)
        {
            return GetElement(locator).Text;
        }

        public IWebElement GetElement(By locator, int timeOutSeconds = 12) 
        {
            try
            {
                SetImplicitWaitToCustomValue(DelayType.ShortDelay);
                return GetWebDriverWait(timeOutSeconds).Until(ExpectedConditions.ElementExists(locator));
            }
            finally
            {
                SetImplicitWaitToDefaultValue();
            }
        }
        #endregion

        #region Send text
        /// <summary>
        /// This approach helps ensure that even if there are temporary issues or intermittent problems accessing the element, 
        /// your test code attempts to recover and continue the interaction. It's a form of defensive programming to handle 
        /// various scenarios and provide a more robust automation script.
        /// </summary>
        public void SendText(IWebElement element, string text)
        {
            try
            {
                WaitElementToBeDisplayed(element);
                element.Clear();
                element.SendKeys(text);
            }
            catch (WebDriverException)
            {
                ScrollToElement(element);
                element.Clear();
                element.SendKeys(text);
            }
        }

        public void SendText(By locator, string text)
        {
            Clear(locator);
            GetElement(locator).SendKeys(text);
        }

        public void SendText(IWebElement element, string text, int timeOutSeconds)
        {
            try
            {
                WaitElementToBeDisplayed(element, timeOutSeconds);
                element.Clear();
                element.SendKeys(text);
            }
            catch (WebDriverException)
            {
                ScrollToElement(element);
                element.Clear();
                element.SendKeys(text);
            }
        }

        public void SendText(By locator, string text, int timeOutSeconds)
        {
            Clear(locator);
            GetElement(locator).SendKeys(text);
        }
        #endregion

        #region Clear
        public void Clear(By locator)
        {
            try
            {
                WaitElementToBeVisible(locator);
                GetElement(locator).Clear();
            }
            catch (WebDriverException)
            {
                ScrollToElement(locator);
                GetElement(locator).Clear();
            }
        }
        #endregion

        #region Click
        public void Click(IWebElement element)
        {
            try
            {
                WaitElementToBeClickable(element);
                element.Click();
            }
            catch (WebDriverException)
            {
                ScrollToElement(element);
                element.Click();
            }
        }

        public void Click(By locator)
        {
            try
            {
                WaitElementToBeClickable(locator);
                GetElement(locator).Click();
            }
            catch (WebDriverException)
            {
                ScrollToElement(locator);
                GetElement(locator).Click();
            }
        }

        public void Click(IWebElement element, int timeOutSeconds)
        {
            try
            {
                WaitElementToBeClickable(element, timeOutSeconds);
                element.Click();
            }
            catch (WebDriverException)
            {
                ScrollToElement(element);
                element.Click();
            }
        }

        public void Click(By locator, int timeOutSeconds)
        {
            try
            {
                WaitElementToBeClickable(locator, timeOutSeconds);
                GetElement(locator).Click();
            }
            catch (WebDriverException)
            {
                ScrollToElement(locator);
                GetElement(locator).Click();
            }
        }

        public void ClickJS(IWebElement element)
        {
            try
            {
                ClickWithJS(element);
            }
            catch (WebDriverException exception)
            {
                HardcodedWait(DelayType.ShortDelay);
                Log.Info(exception, "Second attempt to click!");
                ClickWithJS(element);
            }

            HardcodedWait(DelayType.ShortDelay);
        }

        public void ClickJS(By locator)
        {
            try
            {
                ClickWithJS(locator);
            }
            catch (WebDriverException exception)
            {
                HardcodedWait(DelayType.ShortDelay);
                Log.Info(exception, "Second attempt to click!");
                ClickWithJS(locator);
            }

            HardcodedWait(DelayType.ShortDelay);
        }
        #endregion

        #region Waiters
        public void SetImplicitWaitToCustomValue(DelayType delayType)
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds((double)delayType);
        }

        public void SetImplicitWaitToDefaultValue()
        {
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6.0);
        }

        public void WaitElementToBeDisplayed(IWebElement element, int timeOutSeconds = 12)
        {
            try
            {
                GetWebDriverWait(timeOutSeconds).Until((IWebDriver _) => element.Displayed);
            }
            catch(WebDriverTimeoutException ex)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 1);
                defaultInterpolatedStringHandler.AppendLiteral("Timed out after ");
                defaultInterpolatedStringHandler.AppendFormatted(timeOutSeconds);
                defaultInterpolatedStringHandler.AppendLiteral(" seconds, while waiting for WebElement to be Displayed!");
                throw new WebDriverTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
            }
        }

        public void WaitElementToNOTBeDisplayed(IWebElement element, int timeOutSeconds = 12)
        {
            try
            {
                GetWebDriverWait(timeOutSeconds).Until((IWebDriver _) => !element.Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 1);
                defaultInterpolatedStringHandler.AppendLiteral("Timed out after ");
                defaultInterpolatedStringHandler.AppendFormatted(timeOutSeconds);
                defaultInterpolatedStringHandler.AppendLiteral(" seconds, while waiting for WebElement to be Displayed!");
                throw new WebDriverTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
            }
        }

        public void WaitElementToBeVisible(By locator, int timeOutSeconds = 12)
        {
            try
            {
                SetImplicitWaitToCustomValue(DelayType.ShortDelay);
                GetWebDriverWait(timeOutSeconds).Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch (WebDriverTimeoutException innerException)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
                defaultInterpolatedStringHandler.AppendLiteral("Timed out after ");
                defaultInterpolatedStringHandler.AppendFormatted(timeOutSeconds);
                defaultInterpolatedStringHandler.AppendLiteral(" seconds, while waiting for ");
                defaultInterpolatedStringHandler.AppendFormatted(locator);
                defaultInterpolatedStringHandler.AppendLiteral(" to be Visible!");
                throw new WebDriverTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
            }
            finally
            {
                SetImplicitWaitToDefaultValue();
            }
        }

        public void WaitElementToNOTBeVisible(By locator, int timeOutSeconds = 12)
        {
            try
            {
                SetImplicitWaitToCustomValue(DelayType.ShortDelay);
                GetWebDriverWait(timeOutSeconds).Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            catch (WebDriverTimeoutException innerException)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 2);
                defaultInterpolatedStringHandler.AppendLiteral("Timed out after ");
                defaultInterpolatedStringHandler.AppendFormatted(timeOutSeconds);
                defaultInterpolatedStringHandler.AppendLiteral(" seconds, while waiting for ");
                defaultInterpolatedStringHandler.AppendFormatted(locator);
                defaultInterpolatedStringHandler.AppendLiteral(" to NOT be Visible!");
                throw new WebDriverTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
            }
            finally
            {
                SetImplicitWaitToDefaultValue();
            }
        }

        public void WaitElementToBeClickable(IWebElement element, int timeOutSeconds = 12)
        {
            try
            {
                GetWebDriverWait(timeOutSeconds).Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (WebDriverTimeoutException innerException)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 1);
                defaultInterpolatedStringHandler.AppendLiteral("Timed out after ");
                defaultInterpolatedStringHandler.AppendFormatted(timeOutSeconds);
                defaultInterpolatedStringHandler.AppendLiteral(" seconds, while waiting for WebElement to be Clicable!");
                throw new WebDriverTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
            }
        }

        public void WaitElementToBeClickable(By locator, int timeOutSeconds = 12)
        {
            try
            {
                SetImplicitWaitToCustomValue(DelayType.ShortDelay);
                GetWebDriverWait(timeOutSeconds).Until(ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (WebDriverTimeoutException innerException)
            {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(60, 2);
                defaultInterpolatedStringHandler.AppendLiteral("Timed out after ");
                defaultInterpolatedStringHandler.AppendFormatted(timeOutSeconds);
                defaultInterpolatedStringHandler.AppendLiteral(" seconds, while waiting for ");
                defaultInterpolatedStringHandler.AppendFormatted(locator);
                defaultInterpolatedStringHandler.AppendLiteral(" to be Clicable!");
                throw new WebDriverTimeoutException(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
            }
            finally
            {
                SetImplicitWaitToDefaultValue();
            }
        }

        public static void HardcodedWait(DelayType delayType)
        {
            Thread.Sleep(TimeSpan.FromSeconds((double)delayType));
        }
        #endregion

        #region JavaScript
        //
        // Summary:
        //     Scroll to Element
        //
        // Parameters:
        //   element:
        //     WebElement
        public void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView();", element);
            if (!IsElementInViewport(element))
            {
                ((IJavaScriptExecutor)GetWebDriver()).ExecuteScript("window.scrollTo(document.body.scrollHeight, 0)", element);
                ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView(false)", element);
                if (!IsElementInViewport(element))
                {
                    ((IJavaScriptExecutor)GetWebDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)", element);
                    ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView(true)", element);
                }

                if (!IsElementInViewport(element))
                {
                    ((IJavaScriptExecutor)GetWebDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)", element);
                    ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'})", element);
                }
            }
        }

        public void ScrollToElement(By locator)
        {
            ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView();", GetElement(locator));
            if (!IsElementInViewport(GetElement(locator)))
            {
                ((IJavaScriptExecutor)GetWebDriver()).ExecuteScript("window.scrollTo(document.body.scrollHeight, 0)", GetElement(locator));
                ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView(false)", GetElement(locator));
                if (!IsElementInViewport(GetElement(locator)))
                {
                    ((IJavaScriptExecutor)GetWebDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)", GetElement(locator));
                    ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView(true)", GetElement(locator));
                }

                if (!IsElementInViewport(GetElement(locator)))
                {
                    ((IJavaScriptExecutor)GetWebDriver()).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)", GetElement(locator));
                    ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'})", GetElement(locator));
                }
            }
        }
        private bool IsElementInViewport(By locator)
        {
            return IsElementInViewport(GetElement(locator));
        }

        private bool IsElementInViewport(IWebElement element)
        {
            return (bool)((IJavaScriptExecutor)_webDriver).ExecuteScript("var elem = arguments[0],                   box = elem.getBoundingClientRect(),      cx = box.left + box.width / 2,           cy = box.top + box.height / 2,           e = document.elementFromPoint(cx, cy); for (; e; e = e.parentElement) {           if (e === elem)                            return true;                         }                                        return false;                            ", element);
        }

        private void ClickWithJS(IWebElement element)
        {
            try
            {
                ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].click();", element);
            }
            catch
            {
                HardcodedWait(DelayType.ShortDelay);
                ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].click();", element);
            }
        }

        private void ClickWithJS(By locator)
        {
            try
            {
                ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].click();", GetElement(locator));
            }
            catch
            {
                HardcodedWait(DelayType.ShortDelay);
                ((IJavaScriptExecutor)_webDriver).ExecuteScript("arguments[0].click();", GetElement(locator));
            }
        }
        #endregion
    }
}
