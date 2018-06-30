﻿using System;
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
            var longUrl = "https://www.youtube.com/results?search_query=async+await+c%23";

            var shortUrl = ShortUrlGenerator(username, apiKey, longUrl).Result;

            Console.WriteLine(shortUrl);
            Console.ReadKey();
        }

        public static async Task<string> ShortUrlGenerator(string username, string apiKey, string longUrl)
        {
            return await Task.FromResult<string>(CreateShortUrl(username, apiKey, longUrl));
        }

        public static string CreateShortUrl(string username, string apiKey, string longUrl)
        {
            XmlDocument xmlDoc = new XmlDocument();

            WebRequest request = WebRequest.Create("http://api.bitly.com/v3/shorten");
            byte[] data = Encoding.UTF8.GetBytes(string.Format("login={0}&apiKey={1}&longUrl={2}&format={3}",
                username, apiKey, HttpUtility.UrlEncode(longUrl), "xml"));

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    xmlDoc.LoadXml(reader.ReadToEnd());
                }
            }

            return xmlDoc.GetElementsByTagName("url")[0].InnerText;
        }
    }
}