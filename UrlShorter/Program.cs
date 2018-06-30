using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace UrlShorter
{
    class Program
    {   
        static void Main(string[] args)
        {
            var username = "slavche001";
            var apiKey = "R_82581e4222df41e3859b3262f1d2d3f0";
            var longUrl = "https://github.com/SlavcheProger/urlShorter/tree/master/UrlShorter";
            var shortUrl = CreateShortUrl(username, apiKey, longUrl).Result;

            Console.WriteLine(shortUrl);
            Console.ReadKey();
        }
        public static async Task<String> CreateShortUrl(string username, string apiKey, string longUrl)
        {
            var xmlDoc = new XmlDocument();
            var request = WebRequest.Create("http://api.bitly.com/v3/shorten");
            var data = Encoding.UTF8.GetBytes(string.Format("login={0}&apiKey={1}&longUrl={2}&format={3}",
                username, apiKey, HttpUtility.UrlEncode(longUrl), "xml"));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = await request.GetRequestStreamAsync())
            {
                await stream.WriteAsync(data, 0, data.Length);
            }
            using (var response = await request.GetResponseAsync())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    xmlDoc.LoadXml(await reader.ReadToEndAsync());
                }
            }

            return xmlDoc.GetElementsByTagName("url")[0].InnerText;
        }
    }
}