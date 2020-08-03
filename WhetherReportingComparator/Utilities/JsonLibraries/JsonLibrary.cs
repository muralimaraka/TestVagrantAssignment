using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Newtonsoft.Json.Linq;

namespace WhetherReportingComparator.Utilities.JsonLibraries
{
    class JsonLibrary
    {
        public bool VerifyJsonProperty(string json, string jsonPath, string ExpectedTaken)
        {
            try
            {
                JObject JsonObject = JObject.Parse(json);
                JToken ActualToken = JsonObject.SelectToken(jsonPath);
                string ActualTokenString = ActualToken.ToString();
                if (ExpectedTaken.CompareTo(ActualTokenString) != 0)
                {
                    Console.WriteLine("Json Path Applied:{0}", jsonPath);
                    Console.WriteLine("Expected Token is :{0} and Actual Token:{1}", ExpectedTaken, ActualTokenString);
                    throw new Exception("Verify Json Propertry Failed");
                }
                else
                {
                    Console.WriteLine("Json Path Applied:{0}", jsonPath);
                    Console.WriteLine("Expected Token is :{0} and Actual Token:{1}", ExpectedTaken, ActualTokenString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ensure that Json path is valid,Json Path Applied:{0}", jsonPath);
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
            return true;
        }


        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var item in json.Children())
            {
                yield return item;
                foreach (var childItem in AllChildren(item))
                {
                    yield return childItem;
                }
            }
        }
        public string GetJsonToken(string json, string jsonPath)
        {
            try
            {
                JObject JsonObject = JObject.Parse(json);
                JToken Token = JsonObject.SelectToken(jsonPath);
                string TokenString = Token.ToString();
                if (string.IsNullOrEmpty(TokenString))
                {
                    throw new Exception("Verify Json Propertry Failed");
                }
                else
                {
                    return TokenString;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Ensure that Json and Json path is valid Json String:{0},Json Path Applied:{1}", json, jsonPath);
                return null;
            }
        }
        public List<string> GetJsonTokens(string json, List<string> jsonPath)
        {
            var retList = new List<string>();
            Label loop;
            int count = 0;
            try
            {
                JObject JsonObject = JObject.Parse(json);
                foreach (string token in jsonPath)
                {
                    JToken Token = JsonObject.SelectToken(token);
                    string TokenString = Token.ToString();
                    if (string.IsNullOrEmpty(TokenString))
                    {
                      
                        retList.Add("null");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("Json String:{0}", json);
                        Console.WriteLine("Json Path Applied:{0}", jsonPath);
                        Console.WriteLine("Retrived Token for Jpath:{0} is :{1}", jsonPath[count], TokenString);
                        retList.Add(TokenString);
                        count++;
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ensure that Json and Json path is valid Json String:{0},Json Path Applied:{1}", json, jsonPath);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

    }
}
