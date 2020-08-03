using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using WhetherReportingComparator.Common;
using WhetherReportingComparator.Utilities;
using WhetherReportingComparator.Utilities.JsonLibraries;

namespace WhetherReportingComparator.ApplicationSpecificLibrary
{
    class WeatherInfoHelpers
    {
        string baseURI = ConfigurationManager.AppSettings["ApiUrl"];
        string apiKey = ConfigurationManager.AppSettings["ApiKey"];
        string cityNameEndPoint= "q={0}";
        string cityByIdEndPoint = "id={0}";
        string cityByZipCodeEndPoint = "zip={0},{1}";

        public WeatherInfoAPI GetWeatherInfoAPI(Enum InputType,string cityName=null,string cityID= null,string zipCode= null, string countryCode = null)
        {
            try
            {
                var lst = new List<double>();
                var objWeatherInfoAPI = new WeatherInfoAPI();
                
                string method = "POST";
                string methodUrl = string.Empty;
                switch (InputType)
                {
                    case RequestInputType.CityName:
                        {
                            methodUrl = string.Format(cityNameEndPoint, cityName);
                            break;
                        }
                    case RequestInputType.CityID:
                        {
                            methodUrl = string.Format(cityByIdEndPoint, cityID);
                            break;
                        }
                    case RequestInputType.ZipCode:
                        {
                            methodUrl = string.Format(cityByZipCodeEndPoint, zipCode, countryCode);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Wrong Input type" + InputType.ToString());
                            Extent.test.Log(Status.Fail, "Wrong Input type" + InputType.ToString());
                            Assert.Fail("Wrong Input type" + InputType.ToString());
                            break;
                        }
                }
                //string City = string.Format("id={0}", "1264527");
                //string City = string.Format("id={0}", "1277333");

                WebApiHelpers.CreateWebRequest(baseURI, methodUrl, method, null, out string response, out HttpStatusCode statusCode, "application/json", headers: apiKey);
                var jsonResponse = new JsonLibrary();
                if (statusCode != HttpStatusCode.OK)
                    Assert.IsTrue(statusCode == HttpStatusCode.OK, statusCode.ToString());
                    Console.WriteLine("Response from API :" + response);
                    Extent.test.Log(Status.Pass, "Response from API :" + response);

                //Get json token from json response
                string temp = jsonResponse.GetJsonToken(response, "$..main.temp");
                lst = ConvertKelvinToDegrees(temp);
                objWeatherInfoAPI.tempInCelsius = lst[0];
                objWeatherInfoAPI.tempInFH = lst[1];

                string speedFrmAPI = jsonResponse.GetJsonToken(response, "$..wind.speed");
                double speedFrmAPIAfterConversion = ConvertSpeedToKMPerSec(speedFrmAPI);
                objWeatherInfoAPI.wind = speedFrmAPIAfterConversion;

                objWeatherInfoAPI.humidity = Convert.ToInt32(jsonResponse.GetJsonToken(response, "$..main.humidity"));
                objWeatherInfoAPI.cityName = jsonResponse.GetJsonToken(response, "$..name");
                new CommonMethods().PrintObjectInfo(objWeatherInfoAPI);
                return objWeatherInfoAPI;
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Failed to get respose from API" + ex.Message);
                Extent.test.Log(Status.Fail, "Failed to get respose from API" + ex.Message);
                Assert.Fail(ex.Message);
                return null;
            }
        }

        public List<double> ConvertKelvinToDegrees(string Temp)
        {
            try {
                var lst = new List<double>();
                double _tempDouble = Convert.ToDouble(Temp);
                double tempInCelsius = (_tempDouble - 273.15);
                double tempInFH = (tempInCelsius) * 9 / 5 + 32;
                lst.Add(tempInCelsius);
                lst.Add(tempInFH);
                return lst;
            }
            catch (Exception ex)
            {
                Assert.Fail("Converting Temperature from Kelvin to celsius and FH "+ex);
                return null;
            }
        }

        public double ConvertSpeedToKMPerSec(string speed)
        {
            try
            {
                double _tempSpeed = Convert.ToDouble(speed);
                double _tempSpeedinKM = (_tempSpeed * 1.60934);
                return _tempSpeedinKM;
            }
            catch (Exception ex)
            {
                throw new Exception("Converting Speed from ms to kmh " + ex);
            }
        }
    }

    public class WeatherInfoAPI
    {
        public double tempInCelsius { get; set; }
        public double tempInFH { get; set; }
        public double wind { get; set; }
        public int humidity { get; set; }
        public string cityName { get; set; }
    }

    public enum RequestInputType
    {
        CityName,
        CityID,
        ZipCode
    }

}
