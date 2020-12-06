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
            WebScraping webScraping = new WebScraping("https://news.yahoo.co.jp/");
            await webScraping.Scraping();
            string ulStr = webScraping.GetSelectTagText("ul", new string[] { "class=\"topicsList_main\"" }, true);
            List<string> listStr = webScraping.GetAncherURL(ulStr);
            foreach (string s in listStr)
            {
                var ws = new WebScraping(s);
                await ws.Scraping();
                string pickUpStr = ws.GetSelectTagText("p", new string[] { "class=\"sc-OqFzE hMpzGy\"" }, true);
                List<string> newURL = webScraping.GetAncherURL(pickUpStr);
                ws = new WebScraping(newURL[0]);
                await ws.Scraping();
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine( "タイトル：" + ws.GetSelectTagText( "h1", new string[] { "class=\"sc-ipZHIp lczCjB\"" }, true ) );
                Console.WriteLine( "本文：" + ws.GetSelectTagText( "p", new string[] { "class=\"sc-gqPbQI hvfJU yjSlinkDirectlink\"" }, true ) + "\n\n" );
            }
        }
    }
}
