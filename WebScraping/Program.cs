using System;
using System.IO;
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
            string section = webScraping.GetSelectTagElement("section", "class", "topics", false);
            List<string> ulStr = webScraping.GetSelectTagText(section, "ul", true);
            List<string> listStr = webScraping.GetAncherURL(ulStr[0]);
            foreach (string s in listStr)
            {
                var ws = new WebScraping(s);
                await ws.Scraping();
                List<string> pickUpStr = ws.GetSelectTag("p", "続きを読む", false);
                List<string> newURL = webScraping.GetAncherURL(pickUpStr[0]);
                ws = new WebScraping(newURL[0]);
                await ws.Scraping();

                Console.WriteLine("タイトル：" + ws.GetSelectTagElement("h1", new string[] { "class=\"sc-cooIXK krNmKM\"" }, true));
                Console.WriteLine("本文：" + ws.RemoveAncher(ws.GetSelectTagElement("p", "class", "yjSlinkDirectlink", true) + "\n\n"));

                csv.Add(new string[] { ws.GetSelectTagElement("h1", new string[] { "class=\"sc-cooIXK krNmKM\"" }, true), ws.RemoveAncher(ws.GetSelectTagElement("p", "class", "yjSlinkDirectlink", true)) });
            }
            try
            {
                StreamWriter file = new StreamWriter("test.csv", false, Encoding.UTF8);
                foreach(string[] csvText in csv)
                {
                    file.WriteLine(csvText[0].Replace("\n", "") + "," + csvText[1].Replace("\n", ""));
                }
                file.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
