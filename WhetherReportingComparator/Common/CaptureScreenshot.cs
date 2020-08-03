using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium;

namespace WhetherReportingComparator.Common
{
    public static class CaptureScreenshot
    {
        public static void TakeScreenshot(this IWebDriver webDriver, string filePath)
        {
            try
            {
                Screenshot screenShot = ((ITakesScreenshot)webDriver).GetScreenshot();
                screenShot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
            }
            catch (Exception e)
            {
                Logger.LogMessage(e.Message);
            }
        }
    }
}
