using System;
using System.Collections.Generic;
using System.Reflection;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium;
using WhetherReportingComparator.Common;
using WhetherReportingComparator.Utilities;

namespace WhetherReportingComparator.Pages
{
    [TestClass]
    public class WeatherPage
    {

        private IWebDriver webDriver;
        public WeatherPage(IWebDriver WebDriver)
        {
            webDriver = WebDriver;
        }

        # region Properties
        private By txbPinYourCity { get { return By.Id("searchBox"); } }
        private By divCityList { get { return By.XPath("//div[@class='cityText']"); } }
        private By wetherDetailsContainer { get { return By.XPath("//div[@class='leaflet-popup-content-wrapper']"); } }
        #endregion Properties

        #region PageObject Methdos
        public void EnterPinYourCity(string CityName="Bengaluru")
        {
            try 
            {
                SelenimHelpers.SendKeys(webDriver, txbPinYourCity, CityName);
                Logger.LogMessage("Enter City Name to Pin your city " + CityName);
                Extent.test.Log(Status.Pass, "Enter City Name to Pin your city "+ CityName);
            }
            catch(Exception ex)
            {
                Logger.LogMessage("Unable to Enter City Name to Pin your city " + ex.Message);
                Extent.test.Log(Status.Fail, "Unable to Enter City Name to Pin your city " + ex.Message);
                Assert.Fail(ex.Message);
            }
            
        }

        public void VeriyMapRevealsWetherDetails(string CityName = "Bengaluru")
        {
            bool WetherDetailsContainer = false;
            try
            {
                IWebElement elementWetherDetailsContainer = SelenimHelpers.FindElement(webDriver,wetherDetailsContainer, 60);
                WetherDetailsContainer = elementWetherDetailsContainer.Displayed;

                if (WetherDetailsContainer)
                {
                    Logger.LogMessage("weather details are displayed on map as expected");
                    Extent.test.Log(Status.Pass, "weather details are displayed on map as expected");
                }
                else
                {
                   throw new Exception("weather details are not displayed on map");
                }
            }
            catch (Exception ex)
            {
                Extent.test.Log(Status.Fail, ex.Message);
                Assert.Fail(ex.Message);
            }
        }

        public WeatherInfoUI FetchWeatherDetailsForSelectedCity(string CityName)
        {
            var objWeatherInfoUI = new WeatherInfoUI();
            try
            {
                IList<IWebElement> divWeatherInfo = SelenimHelpers.FindElements(webDriver, By.XPath("//div[@class='leaflet-popup-content-wrapper']//span"), 30);

                foreach (IWebElement element in divWeatherInfo)
                {
                    string weatherText = element.Text;

                    if (!string.IsNullOrEmpty(weatherText))
                    {
                        if (weatherText.Contains("Wind"))
                        {
                            string[] tempWind = weatherText.Split(':');
                            string Wnd = tempWind[1].Substring(0, tempWind[1].IndexOf("KPH"));
                            objWeatherInfoUI.wind = Convert.ToDouble(Wnd.Trim());
                        }

                        if (weatherText.Contains("Humidity"))
                        {
                            string[] tempHumidity = weatherText.Split(':');
                            string Hmdty = tempHumidity[1].Substring(0, tempHumidity[1].LastIndexOf("%"));
                            objWeatherInfoUI.humidity = Convert.ToInt32(Hmdty.Trim());
                        }
                        if (weatherText.Contains("Temp in Degrees"))
                        {
                            string[] tempDegree = weatherText.Split(':');
                            objWeatherInfoUI.tempInCelsius = Convert.ToDouble(tempDegree[1].Trim());
                        }

                        if (weatherText.Contains("Temp in Fahrenheit"))
                        {
                            string[] tempFH = weatherText.Split(':');
                            objWeatherInfoUI.tempInFH = Convert.ToDouble(tempFH[1].Trim());
                        }

                        if (weatherText.Contains(CityName))
                        {
                            string[] cityNames = weatherText.Split(',');
                            objWeatherInfoUI.cityName = cityNames[0];
                        }
                    }
                }
                Extent.test.Log(Status.Pass, "Fetching weather info on Map from UI displayed below");
                Logger.LogMessage("Fetching weather info on Map from UI displayed below");
                new CommonMethods().PrintObjectInfo(objWeatherInfoUI);
                return objWeatherInfoUI;
            }
            catch (Exception ex)
            {
                Extent.test.Log(Status.Fail, "Unable to fetch weather info on Map" + ex.Message);
                Logger.LogMessage("Unable to fetch weather info on Map" + ex.Message);
                Assert.Fail(ex.Message);
                return null;
            }
        }

        public void VerifyAndClickOnCityDisplayedOnMap(string CityName = "Bengaluru")
        {
            bool CityExist = false;
            try
            {
                List<IWebElement> divCityListItem=SelenimHelpers.FindElements(webDriver, divCityList);
                foreach (IWebElement city in divCityListItem)
                {
                    if (city.Text.Contains(CityName))
                    {
                        CityExist = true;
                        city.Click();
                        break;
                    }
                }
                if (CityExist)
                { 
                    Extent.test.Log(Status.Pass, "Selected City " + CityName + "available on MAP");
                    Logger.LogMessage("Selected City " + CityName + "available on MAP");
                }
                else
                {
                    throw new Exception("Selected City " + CityName + "is not available on MAP");
                }
            }
            catch (Exception ex)
            {
                Extent.test.Log(Status.Fail, ex.Message);
                Logger.LogMessage(ex.Message);
                Assert.Fail(ex.Message);
            }

        }
        #endregion PageObject Methdos
    }

    public class WeatherInfoUI
    {
        public double tempInCelsius { get; set; }
        public double tempInFH { get; set; }
        public double wind { get; set; }
        public int humidity { get; set; }
        public string cityName { get; set; }
    }
}
