using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Xml.Serialization;
using static System.Console;
namespace Laba5
{
    public class App
    {
        private static string file_name = "C:\\Users\\Filip\\OneDrive\\Робочий стіл\\Laba5\\Result_Folder\\Zodiak_Info.txt";
        private static string xml_name = "C:\\Users\\Filip\\OneDrive\\Робочий стіл\\Laba5\\Result_Folder\\Zodiak_Info.xml";
        private static string json_name= "C:\\Users\\Filip\\OneDrive\\Робочий стіл\\Laba5\\Result_Folder\\Zodiak_Info.json";

        private static List<Student> zodiaks = new List<Student>();
        private static List<Student> existingZodiaks=new List<Student>();
        private static XmlSerializer xml=new XmlSerializer(typeof(List<Student>));

        public static void Choice_Sort(List<Student> zodiaks)
        {
            WriteLine("\nВиберіть тип сортування: ігноруючи рік чи ні");
            string sort = ReadLine();
            switch (sort)
            {
                case "не ігноруючи":
                    zodiaks.Sort(Student.By_Year_Sort);
                    break;
                case "ігноруючи":
                    zodiaks.Sort(Student.By_Month_Sort);
                    break;
                default:
                    zodiaks.Sort();
                    break;
            }
        }
        public static void Adding_Student_To_List()
        {
            Write("Скільки хочете ввести студентів: ");
            int length_student=int.Parse(ReadLine());
            WriteLine("Вводьте студентів: ");
            for(int i = 0; i < length_student; i++)
            {
                string line=ReadLine();
                Student zodiak = new Student(line);
                zodiaks.Add(zodiak);
            }
        }
        public static void Add_To_Text_File(FileMode mode,List<Student> list)
        {
            using (FileStream file=new FileStream(file_name,mode)) {
                using (TextWriter reader=new StreamWriter(file))
                {
                    for(int i = 0; i < list.Count; i++)
                    {
                        reader.WriteLine(list[i].sur_name + " "+list[i].name+" " + list[i].date.ToShortDateString());
                    }
                    
                }
            }
        }
        public static void Add_New_XML_File()
        {
            
            using (FileStream file = new FileStream(file_name, FileMode.Open))
            {
                existingZodiaks = (List<Student>)xml.Deserialize(file);
            }
            existingZodiaks.AddRange(zodiaks);
            Choice_Sort(existingZodiaks);
            Add_To_Text_File(FileMode.Truncate, existingZodiaks);
            using (FileStream file = new FileStream(xml_name, FileMode.Create))
            {
                xml.Serialize(file, existingZodiaks);
            }
        }
        public static void Add_New_JSON_File()
        {
            string text = File.ReadAllText(json_name);
            existingZodiaks = JsonSerializer.Deserialize<List<Student>>(text);
            existingZodiaks.AddRange(zodiaks);

            var options= new JsonSerializerOptions() { WriteIndented= true,Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string json_string=JsonSerializer.Serialize(existingZodiaks,options);
            File.WriteAllText(json_name, json_string);
        }
        public static void Create_To_XML_File()
        {
            using (FileStream file = new FileStream(file_name, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(file))
                {
                    xml.Serialize(writer, zodiaks);
                }
            }

        }
        public static void Create_To_JSON_File()
        {
            var options = new JsonSerializerOptions() { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string json_string = JsonSerializer.Serialize(zodiaks,options);
            File.WriteAllText(json_name, json_string);
        }

        public static void Main()
        {
            string choice;
            do
            {
                WriteLine("\nБажаєте доповнити чи переписати текстовий файл чи ввести вже прізвище ?");
                choice = ReadLine();
                switch (choice)
                {
                    case "доповнити":
                        try
                        {
                            Adding_Student_To_List();
                            WriteLine("Добавляємо до текстового та xml файлу");
                            Add_New_XML_File();
                            Add_New_JSON_File();
                        }
                        catch(Exception e)
                        {
                            WriteLine(e.Message);
                        }
                        break;
                    case "переписати":
                        try
                        {
                            Adding_Student_To_List();
                            WriteLine("Добавляємо до текстового та xml файлу");
                            Choice_Sort(zodiaks);
                            Add_To_Text_File(FileMode.Truncate,zodiaks);
                            Create_To_XML_File();
                            Create_To_JSON_File();
                        }
                        catch(Exception e)
                        {
                            WriteLine(e.Message);
                        }
                        break;
                    case "вивести":
                        WriteLine("Введіть прізвище");
                        string sur_name = ReadLine();
                        List<Student> matchingZodiaks = new List<Student>();

                        using (TextReader reader = new StreamReader(xml_name))
                        {
                            List<Student> result = (List<Student>)xml.Deserialize(reader);
                            foreach (Student zodiak in result)
                            {
                                if (zodiak.sur_name == sur_name)
                                {
                                    matchingZodiaks.Add(zodiak);
                                }
                            }
                        }
                        if (matchingZodiaks.Count == 0)
                        {
                            WriteLine("Немає інформації для цього прізвища.");
                        }
                        else
                        {
                            foreach (var zodiak in matchingZodiaks)
                            {
                                WriteLine(zodiak);
                            }
                        }
                        break;

                }
            } while (choice != "кінець");


        }
    }
}