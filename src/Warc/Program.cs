using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Warc
{
    class Program
    {
        static void Main(string[] args)
        {
            var warcFilePath = @"C:\Corpus\src.warc";

            var infoPath = @"C:\Corpus\info.txt";

            List<WarcItem> items = new List<WarcItem>();

            Console.WriteLine("Reading ...");

            var info = File.ReadLines(infoPath).ToList();

            var infoList = ExtractInfo(info);

            var text = File.ReadAllText(warcFilePath);

            var warcItems = ExtractWarCItems(text);

            FillWarItemList(items, warcItems);

            var newsOnly = FilterNews(items, infoList);

            foreach (var item in newsOnly)
            {
                item.Save();
            }

            //Save(items);

        }

        private static void FillWarItemList(List<WarcItem> items, List<string> warcItems)
        {
            int count = warcItems.Count;
            int index = 1;

            foreach (var item in warcItems)
            {
                Console.WriteLine($"Proccessing {index} of {count}");

                items.Add(WarcItem.Create(item));

                index++;
            }
        }

        private static List<WarcItem> FilterNews(List<WarcItem> items,List<InfoFile> infos)
        {

            var newsDocIds = infos.Where(a => a.Category == "NEWS_MEDIA").Select(a=> a.DocId).ToList();

            var result = items.Where(a => newsDocIds.Contains(a.DocId)).ToList();

            return result;
        }

        public static List<string> ExtractWarCItems(string data)
        {
            var result = new List<string>();
            
            var pattern = "WARC/1.0(.|\n)*?</html>";

            var matches = Regex.Matches(data, pattern);

            foreach (Match item in matches)
            {
                result.Add(item.Value);
            }

            return result;
        }

        public static List<InfoFile> ExtractInfo(List<string> data)
        {
            List<InfoFile> result = new List<InfoFile>();


            foreach (var item in data)
            {
                var info = InfoFile.Create(item);

                if (info != null)
                    result.Add(info);
            }

            return result;
        }

        public static void Save(List<WarcItem> items)
        {
            int index = 1;
            int count = items.Count;

            foreach (var item in items)
            {
                Console.WriteLine($"Proccessing {index} of {count}");

                item.Save();

                index++;
            }
        }
    }
}
