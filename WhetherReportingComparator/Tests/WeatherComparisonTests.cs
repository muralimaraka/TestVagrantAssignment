using System;
using System.Collections.Generic;
using System.Data;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using WhetherReportingComparator.ApplicationSpecificLibrary;
using WhetherReportingComparator.Common;
using WhetherReportingComparator.ComparatorUtility;
using WhetherReportingComparator.Pages;
using WhetherReportingComparator.Utilities;

namespace WhetherReportingComparator
{

    [TestClass]
    public class WeatherComparisonTests : BaseTest
    {
        [DataRow("Kolkata")]
        [DataRow("Lucknow")]
        [DataRow("Chennai")]
        [DataRow("Bengaluru")]
        [DataRow("New Delhi")]
        [DataRow("Patna")]
        [DataRow("Bhopal")]
        [DataRow("Mumbai")]
        [DataRow("Hyderabad")]
        [TestMethod]
        public void WeatherComparisionByCityName(string SelectCity)
        {
            var objWeatherInfoUI = new WeatherInfoUI();
            var objWeatherInfoAPI = new WeatherInfoAPI();
            var actualVariance = new Dictionary<string, string>();
            var expectedVariance = new Dictionary<string, string>();
            var varince = new Variance();
            try
            {
                var objHomePage = new HomePage(BaseTest.TestWebDriver);
                objHomePage.ClickSubmenuInHomePage();
                objHomePage.ClickOnWetherInHomePage();

                var objWeatherPage = new WeatherPage(BaseTest.TestWebDriver);
                objWeatherPage.EnterPinYourCity(SelectCity);
                objWeatherPage.VerifyAndClickOnCityDisplayedOnMap(SelectCity);
                objWeatherPage.VeriyMapRevealsWetherDetails(SelectCity);
                objWeatherInfoUI = objWeatherPage.FetchWeatherDetailsForSelectedCity(SelectCity);
                objWeatherInfoAPI = new WeatherInfoHelpers().GetWeatherInfoAPI(RequestInputType.CityName,SelectCity);
                expectedVariance = varince.GetExpectedVarinceFromXML();
                actualVariance = new Comparator().Compare(objWeatherInfoUI, objWeatherInfoAPI);
                varince.CompareVarince(actualVariance, expectedVariance, "tempInCelsius");
                varince.CompareVarince(actualVariance, expectedVariance, "tempInFH");
                varince.CompareVarince(actualVariance, expectedVariance, "wind");
                varince.CompareVarince(actualVariance, expectedVariance, "humidity");
                varince.CompareVarince(actualVariance, expectedVariance, "cityName");
            }
            catch (Exception ex)
            {
                Extent.test.Log(Status.Fail, "Test case Failed due to following Error "+ex.Message);
                Logger.LogMessage("Test case Failed due to following Error" + ex.Message);
                Assert.Fail(ex.Message);
            }
        }

       
        [DataRow("Bengaluru", "1277333")]
        [DataRow("Mumbai", "1275339")]
        [TestMethod]
        public void WeatherComparisionByCityID(string SelectCity,string CityID)
        {
            var objWeatherInfoUI = new WeatherInfoUI();
            var objWeatherInfoAPI = new WeatherInfoAPI();
            var actualVariance = new Dictionary<string, string>();
            var expectedVariance = new Dictionary<string, string>();
            var varince = new Variance();
            try
            {
                var objHomePage = new HomePage(BaseTest.TestWebDriver);
                objHomePage.ClickSubmenuInHomePage();
                objHomePage.ClickOnWetherInHomePage();

                var objWeatherPage = new WeatherPage(BaseTest.TestWebDriver);
                objWeatherPage.EnterPinYourCity(SelectCity);
                objWeatherPage.VerifyAndClickOnCityDisplayedOnMap(SelectCity);
                objWeatherPage.VeriyMapRevealsWetherDetails(SelectCity);
                objWeatherInfoUI = objWeatherPage.FetchWeatherDetailsForSelectedCity(SelectCity);
                objWeatherInfoAPI = new WeatherInfoHelpers().GetWeatherInfoAPI(RequestInputType.CityID,cityID: CityID);
                expectedVariance = varince.GetExpectedVarinceFromXML();
                actualVariance = new Comparator().Compare(objWeatherInfoUI, objWeatherInfoAPI);
                varince.CompareVarince(actualVariance, expectedVariance, "tempInCelsius");
                varince.CompareVarince(actualVariance, expectedVariance, "tempInFH");
                varince.CompareVarince(actualVariance, expectedVariance, "wind");
                varince.CompareVarince(actualVariance, expectedVariance, "humidity");
                varince.CompareVarince(actualVariance, expectedVariance, "cityName");
            }
            catch (Exception ex)
            {
                Extent.test.Log(Status.Fail, "Test case Failed due to following Error " + ex.Message);
                Logger.LogMessage("Test case Failed due to following Error" + ex.Message);
                Logger.LogMessage("Test case Failed due to following Error" + ex.StackTrace);
                Assert.Fail(ex.Message);
            }
        }

        [DataRow("Bengaluru", "560004", "IN")]
        [TestMethod]
        public void WeatherComparisionByZipCode(string SelectCity, string ZipCode,string CountryCode)
        {
            var objWeatherInfoUI = new WeatherInfoUI();
            var objWeatherInfoAPI = new WeatherInfoAPI();
            var actualVariance = new Dictionary<string, string>();
            var expectedVariance = new Dictionary<string, string>();
            var varince = new Variance();
            try
            {
                var objHomePage = new HomePage(BaseTest.TestWebDriver);
                objHomePage.ClickSubmenuInHomePage();
                objHomePage.ClickOnWetherInHomePage();

                var objWeatherPage = new WeatherPage(BaseTest.TestWebDriver);
                objWeatherPage.EnterPinYourCity(SelectCity);
                objWeatherPage.VerifyAndClickOnCityDisplayedOnMap(SelectCity);
                objWeatherPage.VeriyMapRevealsWetherDetails(SelectCity);
                objWeatherInfoUI = objWeatherPage.FetchWeatherDetailsForSelectedCity(SelectCity);
                objWeatherInfoAPI = new WeatherInfoHelpers().GetWeatherInfoAPI(RequestInputType.ZipCode,zipCode:ZipCode,countryCode: CountryCode);
                expectedVariance = varince.GetExpectedVarinceFromXML();
                actualVariance = new Comparator().Compare(objWeatherInfoUI, objWeatherInfoAPI);
                varince.CompareVarince(actualVariance, expectedVariance, "tempInCelsius");
                varince.CompareVarince(actualVariance, expectedVariance, "tempInFH");
                varince.CompareVarince(actualVariance, expectedVariance, "wind");
                varince.CompareVarince(actualVariance, expectedVariance, "humidity");
            }
            catch (Exception ex)
            {
                Extent.test.Log(Status.Fail, "Test case Failed due to following Error " + ex.Message);
                Logger.LogMessage("Test case Failed due to following Error" + ex.Message);
                Assert.Fail(ex.Message);
            }
        }

    }
}