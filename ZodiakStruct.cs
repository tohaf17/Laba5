using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Laba5
{
    [XmlRoot("zodiak")]
    [Serializable]
    public struct Student:IComparable<Student>
    {
        public static readonly List<(string, DateTime[])> list;

        [XmlAttribute("sur_name")]
        [JsonPropertyName("sur_name")]
        public string sur_name { get; set; }

        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string name { get; set; }

        [XmlAttribute("date_array")]
        [JsonPropertyName("date")]
        public DateTime date { get; set; }

        [XmlAttribute("zodiak")]
        [JsonPropertyName("zodia")]
        public string zodia { get; set; }

        static Student()
        {
            List<(string, DateTime[])> Zodiaks = new List<(string, DateTime[])>();
            Zodiaks.Add(("Aries", new DateTime[2] { new DateTime(1, 3, 21), new DateTime(1, 4, 19) }));
            Zodiaks.Add(("Taurus", new DateTime[2] { new DateTime(1, 4, 20), new DateTime(1, 5, 20) }));
            Zodiaks.Add(("Gemini", new DateTime[2] { new DateTime(1, 5, 21), new DateTime(1, 6, 21) }));
            Zodiaks.Add(("Cancer", new DateTime[2] { new DateTime(1, 6, 22), new DateTime(1, 7, 22) }));
            Zodiaks.Add(("Leo", new DateTime[2] { new DateTime(1, 7, 23), new DateTime(1, 8, 22) }));
            Zodiaks.Add(("Virgo", new DateTime[2] { new DateTime(1, 8, 23), new DateTime(1, 9, 22) }));
            Zodiaks.Add(("Libra", new DateTime[2] { new DateTime(1, 9, 23), new DateTime(1, 10, 23) }));
            Zodiaks.Add(("Scorpius", new DateTime[2] { new DateTime(1, 10, 24), new DateTime(1, 11, 21) }));
            Zodiaks.Add(("Sagittarius", new DateTime[2] { new DateTime(1, 11, 22), new DateTime(1, 12, 21) }));
            Zodiaks.Add(("Capricornus", new DateTime[2] { new DateTime(1, 12, 22), new DateTime(1, 1, 19) }));
            Zodiaks.Add(("Aquarius", new DateTime[2] { new DateTime(1, 1, 20), new DateTime(1, 2, 18) }));
            Zodiaks.Add(("Pisces", new DateTime[2] { new DateTime(1, 2, 19), new DateTime(1, 3, 20) }));
            list = Zodiaks;
        }

        public Student(string line)
        {
            string[] data = line.Split();
            sur_name = data[0];
            name = data[1];
            date = DateTime.ParseExact(data[2], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
            zodia = DetermineZodiak(date);
        }

        public static string DetermineZodiak(DateTime date)
        {
            foreach (var zodiak in list)
            {
                if ((date.Month == zodiak.Item2[0].Month && date.Day >= zodiak.Item2[0].Day) ||
                    (date.Month == zodiak.Item2[1].Month && date.Day <= zodiak.Item2[1].Day))
                {
                    return zodiak.Item1.ToString();
                }
            }
            return "Невідомо";
        }

        public override string ToString()
        {
            return $"{sur_name} {name} {zodia} {date.ToShortDateString()}";
        }
        public int CompareTo(Student other)
        {
            return sur_name.CompareTo(other.sur_name);
        }

        public static int By_Year_Sort(Student first, Student second)
        {
            return first.date.CompareTo(second.date);
        }

        public static int By_Month_Sort(Student first, Student second)
        {
            int monthComparison = first.date.Month.CompareTo(second.date.Month);
            if (monthComparison == 0)
            {
                return first.date.Day.CompareTo(second.date.Day);
            }
            return monthComparison;
        }
    }
}
