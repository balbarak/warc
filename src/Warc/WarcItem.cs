using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Warc
{
    public class WarcItem
    {
        public string DocId { get; set; }

        public DateTime? Date { get; set; }

        public string Url { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }

        public string BodyStrip { get; set; }

        public static WarcItem Create(string data)
        {
            var body = ExtractBody(data);
            return new WarcItem()
            {
                DocId = ExtractDocId(data),
                Body  = body,
                BodyStrip = StripHtml(body)
            };
        }

        private static string StripHtml(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return null;

            string result = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);
            result = result.Replace("&nbsp;", " ");
            return result;
        }

        private static string ExtractDocId(string data)
        {
            string result = "";

            var pattern = "WARC-DOC-ID:(.+)";

            var row = Regex.Match(data, pattern)?.Value;

            var split = row.Split(':');

            if (split.Length > 1)
                result = split[1];

            return result;
        }

        public static string ExtractBody(string data)
        {
            var result = "";

            var bodyPattern = "<boSuccessfuly>(.|\n)*?</boSuccessfuly>";

            var match = Regex.Match(data, bodyPattern)?.Value;

            if (!String.IsNullOrWhiteSpace(match))
                result = match;

            return result;
        }

        public void Save()
        {
            if (String.IsNullOrWhiteSpace(Body))
                return;

            var output = $"output\\{DocId}.txt";

            File.WriteAllText(output,BodyStrip);
        }
    }
}
