using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Task3;


namespace UnitTestTask3
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }

        public void Inform(string s)
        {
            TestContext.WriteLine(s);
        }

        public void PrintStringList(List<string> LA1)
        {
            foreach (string s in LA1)
            {
                this.Inform(s);
            }
        }


        // первый параметр - фактический результат, второй параметр - ожидаемый результат
        public Boolean CompareStringLists(List<string> LA1, List<string> LA2, int LNLParTestNo)
        {
            int LNj = 0;
            int LNListPower = 0;

            Boolean LLResuLt = true;    //пока считаем, что результаты равны

            // если оба массива = null
            if ((LA1 != null) && (LA2 != null))
            {

                // сравнение с выдачей результата
                if (LA1.Count != LA2.Count)
                {
                    this.Inform($"Тест {LNLParTestNo}: несовпадение по мощности результата");
                    this.Inform("Фактический результат:");
                    this.PrintStringList(LA1);
                    LLResuLt = false;   //фактический результат не правильный
                }
                else
                {
                    // количество элементов одинаковое
                    LNj = 0;
                    LNListPower = LA1.Count;
                    while (LNj < LNListPower)
                    {
                        if (LA1[LNj] != LA2[LNj])
                        {
                            if (LLResuLt)
                            {
                                LLResuLt = false;
                            }

                            this.Inform($"Элемент № {LNj + 1} \"{LA1[LNj]}\" не равен \"{LA2[LNj]}\"");
                        }

                        LNj += 1;
                    }   // перебор по списку строк . Окончание
                }   // мощности списков не равны или равны
            }
            else
            {
                // либо первый массив 0, либо второй массив 0, либо оба
                // если оба 0 => равны, иначе - не равны
                if ((LA1 == null) && (LA2 == null))
                {
                    // оба массива - нули
                    LLResuLt = true;    // массивы равны
                }
                else
                {
                    if (LA1 != null)
                    {
                        this.Inform("Результат null, ожидаемый результат не null");
                    }
                    else
                    {
                        this.Inform("Результат не null, ожидаемый результат null");
                    }

                    LLResuLt = false;

                }   // оба списка нули или другое . Окончание
            }   // оба списка не нули или другое

            return LLResuLt;
        }   // сравнение списков строк . Окончание

        // сообщения метода
        private const string LCLConstErrorTitle = "Произошли ошибки:";

        private const string LCLConstSuccessTitle = "Задача выполнена без ошибок";

        private const string LCLConstWrongArgCount =
            "Необходимо указать 2 параметра: путь к исходному файлу и путь к файлу назначения";

        private const string LCLConstMsg1 =
            "Ошибка: файл-источник не существует";

        private const string LCLConstMsg2 =
            "Ошибка: второй параметр пуст.";

        private const string LCLConstMsg3 =
            "Ошибка: абсолютный путь к файлу назначения имеет неправильный формат.";

        private const string LCLConstMsg4 =
            "Ошибка: название файла назначения совпадает с названием файла проблем";

        private const string LCLConstMsg5 =
            "Ошибка: директория файла назначения не существует.";

        private const int LNLConstSrcPathIndex = 0;
        private const int LNLConstDestPathIndex = 1;
        private const int LNLConstSrcFileExistsIndex = 2;
        private const int LNLConstDestDirExistsIndex = 3;
        private const int LNLConstSrcContentsIndex = 4;

        private const string LCLConstSrcPath = @"F:\TestSrcDir\src1.txt";
        private const string LCLConstDestPath = @"F:\TestDestDir\dest1.txt";

        [TestMethod]
        public void TestMethod1()
        {
            StringWriter LOWriter = null;

            List<object[]> LABeginData = null;
            List<object[]> LAExpectedData = null;

            int LNi = 0;
            int LNTestsCount = 0;

            Boolean LLSrcFileExists = false;    // признак существования исходного файла
            Boolean LLDestDirExists = false;    // признак существования директории назначения

            FileInfo LOSrcFile = null;
            FileInfo LODestFile = null;

            StreamWriter LOFWriter = null;
            List<string> LASrcContents = null;

            // фактические параметры
            string LCFactOutput = null; // выдача программы в консоль
            Boolean LLFactMainDestFileExists = false;
            List<string> LAFactMainDestContents = null;
            Boolean LLFactAdditDestFileExists = false;
            List<string> LAFactAdditDestContents = null;

            // параметры ожидаемого результата
            string LCExpectedOutput = null;
            Boolean LLExpectedMainDestFileExists = false;
            List<string> LAExpectedMainDestContents = null;
            Boolean LLExpectedAdditDestFileExists = false;
            List<string> LAExpectedAdditDestContents = null;


            StreamReader LOFReader = null;

            Boolean LLTestIsOK = false;

            string LCMainDestFilePath = null;
            string LCAdditDestFilePath = null;

            Boolean LLListsAreEqual = false;

            string LCDestFilePath = null;

            try
            {
                LABeginData = new List<object[]>()
                {
                    // 1
                    // неправильное количество параметров
                    new object[]
                    { 
                        LCLConstSrcPath,
                        LCLConstDestPath,
                        false,  // признак существования исходного файла
                        false,  // признак существования родительской директории результирующего файла
                        null,    // содержимое исходного файла
                        new string[]{ LCLConstSrcPath }  // параметр программы
                    },

                    // 2
                    // файл - источник не существует
                    new object[]
                    {
                        LCLConstSrcPath,
                        LCLConstDestPath,
                        false,  // признак существования исходного файла
                        false,  // признак существования родительской директории результирующего файла
                        null,    // содержимое исходного файла
                        new string[]{ LCLConstSrcPath, LCLConstDestPath }  // параметр программы
                    },

                    // 3
                    // второй параметр пуст
                    new object[]
                    {
                        LCLConstSrcPath,
                        "",
                        true,  // признак существования исходного файла
                        false,  // признак существования родительской директории результирующего файла
                        null,    // содержимое исходного файла
                        new string[]{ LCLConstSrcPath, "" }  // параметр программы
                    },

                    // 4
                    // путь к файлу назначения имеет неправильный формат
                    new object[]
                    {
                        LCLConstSrcPath,
                        @"F:\",
                        true,  // признак существования исходного файла
                        false,  // признак существования родительской директории результирующего файла
                        null,    // содержимое исходного файла
                        new string[]{ LCLConstSrcPath, @"F:\" }  // параметр программы
                    },

                    // 5
                    // название файла назначения совпадает с названием файла проблем
                    new object[]
                    {
                        LCLConstSrcPath,
                        @"F:\dir1\problem.txt",
                        true,  // признак существования исходного файла
                        true,  // признак существования родительской директории результирующего файла
                        null,    // содержимое исходного файла
                        new string[]{ LCLConstSrcPath, @"F:\dir1\problems.txt" }  // параметр программы
                    },

                    // 6
                    // директория файла назначения не существует
                    new object[]
                    {
                        LCLConstSrcPath,
                        LCLConstDestPath,
                        true,  // признак существования исходного файла
                        false,  // признак существования родительской директории результирующего файла
                        null,    // содержимое исходного файла
                        new string[]{ LCLConstSrcPath, LCLConstDestPath }  // параметр программы
                    },

                    // 7
                    // прогон программы
                    new object[]
                    {
                        LCLConstSrcPath,
                        LCLConstDestPath,
                        true,  // признак существования исходного файла
                        true,  // признак существования родительской директории результирующего файла
                        new List<string>
                        {
                            @"10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
                            @"2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'",
                            @"Строка, не соответствующая ни одному из форматов"
                        },     
                        // содержимое исходного файла
                        new string[]{ LCLConstSrcPath, LCLConstDestPath }  // параметр программы
                    }

                };

                LAExpectedData = new List<object[]>()
                {
                    // 1
                    // неправильное количество параметров
                    new object[]
                    { 
                        $"{LCLConstErrorTitle}{Environment.NewLine}{LCLConstWrongArgCount}{Environment.NewLine}",
                        null,   // содержимое файла-результата
                        null,   // содержимое файла проблем
                        false,  // признак существования файла-результата
                        false   // признак существования файла проблем
                    },

                    // 2
                    // файл - источник не существует
                    new object[]
                    {
                        $"{LCLConstErrorTitle}{Environment.NewLine}{LCLConstMsg1}{Environment.NewLine}",
                        null,   // содержимое файла-результата
                        null,   // содержимое файла проблем
                        false,  // признак существования файла-результата
                        false   // признак существования файла проблем
                    },
                    
                    // 3
                    // второй параметр пуст
                    new object[]
                    {
                        $"{LCLConstErrorTitle}{Environment.NewLine}{LCLConstMsg2}{Environment.NewLine}",
                        null,   // содержимое файла-результата
                        null,   // содержимое файла проблем
                        false,  // признак существования файла-результата
                        false   // признак существования файла проблем
                    },

                    // 4
                    // путь к файлу назначения имеет неправильный формат
                    new object[]
                    {
                        $"{LCLConstErrorTitle}{Environment.NewLine}{LCLConstMsg3}{Environment.NewLine}",
                        null,   // содержимое файла-результата
                        null,   // содержимое файла проблем
                        false,  // признак существования файла-результата
                        false   // признак существования файла проблем
                    },

                    // 5
                    // название файла назначения совпадает с названием файла проблем
                    new object[]
                    {
                        $"{LCLConstErrorTitle}{Environment.NewLine}{LCLConstMsg4}{Environment.NewLine}",
                        null,   // содержимое файла-результата
                        null,   // содержимое файла проблем
                        false,  // признак существования файла-результата
                        false   // признак существования файла проблем
                    },

                    // 6
                    // директория файла назначения не существует
                    new object[]
                    {
                        $"{LCLConstErrorTitle}{Environment.NewLine}{LCLConstMsg5}{Environment.NewLine}",
                        null,   // содержимое файла-результата
                        null,   // содержимое файла проблем
                        false,  // признак существования файла-результата
                        false   // признак существования файла проблем
                    },

                    // 7
                    // прогон программы
                    new object[]
                    {
                        $"{LCLConstSuccessTitle}{Environment.NewLine}",
                        new List<string>()
                        {
                            @"10-03-2025	15:14:49.523	INFO	DEFAULT	Версия программы: '3.4.0.48729'",
                            @"10-03-2025	15:14:51.5882	INFO	MobileComputer.GetDeviceId	 Код устройства: '@MINDEO-M40-D-410244015546'"
                        },   // содержимое файла-результата
                        new List<string>()
                        {
                            @"Строка, не соответствующая ни одному из форматов"
                        },   // содержимое файла проблем
                        true,  // признак существования файла-результата
                        true   // признак существования файла проблем
                    }

                };

                LNi = 0;
                LNTestsCount = LABeginData.Count;

                this.Inform($"Тестирование приложения из {LNTestsCount} тестов:");

                while (LNi < LNTestsCount)
                {
                    this.Inform($"Тест № {LNi + 1}:");

                    LOWriter = new StringWriter();
                    Console.SetOut(LOWriter);

                    // подготовка
                    LLSrcFileExists = (Boolean)(LABeginData[LNi][LNLConstSrcFileExistsIndex]);
                    LLDestDirExists = (Boolean)(LABeginData[LNi][LNLConstDestDirExistsIndex]);

                    if (LLSrcFileExists)
                    {
                        // исходный файл должен существовать
                        LOSrcFile = new FileInfo((string)(LABeginData[LNi][LNLConstSrcPathIndex]));

                        if (!(Directory.Exists(LOSrcFile.Directory.FullName)))
                        {
                            // директория не существует
                            Directory.CreateDirectory(LOSrcFile.Directory.FullName);
                        }
                        else
                        {
                            // директория существует => удаляем и создаём
                            Directory.Delete(LOSrcFile.Directory.FullName, true);
                            Directory.CreateDirectory(LOSrcFile.Directory.FullName);
                        }

                        // создаём файл и пишем содержание
                        LOFWriter = File.CreateText(LOSrcFile.FullName);
                        LASrcContents = (List<string>)(LABeginData[LNi][LNLConstSrcContentsIndex]);

                        if (LASrcContents != null)
                        {
                            foreach (string str in LASrcContents)
                            { 
                                LOFWriter.WriteLine(str);
                            }
                        }

                        LOFWriter.Flush();
                        LOFWriter.Close();
                        LOFWriter.Dispose();
                        LOFWriter = null;

                    }
                    else
                    {
                        // исходный файл не должен существовать
                        LOSrcFile = new FileInfo((string)(LABeginData[LNi][LNLConstSrcPathIndex]));
                        if (Directory.Exists(LOSrcFile.Directory.FullName))
                        { 
                            Directory.Delete(LOSrcFile.Directory.FullName, true);
                        }

                    }   // исходный файл должен существовать или нет

                    // директория к файлу результата
                    if (LLDestDirExists)
                    {
                        LODestFile = new FileInfo((string)(LABeginData[LNi][LNLConstDestPathIndex]));
                        if (!(Directory.Exists(LODestFile.Directory.FullName)))
                        {
                            // директория не существует => создаём
                            Directory.CreateDirectory(LODestFile.Directory.FullName);
                        }
                        else
                        {
                            // директория существует => удаляем и создаём
                            Directory.Delete(LODestFile.Directory.FullName, true);
                            Directory.CreateDirectory(LODestFile.Directory.FullName);
                        }
                    }
                    else
                    {
                        // директория файла результата не должна существовать
                        LCDestFilePath = (string)(LABeginData[LNi][LNLConstDestPathIndex]);
                        if (LCDestFilePath.Length > 0)
                        {
                            LODestFile = new FileInfo(LCDestFilePath);
                            if (LODestFile.Directory != null)
                            {
                                if (Directory.Exists(LODestFile.Directory.FullName))
                                {
                                    Directory.Delete(LODestFile.Directory.FullName, true);
                                }
                            }
                        }
                    }   // директория пути к файлу результата должна существовать или не должна 

                    // вызов тестируемого метода
                    Program.Main((string[])(LABeginData[LNi][5]));

                    LCFactOutput = LOWriter.ToString();

                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));

                    LOWriter.Close();
                    LOWriter.Dispose();
                    LOWriter = null;

                    // сравнение результатов
                    LLTestIsOK = true;  // оптимистическое предположение

                    // ожидаемые результаты
                    LCExpectedOutput = (string)(LAExpectedData[LNi][0]);
                    LAExpectedMainDestContents = (List<string>)(LAExpectedData[LNi][1]);
                    LAExpectedAdditDestContents = (List<string>)(LAExpectedData[LNi][2]);
                    LLExpectedMainDestFileExists = (Boolean)(LAExpectedData[LNi][3]);
                    LLExpectedAdditDestFileExists = (Boolean)(LAExpectedData[LNi][4]);

                    LCMainDestFilePath = (string)(LABeginData[LNi][LNLConstDestPathIndex]);

                    if (LCMainDestFilePath.Length > 0)
                    {
                        LLFactMainDestFileExists = File.Exists(LCMainDestFilePath);
                    }
                    else
                    { 
                        LLFactMainDestFileExists = false;
                    }

                    if (LCMainDestFilePath.Length > 0)
                    {
                        if ((new FileInfo((string)(LABeginData[LNi][LNLConstDestPathIndex]))).Directory != null)
                        {
                            LCAdditDestFilePath = $@"{(new FileInfo((string)(LABeginData[LNi][LNLConstDestPathIndex]))).Directory.FullName}\problems.txt";
                            LLFactAdditDestFileExists = File.Exists(LCAdditDestFilePath);
                        }
                        else
                        {
                            LLFactAdditDestFileExists = false;
                        }
                    }
                    else
                    { 
                        LLFactAdditDestFileExists = false;
                    }

                    if (LLFactMainDestFileExists)
                    {
                        LAFactMainDestContents = new List<string>() { };
                        LOFReader = new StreamReader(LCMainDestFilePath);
                        while (!(LOFReader.EndOfStream))
                        {
                            LAFactMainDestContents.Add(LOFReader.ReadLine());
                        }
                        LOFReader.Close();
                        LOFReader.Dispose();
                        LOFReader = null;
                    }
                    else
                    {
                        LAFactMainDestContents = null;
                    }

                    // содержание файла проблем
                    if (LLFactAdditDestFileExists)
                    {
                        LAFactAdditDestContents = new List<string>() { };
                        LOFReader = new StreamReader(LCAdditDestFilePath);
                        while (!(LOFReader.EndOfStream))
                        {
                            LAFactAdditDestContents.Add(LOFReader.ReadLine());
                        }
                        LOFReader.Close();
                        LOFReader.Dispose();
                        LOFReader = null;
                    }
                    else
                    {
                        LAFactAdditDestContents = null;
                    }

                    if (LCFactOutput != LCExpectedOutput)
                    {
                        LLTestIsOK = false;
                        this.Inform(
                            $"Несовпадение выдачи программы: факт: {LCFactOutput}, ожидаемое: {LCExpectedOutput}");
                    }

                    if (LLFactMainDestFileExists != LLExpectedMainDestFileExists)
                    {
                        LLTestIsOK = false;
                        this.Inform(
                            $"Несовпадение признака существования файла основного результата: факт: {LLFactMainDestFileExists}, ожидаемое: {LLExpectedMainDestFileExists}");
                    }

                    if (LLFactAdditDestFileExists != LLExpectedAdditDestFileExists)
                    {
                        LLTestIsOK = false;
                        this.Inform(
                            $"Несовпадение признака существования файла проблем: факт: {LLFactAdditDestFileExists}, ожидаемое: {LLExpectedAdditDestFileExists}");
                    }

                    this.Inform($"Сравнение содержимого основного файла назначения:");
                    //LLTestIsOK
                    LLListsAreEqual = this.CompareStringLists(LAFactMainDestContents, LAExpectedMainDestContents, LNi);
                    if (!LLListsAreEqual)
                    {
                        LLTestIsOK = false;
                    }

                    this.Inform($"Сравнение содержимого основного файла проблем:");
                    //LLTestIsOK = 
                    LLListsAreEqual = this.CompareStringLists(LAFactAdditDestContents, LAExpectedAdditDestContents, LNi);
                    if (!LLListsAreEqual)
                    {
                        LLTestIsOK = false;
                    }

                    if (LLTestIsOK)
                    {
                        this.Inform($"Тест № {LNi + 1} успешен.");
                    }
                    else
                    {
                        this.Inform($"Тест № {LNi + 1} провален.");
                    }

                    

                    LNi += 1;
                }

            }
            catch (Exception e1)
            {
                this.Inform($"При тестировании приложения произошли ошибки: {e1.Message}");

                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));

            }   // конец попытки
        }   // метод тестирования . Окончание

    }   // класс тестов
}   // пространство тестов задачи 3
