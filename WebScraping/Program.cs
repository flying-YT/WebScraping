using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<string[]> csv = new List<string[]>();
            WebScraping webScraping = new WebScraping("https://news.yahoo.co.jp/");
            await webScraping.Scraping();
            string ulStr = webScraping.GetSelectTagElementText("ul", new string[] { "class=\"topicsList_main\"" }, true);
            List<string> listStr = webScraping.GetAncherURL(ulStr);
            foreach (string s in listStr)
            {
                var ws = new WebScraping(s);
                await ws.Scraping();
                List<string> pickUpStr = ws.GetSelectTagText("p", "続きを読む", false);
                List<string> newURL = webScraping.GetAncherURL(pickUpStr[0]);
                ws = new WebScraping(newURL[0]);
                await ws.Scraping();

                Console.WriteLine("タイトル：" + ws.GetSelectTagElementText("h1", new string[] { "class=\"sc-cooIXK krNmKM\"" }, true));
                Console.WriteLine("本文：" + ws.GetSelectTagElementText("p", "class", "yjSlinkDirectlink", true) + "\n\n");

                //        csv.Add(new string[] { ws.GetSelectTagElementText( "h1", new string[] { "class=\"sc-cooIXK krNmKM\"" }, true ), ws.GetSelectTagText( "p", new string[] { "class=\"sc-cbkKFq dOvqCR yjSlinkDirectlink\"" }, true ).Replace("\n", "") } );//sc-gqPbQI hvfJU yjSlinkDirectlink
            }
        }
    }
}
