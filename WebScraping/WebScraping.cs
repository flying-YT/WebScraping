using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScraping
{
    public class WebScraping
    {
        private readonly string url;
        public string HtmlText { set; get; }

        public WebScraping(string _url)
        {
            url = _url;
        }

        public async Task Scraping()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, @url);

                var res = await client.SendAsync(request);
                string HTMLText = await res.Content.ReadAsStringAsync();

                HtmlText = RemoveStyle(RemoveScript(HTMLText));
            }
            catch
            {
                Console.WriteLine("ScrapingErr");
            }
        }

        private string RemoveScript(string _str)
        {
            string str = _str;
            MatchCollection matches = Regex.Matches(str, @"<script(.*?|\n)*>(.*?|\n)*</script>");
            foreach (Match match in matches)
            {
                str = str.Replace(match.Value, "");
            }
            return str;
        }

        private string RemoveStyle(string _str)
        {
            string str = _str;
            MatchCollection matches = Regex.Matches(str, @"<style(.*?|\n)*>(.*?|\n)*</style>");
            foreach (Match match in matches)
            {
                str = str.Replace(match.Value, "");
            }
            return str;
        }

        public List<string> GetSelectTagText(string tag, bool noTag)
        {
            List<string> list = new List<string>();
            string key = "<" + tag + @"(\s|>)(.*?|\n|>)*(.*?|\n)*</" + tag + @">";
            MatchCollection matches = Regex.Matches(HtmlText, @key);
            foreach (Match match in matches)
            {
                if (noTag)
                {
                    string textStr = match.Value.Replace("</" + tag + ">", "");
                    list.Add(Regex.Replace(textStr, @"<(.*?)>", ""));
                }
                else
                {
                    list.Add(match.Value);
                }
            }
            return list;
        }

        public string GetSelectTagText(string tag, string[] options, bool noTag)
        {
            string option = "";
            foreach (string str in options)
            {
                option += str + " ";
            }
            option = option.Trim();
            string key = "<" + tag + " " + option + @"(\s|>)(.*?|\n|>)*(.*?|\n)*</" + tag + @">";
            //            Console.WriteLine( "key:" + key );
            Match match = Regex.Match(HtmlText, @key);
            if (noTag)
            {
                string textStr = match.Value.Replace("</" + tag + ">", "");
                key = "<" + tag + " " + option + @"(\s|>)(.*?|\n)*>?";
                return Regex.Replace(textStr, key, "");
            }
            else
            {
                return match.Value;
            }
        }

        public List<string> GetAncherURL()
        {
            List<string> list = new List<string>();
            string key = @"<a(\s|>)(.*?|\n|>)*(.*?|\n)*</a>";
            MatchCollection matches = Regex.Matches(HtmlText, @key);
            foreach (Match match in matches)
            {
                if (!Regex.Match(match.Value, @"rel=nofollow").Success)
                {
                    Match href = Regex.Match(match.Value, @"href=""(.*?)""");
                    string hrefText = href.Value.Replace("\"", "");
                    list.Add(hrefText.Replace("href=", ""));
                }
            }
            return list;
        }

        public List<string> GetAncherURL(string str)
        {
            List<string> list = new List<string>();
            string key = @"<a(\s|>)(.*?|\n|>)*(.*?|\n)*</a>";
            MatchCollection matches = Regex.Matches(str, @key);
            foreach (Match match in matches)
            {
                if (!Regex.Match(match.Value, @"rel=nofollow").Success)
                {
                    Match href = Regex.Match(match.Value, @"href=""(.*?)""");
                    string hrefText = href.Value.Replace("\"", "");
                    list.Add(hrefText.Replace("href=", ""));
                }
            }
            return list;
        }
    }
}
