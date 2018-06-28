using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace UrlShorter
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "slavche001";
            string apiKey = "R_82581e4222df41e3859b3262f1d2d3f0";
            string longUrl = "https://www.youtube.com/results?search_query=bitly+api+key+c%23";
            string shortUrl = CreateShortUrl(username, apiKey, longUrl);

            Console.WriteLine(shortUrl);
            Console.ReadKey();
        }

        public static string CreateShortUrl(string username, string apiKey, string longUrl)
        {
            string statusCode = string.Empty;
            string statusText = string.Empty;
            string shortUrl = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();

            WebRequest request = WebRequest.Create("http://api.bitly.com/v3/shorten");
            byte[] data = Encoding.UTF8.GetBytes(string.Format("login={0}&apiKey={1}&longUrl={2}&format={3}",
                username, apiKey, HttpUtility.UrlEncode(longUrl), "xml"));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream ds = request.GetRequestStream())
            {
                ds.Write(data, 0, data.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    xmlDoc.LoadXml(sr.ReadToEnd());
                }
            }

            shortUrl = xmlDoc.GetElementsByTagName("url")[0].InnerText;

            return shortUrl;
        }
    }
}