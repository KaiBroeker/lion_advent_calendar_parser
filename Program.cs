using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace lion_advent_calendar_parser
{
    class Program
    {
        static void Main(string[] args)
        {
            // get feedback only for today or all days?
            bool alldays = false;
            // set your calendar numbers here:
            String[] tmp_Numbers = { "", "", };
            // set the url here:
            string url = "http://www.lc-hoexter-corvey.de/content/adventsloskalender-2020-benefizaktion/gewinnnummern-adventsloskalender-2020/";

            List<string> lookingforNumbers = new List<string>();
            lookingforNumbers.AddRange(tmp_Numbers);

            // parse the html page and search for the needed lines
            // the website isn't well formated and very flat (only many paragraphs)
            var doc = (new HtmlWeb()).Load(url);
            var rows = doc.DocumentNode.SelectNodes("/html/body/div[2]/div[2]/div/div[1]/div/p/span");

            //init the parsers
            Regex rex_date = new Regex(@"\d{2}(\.|-)\d{2}(\.|-)\d{4}");
            Regex rex_number = new Regex(@"\d{4}");
            Regex rex_winning = new Regex(@"(?![Nr\. \d{4} ]).*");

            string cur_date = "";
            DateTime cur_date_parsed = new DateTime();
            string cur_number = "";
            string cur_winning = "";

            //iterate throught the rows and search, if one number has won
            // if alldays is false, only winnings of the current day will displayed
            foreach (var row in rows)
            {
                string text = row.GetDirectInnerText();
                if (rex_date.IsMatch(text)){
                    cur_date = rex_date.Match(text).ToString();
                    if (alldays == false){
                         cur_date_parsed = DateTime.Parse(cur_date);
                    }
                }else if(alldays == true || DateTime.Today == cur_date_parsed){
                    cur_number = rex_number.Match(text).ToString();
                    if (lookingforNumbers.Contains(cur_number))
                    {
                        cur_winning = rex_winning.Match(text).ToString();
                        Console.WriteLine("you won!!");
                        Console.WriteLine(cur_date);
                        Console.WriteLine(cur_number);
                        Console.WriteLine(cur_winning);
                    }
                }
            }
        }
    }
}
