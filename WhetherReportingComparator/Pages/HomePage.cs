using System;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium;
using WhetherReportingComparator.Common;
using WhetherReportingComparator.Utilities;

namespace WhetherReportingComparator.Pages
{
    [TestClass]
    public class HomePage
    {

        private IWebDriver webDriver;
        public HomePage(IWebDriver WebDriver)
        {
            webDriver = WebDriver;
        }

        # region Properties
        private By linkSubMenu { get { return By.XPath("//a[@id='h_sub_menu']"); } }
        private By linkWhether { get { return By.XPath("//a[text()='WEATHER']"); } }
        #endregion Properties

        #region PageObject Methdos
        public void ClickSubmenuInHomePage()
        {
            try 
            {
                SelenimHelpers.Click(webDriver, linkSubMenu);
                Logger.LogMessage("Clicked on Submenu in Home Page");
                Extent.test.Log(Status.Pass, "Clicked on Submenu in Home Page");
            }
            catch(Exception e)
            {
                Logger.LogMessage("Unable to Click on SubMenu "+ e.Message);
                Extent.test.Log(Status.Fail, "Unable to Click on SubMenu " + e.Message);
                Assert.Fail(e.Message);
            }
        }

        public void ClickOnWetherInHomePage()
        {
            try
            {
                SelenimHelpers.Click(webDriver, linkWhether);
                Logger.LogMessage("Clicked on Weather in Home Page");
                Extent.test.Log(Status.Pass, "Clicked on Weather Link in Home Page");
            }
            catch (Exception e)
            {
                Extent.test.Log(Status.Fail, "Unable to Click on Weather link " + e.Message);
                Logger.LogMessage("Unable to Click on Weather link"+ e.Message);
                Assert.Fail(e.Message);
            }
        }
        #endregion PageObject Methdos
    }
}
