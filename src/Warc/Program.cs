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
            var warcFilePath = @"C:\Repos\warc\data\arabicweb16.categorized.content.warc";

            List<WarcItem> items = new List<WarcItem>();

            Console.WriteLine("Reading ...");

            var text = File.ReadAllText(warcFilePath);

            var warcItems = ExtractWarCItems(text);

            int count = warcItems.Count;
            int index = 1;

            foreach (var item in warcItems)
            {
                Console.WriteLine($"Proccessing {index} of {count}");

                items.Add(WarcItem.Create(item));

                index++;
            }

            Save(items);

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
