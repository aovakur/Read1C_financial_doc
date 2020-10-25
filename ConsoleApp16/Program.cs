using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Logger = NLog.Logger;

namespace ConsoleApp16
{
    public class PaymentP
    {
        public string Otpravitel;
        public string Poluchatel;
        public DateTime DataSozdaniya;
        public DateTime VremyaSozdaniya;
        public string RaschSchet;
        public string Dokument;
        public string Nomer;
        public DateTime Data;
        public string Summa;
        public string PlatelshchikSchet;
        public string PlatelshchikINN;
        public string PlatelshchikKPP;
        public string Platelshchik1;
        public string PlatelshchikRaschSchet;
        public string PlatelshchikBank1;
        public string PlatelshchikBank2;
        public string PlatelshchikBIK;
        public string PlatelshchikKorschet;
        public string PoluchatelSchet;
        public string PoluchatelINN;
        public string Poluchatel1;
        public string PoluchatelRaschSchet;
        public string PoluchatelBank1;
        public string PoluchatelBank2;
        public string PoluchatelBIK;
        public string PoluchatelKorschet;
        public string VidPlatezha;
        public int VidOplaty;
        public int Ocherednost;
        public string NaznacheniePlatezha;
        public void TranslateToexport(string name, string value)
        {
            switch (name)
            {
                case "Отправитель":
                   Otpravitel = value;
                    break;
                case "Получатель":
                    Poluchatel = value;
                    break;
                case "ДатаСоздания":
                    DataSozdaniya = Convert.ToDateTime(value);
                    break;
                case "ВремяСоздания":
                    VremyaSozdaniya = Convert.ToDateTime(value);
                    break;
                case "РасчСчет":
                    RaschSchet = value;
                    break;
                case "Документ":
                    Dokument = value;
                    break;
                case "Номер":
                    Nomer = value;
                    break;
                case "Дата":
                    Data = Convert.ToDateTime(value);
                    break;
                case "Сумма":
                    Summa = value;
                    break;
                case "ПлательщикСчет":
                    PlatelshchikSchet = value;
                    break;
                case "ПлательщикИНН":
                    PlatelshchikINN = value;
                    break;
                case "ПлательщикКПП":
                    PlatelshchikKPP = value;
                    break;
                case "Плательщик1":
                    Platelshchik1 = value;
                    break;
                case "ПлательщикРасчСчет":
                    PlatelshchikRaschSchet = value;
                    break;
                case "ПлательщикБанк1":
                    PlatelshchikBank1 = value;
                    break;
                case "ПлательщикБанк2":
                    PlatelshchikBank2 = value;
                    break;
                case "ПлательщикБИК":
                    PlatelshchikBIK = value;
                    break;
                case "ПлательщикКорсчет":
                    PlatelshchikKorschet = value;
                    break;
                case "ПолучательСчет":
                    PoluchatelSchet = value;
                    break;
                case "Получатель1":
                    Poluchatel1 = value;
                    break;
                case "ПолучательРасчСчет":
                    PoluchatelRaschSchet = value;
                    break;
                case "ПолучательБанк1":
                    PoluchatelBank1 = value;
                    break;
                case "ПолучательБанк2":
                    PoluchatelBank2 = value;
                    break;
                case "ПолучательБИК":
                    PoluchatelBIK = value;
                    break;
                case "ПолучательИНН":
                    PoluchatelINN = value;
                    break;
                case "ПолучательКорсчет":
                    PoluchatelKorschet = value;
                    break;
                case "ВидПлатежа":
                    VidPlatezha = value;
                    break;
                case "ВидОплаты":
                    VidOplaty = Convert.ToInt16(value);
                    break;
                case "Очередность":
                    Ocherednost = Convert.ToInt16(value);
                    break;
                case "НазначениеПлатежа":
                    NaznacheniePlatezha = value;
                    break;
            }
        }
    }

    class Program
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        static string NewPath = "";
        static string path = "";
        static string archive = "";
        static string PathError ="" ;

        static void SetNewPath(string Path)
        {
            NewPath = Path;
        }

        
        static void SetReqParam()
        {
            var root = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            if (root.OpenSubKey("Translate1C", true) == null)
            {
                root.CreateSubKey("Translate1C", true).SetValue("Root", @"C:\Temp1\");
                NewPath = root.OpenSubKey("Translate1C", true).GetValue("Root").ToString();
            }
            else
            {
                NewPath = root.OpenSubKey("Translate1C", true).GetValue("Root").ToString();
            }

            logger.Debug("Корневая директория: " + NewPath);
            path = Path.Combine(NewPath, "in");
            archive = Path.Combine(NewPath, "archive");
            PathError = Path.Combine(NewPath, "error");

        }

        static void Main()
        {
            SetReqParam();
            logger.Debug("Version: {0}", Environment.Version.ToString());
            logger.Debug("OS: {0}", Environment.OSVersion.ToString());
            logger.Debug("Command: {0}", Environment.CommandLine.ToString());

            var files = Directory.GetFiles(path);
            logger.Debug("Обнаружено {0} файлов", files.Count());
            
            foreach (var filepath in files)
            {
                logger.Debug("Обнаружен файл {0}", filepath);
            }


            foreach (var filepath in files)
            {
                logger.Debug("Файл {0}", filepath);
                var filename = Path.GetFileName(filepath);
                var fullpath = Path.Combine(path, filepath);
                bool success = ReadFile(fullpath);

                if (success == true)
                {
                    MoveFile(filename);
                }
                else
                {
                    MoveFile(filename, false);
                }
            }
            
        }
        public static bool ReadFile(string filename)
        {
            var srcEncoding = Encoding.GetEncoding(1251);
            var dstEncoding = Encoding.UTF8;
            bool success = false;
            try
            {
                using (StreamReader sr = new StreamReader(filename, encoding: srcEncoding))
                {
                    string headers = sr.ReadLine();
                    int count = System.IO.File.ReadAllLines(filename).Length;
                    int i = 0;
                    logger.Debug("Чтение данных");
                    PaymentP paymentP = new PaymentP();
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
                                    paymentP.TranslateToexport(lines[0], lines[1]);
                                    i++;
                                    success = true;
                                }
                                if (lines[1] == null)
                                {
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
                    Export(paymentP);
                    paymentP=null;

                }
            }
            catch
            {
                logger.Debug("Ошибка чтения файла");
            }
            return success;

        }

        private static void Export(PaymentP paymentP)
        {
            Console.WriteLine(paymentP.Otpravitel);
            Console.WriteLine(paymentP.Poluchatel);
            Console.WriteLine(paymentP.DataSozdaniya);
            Console.WriteLine(paymentP.VremyaSozdaniya);
            Console.WriteLine(paymentP.RaschSchet);
            Console.WriteLine(paymentP.Dokument);
            Console.WriteLine(paymentP.Nomer);
            Console.WriteLine(paymentP.Data);
            Console.WriteLine("\n");
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

        public static void MoveFile(string filename, bool error=false)
        {
            try
            {
                ///Перемещаем в папку архив
                if (error == false)
                {
                    var FullPath = Path.Combine(path, filename);
                    var FullPathArchive = Path.Combine(archive, filename);
                    File.Move(FullPath, FullPathArchive);
                }
                ///Перемещаем в папку error 
                else
                {
                    var FullPath = Path.Combine(path, filename);
                    var FullPathError = Path.Combine(PathError, filename);
                    File.Move(FullPath, FullPathError);
                }
            }
            catch
            {
                logger.Debug("Ошибка перемещения файла");
            }

        }

    }
}
