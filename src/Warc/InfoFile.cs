using System;
using System.Collections.Generic;
using System.Text;

namespace Warc
{
    public class InfoFile
    {
        public string DocId { get; set; }

        public string Category { get; set; }

        public string Url { get; set; }

        public static InfoFile Create(string data)
        {
            var split = data.Split("\t");

            if (split.Length > 2)
            {
                return new InfoFile()
                {
                    DocId = split[0],
                    Category = split[1],
                    Url = split[2]
                };
            }
            else
                return null;
        }
    }
}
