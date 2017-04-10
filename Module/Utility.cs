using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace M1sakaBot.Module {
    public class Utility {
        public String ShortLink(String long_link) {
            String req_url = "http://apis.baidu.com/3023/shorturl/shorten?url_long=";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(req_url + long_link);
            request.Headers.Add("apikey", "83a0fc03038060891b12cdcc7bd8d695");
            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream,System.Text.Encoding.GetEncoding("utf-8"));
            String content = reader.ReadToEnd();
            Dictionary<String, Dictionary<String, String>[]> dt = JsonConvert.DeserializeObject<Dictionary<String, Dictionary<String, String>[]>>(content);
            String short_link=dt["urls"][0]["url_short"];
            return short_link;
        }
        public String WeatherInfo(String cityName) {
            String info = "";
            String req_url = "http://apis.baidu.com/apistore/weatherservice/cityname?cityname=";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(req_url + cityName);
            request.Headers.Add("apikey", "83a0fc03038060891b12cdcc7bd8d695");
            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
            String content = reader.ReadToEnd();
            //Console.WriteLine(content);
            Dictionary<String,Dictionary<String,String>> dt = JsonConvert.DeserializeObject<Dictionary<String, Dictionary<String, String>>>(content);
            Console.WriteLine(dt["retData"]["weather"]);
            Console.WriteLine(dt["retData"]["temp"]);
            info+=(cityName+":"+dt["retData"]["weather"]+","+dt["retData"]["temp"]);
            return info;
        }
        //public String Googleit(String search_content) {

        //}
        public int RandomInt(int low=0,int high=1000000000) {
            if (high >= low) {
                Random rnd = new Random();
                return rnd.Next(low,high);
            }
            else {
                return -1;
            }
        }
        public double RandomDouble(int low=0,int high=1000000000) {
            if (low <= high) {
                Random rnd = new Random();
                return rnd.Next(low, high) + rnd.NextDouble();
            }
            else {
                return -1.0;
            }
        }
        //public String RandomString() {
        //    String content = "";

        //    return content;
        //}
        public String SHA1_Process(String source) {
            byte[] StrRes = Encoding.Default.GetBytes(source);
            HashAlgorithm sha1 = new SHA1CryptoServiceProvider();
            StrRes = sha1.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach(byte b in StrRes) {
                EnText.AppendFormat("{0:x2}", b);
            }
            return EnText.ToString();
        }
    }
}
