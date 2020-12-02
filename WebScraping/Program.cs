using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebScraping
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            WebScraping webScraping = new WebScraping("https://news.yahoo.co.jp/");
            await webScraping.Scraping();
            List<string> strList = webScraping.GetSelectTagText("b", true);
            foreach (string s in strList)
            {
                Console.WriteLine("b:" + s);
            }
            strList = webScraping.GetSelectTagText("a", true);
            foreach (string s in strList)
            {
                Console.WriteLine("a:" + s);
            }
            strList = webScraping.GetAncherURL();
            foreach (string s in strList)
            {
                Console.WriteLine("href:" + s);
            }
            Console.WriteLine("Finish");
        }
    }
}
