using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WhetherReportingComparator.Utilities
{
    public static class SelenimHelpers
    {
        public static void PageSynchronization(this IWebDriver webDriver)
        {
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(120)).Until(
                    d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception e) { Console.WriteLine("Handled " + e + " error"); }
        }

        public static void Click(this IWebDriver driver, By webElement)
        {
            PageSynchronization(driver);
            IWebElement element = driver.FindElement(webElement, 30);
            driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            driver.ExecuteScript("arguments[0].click();", element);
        }

        public static void SendKeys(this IWebDriver driver, By webElement, string text)
        {
            PageSynchronization(driver);
            IWebElement element = driver.FindElement(webElement, 30);
            element.Clear();
            element.SendKeys(text);
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds = 60)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            var elem = fluentWait.Until(drv => drv.FindElement(by));
            if (!elem.Displayed)
                return null;
            return elem;
        }

        public static List<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds = 60)
        {
            DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(1000);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            List<IWebElement> elem = fluentWait.Until(drv => drv.FindElements(by).ToList());
            return elem;
        }

        public static object ExecuteScript(this IWebDriver webDriver, string script, params object[] args)
        {
            return webDriver.GetJsExecutor().ExecuteScript(script, args);
        }

        private static IJavaScriptExecutor GetJsExecutor(this IWebDriver webDriver)
        {
            return (IJavaScriptExecutor)webDriver;
        }
    }
}
