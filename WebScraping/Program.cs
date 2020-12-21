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
            int count = 0;

            Console.WriteLine("Scraping Start");
            List<string[]> csv = new List<string[]>();
            WebScraping webScraping = new WebScraping("https://news.yahoo.co.jp/");
            await webScraping.Scraping();
            string section = webScraping.GetSelectTagElement("section", "class", "topics", false);
            List<string> ulStr = webScraping.GetSelectTagText(section, "ul", false);
            List<string> listStr = webScraping.GetAncherURL(ulStr[0]);
            List<string> titleStr = webScraping.GetSelectTagText(section, "a", true);
            foreach (string s in listStr)
            {
                var ws = new WebScraping(s);
                await ws.Scraping();
                List<string> pickUpStr = ws.GetSelectTag("p", "続きを読む", false);
                List<string> newURL = webScraping.GetAncherURL(pickUpStr[0]);
                ws = new WebScraping(newURL[0]);
                await ws.Scraping();

                Console.WriteLine("タイトル：" + titleStr[count]);
                Console.WriteLine("本文：" + ws.RemoveAncher(ws.GetSelectTagElement("p", "class", "yjSlinkDirectlink", true) + "\n\n"));

                csv.Add(new string[] { titleStr[count], ws.RemoveAncher(ws.GetSelectTagElement("p", "class", "yjSlinkDirectlink", true)) });
                count++;
            }
            try
            {
                Console.WriteLine("Directory");
                Console.WriteLine(Environment.CurrentDirectory);
                DateTime dt = DateTime.Now;
                string fileName = dt.ToString("yyyyMMddHHmmss");
                StreamWriter file = new StreamWriter(fileName + ".csv", false, Encoding.UTF8);
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
