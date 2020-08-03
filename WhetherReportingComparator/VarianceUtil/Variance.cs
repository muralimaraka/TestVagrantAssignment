using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace WhetherReportingComparator.Common
{
    public class Variance
    {
        private static string GetValueFromDataXML(string Element)
        {
            var xml = XDocument.Load(Extent.inputXMLFilePath + "Variance.xml");
            var obj = from c in xml.Descendants(Element.ToString())
                      select c.Value;
            return obj.FirstOrDefault();
        }

        public Dictionary<string, string> GetExpectedVarinceFromXML()
        {
            try {
                var ExpectedVarince = new Dictionary<string, string>()
            {
                {"tempInCelsius" , Variance.GetValueFromDataXML("tempInCelsiusvarince") },
                {"tempInFH", Variance.GetValueFromDataXML("tempInFHvarince") },
                {"wind", Variance.GetValueFromDataXML("windvarince") },
                {"humidity", Variance.GetValueFromDataXML("humidityvarince") },
                {"cityName",Variance.GetValueFromDataXML("cityNamevarince") }
            };

                return ExpectedVarince;
            }
            catch (Exception ex)
            {
                Logger.LogMessage(ex.Message);
                Extent.test.Log(Status.Fail, ex.Message);
                Assert.Fail(ex.Message);
                return null;
            }
        }


        public void CompareVarince(Dictionary<string, string> ActualVarince, Dictionary<string, string> ExpectedVarince,string weatherParameter)
        {
            if(weatherParameter=="tempInCelsius")
            { 
            if (Convert.ToDouble(ActualVarince["tempInCelsius"]) <= Convert.ToDouble(ExpectedVarince["tempInCelsius"]))
            {
                Logger.LogMessage("Temparature Celsius difference in UI and API is with in variance ragne  :" +
                    "Acutal Varince:" + Convert.ToDouble(ActualVarince["tempInCelsius"]) + "Expected  Varince:" + Convert.ToDouble(ExpectedVarince["tempInCelsius"]));
                Extent.test.Log(Status.Pass, "Temparature Celsius difference in UI and API is with variance ragne  :" +
                     "Acutal Varince:" + Convert.ToDouble(ActualVarince["tempInCelsius"]) + "Expected  Varince:" + Convert.ToDouble(ExpectedVarince["tempInCelsius"]));
            }
            else
            {
                    throw new Exception("TempInCelsius Mismatch");
                }
            }

            else if (weatherParameter == "tempInFH")
            {
                if (Convert.ToDouble(ActualVarince["tempInFH"]) <= Convert.ToDouble(ExpectedVarince["tempInFH"]))
                {
                    Logger.LogMessage("Temparature FH difference in UI and API is with in variance ragne  :" +
                        "Acutal Varince:" + Convert.ToDouble(ActualVarince["tempInFH"]) + "Expected  Varince:" + Convert.ToDouble(ExpectedVarince["tempInFH"]));
                    Extent.test.Log(Status.Pass, "Temparature FH difference in UI and API is with variance ragne  :" +
                         "Acutal Varince:" + Convert.ToDouble(ActualVarince["tempInFH"]) + "Expected  Varince:" + Convert.ToDouble(ExpectedVarince["tempInFH"]));
                }
                else
                {
                    throw new Exception("TempInFH Mismatch");
                }
            }

            else if(weatherParameter == "wind")
            {
                if (Convert.ToDouble(ActualVarince["wind"]) <= Convert.ToDouble(ExpectedVarince["wind"]))
                {
                    Logger.LogMessage("Wind Speed difference in UI and API is with in variance ragne  :" +
                        "Acutal Varince:" + Convert.ToDouble(ActualVarince["wind"]) + "Expected  Varince:" + Convert.ToDouble(ExpectedVarince["wind"]));
                    Extent.test.Log(Status.Pass, "Wind Speed difference in UI and API is with variance ragne  :" +
                         "Acutal Varince:" + Convert.ToDouble(ActualVarince["wind"]) + "Expected  Varince:" + Convert.ToDouble(ExpectedVarince["wind"]));
                }
                else
                {
                    throw new Exception("Wind Speed Mismatch");
                }
            }

            else if (weatherParameter == "humidity")
            {
                if (Convert.ToInt32(ActualVarince["humidity"]) <= Convert.ToInt32(ExpectedVarince["humidity"]))
                {
                    Logger.LogMessage("humidity difference in UI and API is with in variance ragne  :" +
                        "Acutal Varince:" + Convert.ToInt32(ActualVarince["humidity"]) + "Expected  Varince:" + Convert.ToInt32(ExpectedVarince["humidity"]));
                    Extent.test.Log(Status.Pass, "humidity difference in UI and API is with variance ragne  :" +
                         "Acutal Varince:" + Convert.ToInt32(ActualVarince["humidity"]) + "Expected  Varince:" + Convert.ToInt32(ExpectedVarince["humidity"]));
                }
                else
                {
                    throw new Exception("Humidity Mismatch");
                }
            }

            else if (weatherParameter == "cityName")
            {
                if (Convert.ToInt32(ActualVarince["cityName"]) <= Convert.ToInt32(ExpectedVarince["cityName"]))
                {
                    Logger.LogMessage("CitName in UI and API is matched");
                    Extent.test.Log(Status.Pass, "CitName in UI and API is matched");
                }
                else
                {
                    throw new Exception("CityName Mismatch");
                }
            }

        }

    }
}
