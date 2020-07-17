using Microsoft.Build.Framework;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp16
{
    public class File1c
    {
        public string name { get; set; }
        public string value { get; set; }
    }
 

    class Program
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main()
        {
            List<File1c> newfile1c = new List<File1c>();
            var path = Path.Combine(@"C:\Temp1\", "in");

            var files = Directory.GetFiles(path);

            foreach (var filepath in files)
            {
                logger.Debug("Обнаружен файл {0}", filepath);
            }


            foreach (var filepath in files)
            {
                logger.Debug("Файл {0}", filepath);
                var filename = Path.GetFileName(filepath);
                var fullpath = Path.Combine(path, filepath);
                bool success = ReadFile(fullpath, newfile1c);

                if (success == true)
                {
                    MoveFile(fullpath);
                }
                else
                {
                    MoveFile(fullpath,false);
                }

                ReadList(newfile1c);
                Console.ReadKey();
            }
        }

        public static bool ReadFile(string filename, List<File1c> newfile1c)
        {
            var srcEncoding = Encoding.GetEncoding(1251);
            var dstEncoding = Encoding.UTF8;
            bool success= false;

            using (StreamReader sr = new StreamReader(filename, encoding: srcEncoding))
            {

                string headers = sr.ReadLine();
                newfile1c.Add(new File1c{name = "system" , value = headers });


                int count = System.IO.File.ReadAllLines(filename).Length;
                int  i = 0;
                logger.Debug("Чтение данных");
                while (i < count)
                {
                   string onedouble = sr.ReadLine();

                   if (Check(onedouble) == true)
                    {
                        string[] lines = Regex.Split(onedouble, "[=]");

                        if (lines[0] != null)
                        {

                            if (lines[1] != null)
                            {
                                logger.Debug($"{lines[0]} {lines[1]}");
                                newfile1c.Add(new File1c { name = lines[0], value = lines[1] });
                                i++;
                                success = true;
                                
                            }
                            if (lines[1] == null)
                            {
                                newfile1c.Add(new File1c { name = lines[0], value = "" });
                                logger.Debug($"{lines[0]}");
                                i++;
                                success = true;
                            }

                        }
                    }

                    else
                    {
                        i++;
                        success = true;
                    }        

                }

            }
            return success;
        }

        public static bool Check(string onedouble)
        {
            bool check = false;
            if (onedouble != null)
            { 
            for (int j = 0; j < onedouble.Length; j++)
            {
                if (onedouble[j] == '=')
                {
                    check = true;
                    return check;
                }

            }
            }
            return check;
        
        }

        public static void MoveFile(string fullpath, bool error=false)
        {
            ///Перемещаем в папку архив
            if (error == false)
            {
               
            }

            ///Перемещаем в папку error 
            else
            {

            }

        }

        public static void ReadList(List<File1c> newfile1c)
        {

            foreach (File1c f in newfile1c)
            {
               // Console.WriteLine($" { f.name} = {f.value}");
            }

        }
    }
}
