using Microsoft.Build.Framework;
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

        static void Main()
        {
            List<File1c> newfile1c = new List<File1c>();
            var path = Path.Combine(@"C:\Temp1\", "in");

            var files = Directory.GetFiles(path);

            foreach (var filepath in files)
            {
                var filename = Path.GetFileName(filepath);
                var fullpath = Path.Combine(path, filepath);
                ReadFile(fullpath, newfile1c);
                ReadList(newfile1c);

            }
        }

        public static void ReadFile(string filename, List<File1c> newfile1c)
        {
            var srcEncoding = Encoding.GetEncoding(1251);
            var dstEncoding = Encoding.UTF8;

            using (StreamReader sr = new StreamReader(filename, encoding: srcEncoding))
            {

                string headers = sr.ReadLine();
                newfile1c.Add(new File1c{name = "system" , value = headers });


                int count = System.IO.File.ReadAllLines(filename).Length;
                int  i = 0;
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
                                newfile1c.Add(new File1c { name = lines[0], value = lines[1] });
                                i++;
                                
                            }
                            if (lines[1] == null)
                            {
                                newfile1c.Add(new File1c { name = lines[0], value = "" });
                                i++;
                            }
                            

                        }
                    }

                    else
                    {
                        i++;
                    }        

                }

            }

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

        public static void ReadList(List<File1c> newfile1c)
        {

            foreach (File1c f in newfile1c)
            {
                Console.WriteLine($" { f.name} = {f.value}");
            }

            Console.ReadLine();

        }
    }
}
