using System;
using System.Configuration;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WhetherReportingComparator.Common;

namespace WhetherReportingComparator.Utilities
{
    [TestClass]
    public class BaseTest
    {
        public static IWebDriver TestWebDriver { get; set; }
        public static string ReportPath;
        public static ExtentReports extent;
        public static ExtentHtmlReporter htmlReporter;
        private static TestContext testContextInstance;

        [AssemblyInitialize]
        public static void AssemblyInitilize(TestContext context)
        {
            Extent.snapFileName = context.TestName;
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var actualPath = path.Substring(0, path.LastIndexOf("bin"));
            Extent.projectPath = new Uri(actualPath).LocalPath;
            ReportPath = Extent.projectPath + "ExtentReports\\ExtentReport.html";
            Extent.snappath = Extent.projectPath + "ExtentReports\\Screenshots\\";
            Extent.inputXMLFilePath = Extent.projectPath;
            var htmlReporter = new ExtentHtmlReporter(ReportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            htmlReporter.LoadConfig(Extent.projectPath + "ExtentReports\\extent-config.xml");
        }

        [TestInitialize]
        public void TestSetup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-extensions"); 
            options.AddArguments("--disable-notifications"); 
            //options.UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss;
    
            Extent.test = extent.CreateTest(TestContext.TestName);
            Logger.LogMessage("Navigating to URL: {0}", ConfigurationManager.AppSettings["Url"]);
            TestWebDriver = new ChromeDriver(Extent.projectPath + "ChromeDriver", options);
            TestWebDriver.Url = ConfigurationManager.AppSettings["Url"];
            TestWebDriver.Manage().Window.Maximize();    
            TestWebDriver.Manage().Cookies.DeleteAllCookies(); 
        }
  
        [TestCleanup]
        public void TestTearDown()
        {
            if (!(TestContext.CurrentTestOutcome == UnitTestOutcome.Passed))
            {
                string ScreenshotPath = string.Format("{0}{1}-{2}.jpg",
                Extent.snappath, TestContext.TestName, DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-ffff"));
                Logger.LogMessage("ScreenShotPath = " + ScreenshotPath);
                CaptureScreenshot.TakeScreenshot(TestWebDriver, ScreenshotPath);
                TestContext.AddResultFile(ScreenshotPath);
                Extent.test.Log(Status.Info, "Please refer the screenshot below and Screenshot path is"+ ScreenshotPath);
                Extent.test.AddScreenCaptureFromPath(ScreenshotPath,"Screenshot");
            }
            Logger.LogMessage(@"Quitting WebDriver.");
            TestWebDriver.Quit();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            extent.AddSystemInfo("Source1", "https://www.ndtv.com/");
            extent.AddSystemInfo("Source2", "https://openweathermap.org/");
            extent.Flush();
            TestWebDriver = null;
            System.Diagnostics.Process.Start("chrome", Extent.projectPath + "ExtentReports\\dashboard.html");
        }

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

    }
}
