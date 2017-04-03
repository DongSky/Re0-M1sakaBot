using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Data;

namespace M1sakaBot.Module {
    public class Utility {
        public String ShortLink(String long_link) {
            String req_url = "http://apis.baidu.com/3023/shorturl/shorten?url_long=";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(req_url + long_link);
            request.Headers.Add("apikey", "your key");
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
            request.Headers.Add("apikey", "your key");
            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            Stream stream = res.GetResponseStream();
            StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
            String content = reader.ReadToEnd();
            //Console.WriteLine(content);
            Dictionary<String,Dictionary<String,String>[]> dt = JsonConvert.DeserializeObject<Dictionary<String, Dictionary<String, String>>>(content);
            Console.WriteLine(dt["retData"]["weather"]);
            Console.WriteLine(dt["retData"]["temp"]);
            info+=(cityName+":"+dt["retData"]["weather"]+","+dt["retData"]["temp"]);
            return info;
        }
    }
}
