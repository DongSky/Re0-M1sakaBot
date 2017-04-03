using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace M1sakaBot.Module {
    public class PixivGet {
        private const string referer = "http://www.pixiv.com/";
        public PixivGet() {

        }
        public async System.Threading.Tasks.Task get(string picid) {
            string url1, urlmulti;
            url1 = "http://www.pixiv.net/member_illust.php?mode=medium&illust_id="+picid;
            urlmulti = "http://www.pixiv.net/member_illust.php?mode=medium&illust_id="+picid;
            MatchCollection match = null;
            List<string> urllist = new List<string>();
            Regex reg = new Regex(@"http://\S+?master1200\.jpg");
            Regex finderr = new Regex(@"发生了错误");
            // url=Console.ReadLine();
            HttpWebRequest request_multi = (HttpWebRequest)WebRequest.Create(url1);
            request_multi.Referer = "http://www.pixiv.com/";
            HttpWebResponse res_multi = (HttpWebResponse)request_multi.GetResponse();
            Stream stream_multi = res_multi.GetResponseStream();
            StreamReader reader_multi = new StreamReader(stream_multi, System.Text.Encoding.GetEncoding("utf-8"));
            string content_multi = reader_multi.ReadToEnd();
            MatchCollection m = finderr.Matches(content_multi);
            if (m.Count > 0) {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url1);
                request.Referer = "http://www.pixiv.com/";
                HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                Stream stream = res.GetResponseStream();
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
                string content = reader.ReadToEnd();
                match = reg.Matches(content);
                Console.WriteLine(match.Count);
            }
            else {
                match = reg.Matches(content_multi);
                Console.WriteLine(match.Count);
            }

            foreach (Match ma in match) {
                urllist.Add(ma.Value);
            }
            //return urllist;
            foreach (string s in urllist) {
                Console.WriteLine(s);
                HttpWebRequest req0 = (HttpWebRequest)WebRequest.Create(s);
                req0.Referer = "http://www.pixiv.com/";
                WebClient cli = new WebClient();
                cli.Headers["Referer"] = "http://www.pixiv.com/";
                cli.DownloadFile(s, "i.jpg");
            }
        }
    }
}