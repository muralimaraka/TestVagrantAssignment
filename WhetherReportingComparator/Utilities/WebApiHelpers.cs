using System;
using System.IO;
using System.Net;

namespace WhetherReportingComparator.Utilities
{
    public class WebApiHelpers
    {
        public static void CreateWebRequest(string baseUrl, string methodUrl, string method,string jsonPayload, out string result, out HttpStatusCode statusCode, string contentType = "application/x-www-form-urlencoded", string headers = null, string userName = null, string password = null, string specialHeaders = null)
        {
            var httpWebRequest = InitializeRequest(baseUrl, methodUrl, method, contentType, userName, password, headers, specialHeaders);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonPayload);
                streamWriter.Flush();
                streamWriter.Close();
                GetWebResponse(out result, out statusCode, httpWebRequest);
            }
        }       

        private static HttpWebRequest InitializeRequest(string baseUrl, string methodUrl, string method, string contentType, string userName = null, string password = null, string headers = null, string SpecialHeaders = null)
        {
            var httpWebRequest =
               (HttpWebRequest)WebRequest.Create(baseUrl + methodUrl);

            httpWebRequest.Method = method;
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Timeout = 100000;

            if (!string.IsNullOrEmpty(SpecialHeaders))
            {
                char tempChar = ',';
                var temp = SpecialHeaders.Split(tempChar);
                if (temp.GetLength(0) > 0)
                {
                    httpWebRequest.Headers.Add(temp[0], temp[1]);
                }
            }
                try
                {
                    httpWebRequest.Headers.Add("x-api-key", headers);
                }
                catch { }

                return httpWebRequest;
        }

        private static void GetWebResponse(out string result, out HttpStatusCode statusCode, HttpWebRequest httpWebRequest)
        {
            statusCode = HttpStatusCode.NotFound;

            try
            {
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                statusCode = httpWebResponse.StatusCode;
                var stream = httpWebResponse.GetResponseStream();
                if (stream == null)
                {
                    result = string.Empty;
                    return;
                }
                using (var streamReader = new StreamReader(stream))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                statusCode = HttpStatusCode.RequestTimeout;
                result = string.Empty;
                if (ex.Message.Split(')').Length > 1)
                {
                    if (ex.Message.Split(')')[1].Trim().ToLower().Replace(" ", "").Replace(".", "") == HttpStatusCode.NotImplemented.ToString().ToLower())
                        statusCode = HttpStatusCode.NotImplemented;
                    if (ex.Message.Split(')')[1].Trim().ToLower().Replace(" ", "").Replace(".", "") == HttpStatusCode.BadRequest.ToString().ToLower())
                        statusCode = HttpStatusCode.BadRequest;
                    if (ex.Message.Split(')')[1].Trim().ToLower().Replace(" ", "").Replace(".", "") == HttpStatusCode.InternalServerError.ToString().ToLower())
                        statusCode = HttpStatusCode.InternalServerError;

                    using (var stream = ex.Response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            result = string.Empty;
                            statusCode = HttpStatusCode.NotFound;
                            return;
                        }
                        using (var reader = new StreamReader(stream))
                        {
                            result = reader.ReadToEnd();

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

        }     

    }
}
