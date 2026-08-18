using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Task3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // начало работы приложения
            Boolean LLErrorFound = false;
            List<string> LAErrorDescrs = new List<string>() { };

            string LCSrcPath = "";
            string LCMainDestPath = "";

            FileInfo LOMainFile = null;
            DirectoryInfo LODestDir = null;

            StreamReader LOSrcReader = null;
            StreamWriter LOMainDestWriter = null;
            StreamWriter LOAdditWriter = null;

            Encoding LOEncUTF8 = new System.Text.UTF8Encoding(false);
            string LCAdditDestFName = "";

            IFileFormatType LOFileType = null;
            string LCSrcString = null;
            Boolean LLStringTypeIsFound = false;

            int LNi = 0;    // перечислитель доступных типов
            int LNTypesCount = 0;
            Type LOCurrentType = null;
            ConstructorInfo LOConstructor1 = null;

            ISplitResuLt LOSplitResuLt = null;
            IList<string> LAStringParts = null;


            if (!LLErrorFound)
            {
                try
                {
                    if (args.Length != 2)
                    {
                        LLErrorFound = true;
                        LAErrorDescrs.Add(
                            "Необходимо указать 2 параметра: путь к исходному файлу и путь к файлу назначения"
                        );
                    }
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(
                        $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}");
                }   // конец попытки  
            }   // не было ошибки . Окончание 

            if (!LLErrorFound)
            {
                try
                {
                    LCSrcPath = args[0];
                    LCMainDestPath= args[1];

                    if (!(File.Exists(LCSrcPath)))
                    { 
                        LLErrorFound = true;
                        LAErrorDescrs.Add(
                            $"Ошибка: файл-источник не существует");
                    }

                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(
                        $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}");
                }   // конец попытки
            }   // не было ошибки . Окончание

            if (!LLErrorFound)
            {
                try
                {
                    if (LCMainDestPath.Length == 0)
                    {
                        LLErrorFound = true;
                        LAErrorDescrs.Add(
                            $"Ошибка: второй параметр пуст.");
                    }
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(
                        $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}");
                }   // конец попытки
            }   // не было ошибки . Окончание

            // проверка пути назначения на правильность
            if (!LLErrorFound)
            {
                try
                {
                    if (!(Regex.IsMatch(LCMainDestPath,
                        @"(?i)^(?:[a-z]\:)(?:\\([^\\|\/|\:|\*|\?|\""|\<|\>|\|]+))+$")))
                    {
                        LLErrorFound = true;
                        LAErrorDescrs.Add(
                            "Ошибка: абсолютный путь к файлу назначения имеет неправильный формат.");
                    }
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(
                        $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}");
                }   // конец попытки
            }   // не было ошибки . Окончание

            if (!LLErrorFound)
            {
                try
                {
                    if (Regex.IsMatch(LCMainDestPath, @"(?i)\\problems\.txt$"))
                    { 
                        LLErrorFound = true;
                        LAErrorDescrs.Add(
                            "Ошибка: название файла назначения совпадает с названием файла проблем");
                    }
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(
                        $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}");
                }   // конец попытки
            }   // не было ошибки . Окончание

            if (!LLErrorFound)
            {
                try
                {
                    // файл назначения
                    LOMainFile = new FileInfo(LCMainDestPath);

                    LODestDir = LOMainFile.Directory;
                    if (!(LODestDir.Exists))
                    {
                        LLErrorFound = true;
                        LAErrorDescrs.Add(
                            "Ошибка: директория файла назначения не существует.");
                    }
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(
                        $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}");
                }   // конец попытки
            }   // не было ошибки . Окончание

            if (!LLErrorFound)
            {
                try
                {
                    LOSrcReader = new StreamReader(LCSrcPath);
                    if (LOMainFile.Exists)
                    {
                        LOMainFile.Delete();
                    }

                    LOMainDestWriter = new StreamWriter(LOMainFile.FullName, true, LOEncUTF8);

                    LCAdditDestFName = $@"{LODestDir.FullName}\problems.txt";

                    // удаляем файл проблем, если есть
                    if (File.Exists(LCAdditDestFName))
                    { 
                        File.Delete(LCAdditDestFName);
                    }

                    LOAdditWriter = new StreamWriter(LCAdditDestFName, true, LOEncUTF8);

                    while (!(LOSrcReader.EndOfStream) && (!LLErrorFound))
                    { 
                        LCSrcString = LOSrcReader.ReadLine();   // строка в исходном файле
                        LLStringTypeIsFound = false;    // пока тип строки не известен

                        LNi = 0;
                        LNTypesCount = TypeConstants.LAStrTypes.Count;
                        LAStringParts = null;

                        while ((LNi < LNTypesCount) && (!LLStringTypeIsFound) && (!LLErrorFound))
                        {
                            LOCurrentType = TypeConstants.LAStrTypes[LNi];
                            LOConstructor1 = LOCurrentType.GetConstructor(new Type[] { });
                            LOFileType = (IFileFormatType)LOConstructor1.Invoke(new object[] { });

                            if (LOFileType == null)
                            {
                                LLErrorFound = true;
                                LAErrorDescrs.Add(
                                    $@"Ошибка: тип {LOCurrentType.Name} не поддержиивает интерфейс ""Task3.IFileFormatType""");
                            }

                            if (!LLErrorFound)
                            {
                                LOSplitResuLt = LOFileType.SplitString(LCSrcString);
                                LLErrorFound = LOSplitResuLt.LLErrorFound;
                                if(LLErrorFound)
                                {
                                    foreach (string Err in LOSplitResuLt.LAErrors)
                                    {
                                        LAErrorDescrs.Add(Err);
                                    }
                                }
                            }   // не было ошибки . Окончание

                            if (!LLErrorFound)
                            {
                                LLStringTypeIsFound = LOSplitResuLt.LLStringTypeIsFound;
                                if (LLStringTypeIsFound)
                                {
                                    LAStringParts = LOSplitResuLt.LAStringParts;
                                }
                            
                            }   // не было ошибки . Окончание

                            LNi += 1;
                        }   // перебор по списку типов файлов

                        if (!LLErrorFound)
                        {
                            if (LLStringTypeIsFound)
                            {
                                // пишем в главный файл назначения
                                LOMainDestWriter.WriteLine(
                                    $"{LAStringParts[0]}\t{LAStringParts[1]}\t{LAStringParts[2]}\t{LAStringParts[3]}\t{LAStringParts[4]}");
                            }
                            else
                            {
                                // тип не найден
                                LOAdditWriter.WriteLine(LCSrcString);
                            }   // тип найден или не найден
                        }   // не было ошибки . Окончание
                    }   // перебор по строкам исходного файла . Окончание

                    LOSrcReader.Close();
                    LOSrcReader.Dispose();
                    LOSrcReader = null;

                    LOMainDestWriter.Flush();
                    LOMainDestWriter.Close();
                    LOMainDestWriter.Dispose();
                    LOMainDestWriter = null;

                    LOAdditWriter.Flush();
                    LOAdditWriter.Close();
                    LOAdditWriter.Dispose();
                    LOAdditWriter = null;

                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(
                        $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}");
                }   // конец попытки

            }   // не было ошибки . Окончание

            if (LLErrorFound)
            {
                Console.WriteLine("Произошли ошибки:");
                foreach (string Err in LAErrorDescrs)
                {
                    Console.WriteLine(Err);
                }
            }
            else
            {
                Console.WriteLine("Задача выполнена без ошибок");
            }   // была ошибка или не было . Окончание

        }   // Main . Окончание
    }   // консольное приложение . Окончание
}   // пространство имён Task3 (задание 3)
