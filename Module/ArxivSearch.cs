//this module could not work perfectly

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace M1sakaBot.Module {
    public class ArxivSearch {
        private String _field;
        private String _content;
        private String _choice;
        public ArxivSearch(String field, String content,String choice) {
            _content = Regex.Replace(_content, " ", "%20");
            _choice = choice;
            _field = field;
        }
        public String[] linkGet() {
            String[] links =null;
            String search_link = "http://search.arxiv.org:8081/?query="+_content+"&in=grp_"+_field;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search_link);
            request.Referer = "http://arxiv.org/";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            //responce.Cookies;
            Stream res_stream = responce.GetResponseStream();
            StreamReader stream_reader = new StreamReader(res_stream, Encoding.GetEncoding("UTF-8"));
            string content = stream_reader.ReadToEnd();
            Regex reg_link = new Regex(@"https://arxiv.org/abs/.+?</a>");
            MatchCollection link_list = reg_link.Matches(content);
            Console.Write("choose option, 0 to print links, 1 to download additionally: ");
            String download_flag = Console.ReadLine();
            String[] list = new String[link_list.Count];
            int _ = 0;
            foreach (Match m in link_list) {
                list[_++] = Regex.Replace(m.ToString(), "</a>", "");
            }
            if(_choice=="1"){
                foreach (String m in list) {
                    Console.WriteLine(m);
                    if (download_flag == "1") {
                        String pdf_link = Regex.Replace(m, "abs", "pdf");
                        HttpWebRequest down_req = (HttpWebRequest)WebRequest.Create(pdf_link);
                        down_req.UserAgent =
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3047.4 Safari/537.36";
                        HttpWebResponse down_res = (HttpWebResponse)down_req.GetResponse();
                        WebClient cli = new WebClient();
                        Console.WriteLine(down_res.ResponseUri);
                        cli.Headers.Add(HttpRequestHeader.UserAgent,
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3047.4 Safari/537.36");
                        cli.DownloadFile(down_res.ResponseUri, "pdf/"+pdf_link.Split('/')[4] + ".pdf");

                    }
                }
                links = new String[list.Length];
                //for(int i = 0; i < list.Length; i++) {
                //    links[i] = "file:///" +;

                //}
            }
            else if(_choice=="0"){
                links = list;
            }
            return links;
        }
        public String TitleGet(String ArxivID) {
            String search_link = "https://arxiv.org/abs/" +ArxivID;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(search_link);
            request.Referer = "http://arxiv.org/";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            //responce.Cookies;
            Stream res_stream = responce.GetResponseStream();
            StreamReader stream_reader = new StreamReader(res_stream, Encoding.GetEncoding("UTF-8"));
            string content = stream_reader.ReadToEnd();
            Regex title_reg = new Regex(@"<title>.+?</title>");
            MatchCollection title_ = title_reg.Matches(content);
            String Title="";
            foreach(Match t in title_) {
                Title = t.Value;
            }
            Regex.Replace(Title, @"<title>", "");
            Regex.Replace(Title, @"</title>", "");
            return Title;
        }
    }
}