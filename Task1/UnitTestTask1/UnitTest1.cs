using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Task1;

namespace UnitTestTask1
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }
        public const string ErrMsg0 = "Строка для развертки должна начинаться со строчной латинской буквы";
        public const string ErrMsg1 = "Ошибка: строка для сжатия должна состоять из строчных букв латиницы";
        public const string ErrMsg2 = "Ошибка: строка для декомпресии должна состоять из строчных букв латиницы и цифр";
        public const string ErrMsg3_1 = "Ошибка: найдены повторяющиеся подряд одинаковые буквы, позиция символа 5";

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

            return LLResuLt;
        }   // сравнение списков строк


        // тестирование метода 
        [TestMethod]
        public void TestGetError2Compress()
        {
            // список ошибок начальных данных для сжатия строки
            List<string> LABeginData = null;
            List<List<string>> LAExpectedResuLts = null;
            int LNi = 0;
            int LNCallsCount = 0;

            string LCData = null;
            List<string> LAResuLt = null;
            List<string> LAExpectedResuLt = null;
            ICompressInterface LOTestObj = null;
            Boolean LLListsAreEqual = false;

            // определим начальные данные


            try
            {
                LABeginData = new List<string>()
                {
                    "ekuu",
                    "Ekuu",
                    "Бkgg",
                    "<kgg",
                    ""
                };  // 5 строк

                // определим ожидаемый результат
                LAExpectedResuLts = new List<List<string>>()
                {
                    new List<string>(){ },
                    new List<string>(){ ErrMsg1 },
                    new List<string>(){ ErrMsg1 },
                    new List<string>(){ ErrMsg1 },
                    new List<string>(){ }
                };  // 5 списков


                LNCallsCount = LABeginData.Count;
                LOTestObj = new Compressor();


                this.Inform("Начато тестирование Compressor.GetError2Compress");

                while (LNi < LNCallsCount)
                {
                    LCData = LABeginData[LNi];
                    LAExpectedResuLt = LAExpectedResuLts[LNi];

                    LAResuLt = LOTestObj.GetError2Compress(LCData); // получили фактический результат

                    LLListsAreEqual = this.CompareStringLists(LAResuLt, LAExpectedResuLt, LNi + 1);
                    if (LLListsAreEqual)
                    {
                        this.Inform($"Тест № {LNi + 1} ошибок не выявил");
                    }
                    LNi += 1;
                }   // цикл по тестам GetError2Compress . Окончание
            }
            catch (Exception e1)
            {
                this.Inform($"Выполнения теста Compressor.GetError2Compress вызвало исключение {e1.Message}");
            }

        }   // тестирование получения ошибок при сжатии строки . Окончание

        // тестирование Boolean CanCompress(string LCLParData);
        [TestMethod]
        public void TestCanCompress()
        {
            List<string> LABeginData = null;    // начальные данные
            List<Boolean> LAExpectedResuLts = null;

            int LNi = 0;    // перечислитель начальных данных
            int LNTestCount = 0;    // мощность начальных данных
            ICompressInterface LOTestObj1 = null;
            string LCData = null;
            Boolean LLResuLt = false;
            Boolean LLExpectedResuLt = false;

            // определим начальные данные
            try
            {
                LABeginData = new List<string>()
                {
                    "ekuu",
                    "Ekuu",
                    "Бkgg",
                    "<kgg",
                    ""
                };  // 5 строк

                // определим ожидаемый результат
                LAExpectedResuLts = new List<bool>()
                {
                    true,
                    false,
                    false,
                    false,
                    true
                };  // 5 значений


                LNTestCount = LABeginData.Count;
                LOTestObj1 = new Compressor();

                this.Inform("Тестирование метода Compressor.CanCompress начато");

                while (LNi < LNTestCount)
                {
                    LCData = LABeginData[LNi];
                    LLExpectedResuLt = LAExpectedResuLts[LNi];

                    LLResuLt = LOTestObj1.CanCompress(LCData);
                    if (LLResuLt != LLExpectedResuLt)
                    {
                        this.Inform(
                            $"Тест № {LNi + 1} обнаружил ошибку. Результат {LLResuLt}, ожидалось {LLExpectedResuLt}");
                    }
                    else
                    {
                        this.Inform($"Тест № {LNi + 1} ошибок не выявил");
                    }

                    LNi += 1;
                }
            }
            catch (Exception e1)
            {
                this.Inform($"Выполнения теста Compressor.CanCompress вызвало исключение {e1.Message}");
            }   // конец попытки

        }   // тестирование CanCompress . Окончание

        [TestMethod]
        public void TestCompress()
        {
            List<string> LABeginData = null;
            List<IExeResuLt> LAExpectedResuLts = null;

            Compressor LOTestObj1 = null;
            int LNi = 0;
            int LNTestCount = 0;
            string LCData = null;
            IExeResuLt LOExpectedResuLt = null;

            IExeResuLt LOResuLt = null;
            Boolean LLTestSuccess = true;   // предполагаем, что ошибок нет
            Boolean LLListsAreEqual = false;


            try
            {
                // определим начальные данные
                LABeginData = new List<string>()
                {
                    "ekuu",
                    "aassssuue",
                    "ajhhhhhhhhhhhhht",
                    ""
                };

                // определим ожидаемый результат
                LAExpectedResuLts = new List<IExeResuLt>()
                {
                    new TExeResuLtStr(false, new List<string>(){ }, "eku2"),
                    new TExeResuLtStr(false, new List<string>(){ }, "a2s4u2e"),
                    new TExeResuLtStr(false, new List<string>(){ }, "ajh13t"),
                    new TExeResuLtStr(false, new List<string>(){ }, "")
                };


                LOTestObj1 = new Compressor();
                LNTestCount = LABeginData.Count;

                this.Inform("Тестирование метода Compressor.Compress начато");

                while (LNi < LNTestCount)
                {
                    LCData = LABeginData[LNi];
                    LOExpectedResuLt = LAExpectedResuLts[LNi];
                    LLTestSuccess = true;

                    LOResuLt = LOTestObj1.Compress(LCData);

                    if (LOResuLt.LLErrorFound != LOExpectedResuLt.LLErrorFound)
                    {
                        this.Inform(
                            $"Тест № {LNi + 1}: найдено несовпаднение IExeResuLt.LLErrorFound: Результат {LOResuLt.LLErrorFound}, ожидалось {LOExpectedResuLt.LLErrorFound}");
                        LLTestSuccess = false;
                    }

                    if ((LOResuLt as TExeResuLtStr).LCAim != 
                        (LOExpectedResuLt as TExeResuLtStr).LCAim)
                    {
                        this.Inform(
                            $"Тест № {LNi + 1}: найдено несовпадение TExeResuLtStr.LCAim: Результат \"{(LOResuLt as TExeResuLtStr).LCAim}\", ожидалось \"{(LOExpectedResuLt as TExeResuLtStr).LCAim}\"");
                        LLTestSuccess = false;
                    }

                    LLListsAreEqual =
                        this.CompareStringLists(LOResuLt.LAErrors, LOExpectedResuLt.LAErrors, LNi + 1);
                    if (!LLListsAreEqual)
                    {
                        LLTestSuccess = false;
                    }

                    if (LLTestSuccess)
                    {
                        this.Inform($"Тест № {LNi + 1} ошибок не выявил");
                    }

                    LNi += 1;
                }   // цикл перебора по данным для тестирования . Окончание

            }
            catch (Exception e1)
            {
                this.Inform($"Выполнения теста Compressor.Compress вызвало исключение {e1.Message}");
            }   // конец попытки

        }   // тестирование метода сжатия . Окончание

        // тестирование класса развертки сжатой строки
        // тестирование метода List<string> GetError2Decompress(string LCLParData);
        [TestMethod]
        public void TestGetError2Decompress()
        {
            List<string> LABeginData = null;
            List<List<string>> LAExpectedResuLts = null;
            int LNi = 0;
            int LNCallsCount = 0;

            string LCData = null;
            List<string> LAResuLt = null;
            List<string> LAExpectedResuLt = null;
            IDecompressInterface LOTestObj = null;
            Boolean LLListsAreEqual = false;

            try
            {
                LABeginData = new List<string>()
                {
                    "g2e",
                    "G2e",
                    @"g5<u",
                    "y56wh781h",
                    ""
                };  // 5 строк

                LAExpectedResuLts = new List<List<string>>()
                {
                    new List<string>(){ },
                    new List<string>(){ ErrMsg0 },
                    new List<string>(){ ErrMsg2 },
                    new List<string>(){ ErrMsg3_1 },
                    new List<string>(){ }
                };  // 5 элементов

                LNCallsCount = LABeginData.Count;
                LOTestObj = new Decompressor();


                this.Inform("Начато тестирование Decompressor.GetError2Decompress");

                while (LNi < LNCallsCount)
                {
                    LCData = LABeginData[LNi];
                    LAExpectedResuLt = LAExpectedResuLts[LNi];

                    LAResuLt = LOTestObj.GetError2Decompress(LCData); // получили фактический результат

                    LLListsAreEqual = this.CompareStringLists(LAResuLt, LAExpectedResuLt, LNi + 1);
                    if (LLListsAreEqual)
                    {
                        this.Inform($"Тест № {LNi + 1} ошибок не выявил");
                    }
                    LNi += 1;
                }   // цикл по тестам GetError2Decompress . Окончание

            }
            catch (Exception e1)
            {
                this.Inform($"Выполнения теста GetError2Decompress вызвало исключение {e1.Message}");

            }   // конец попытки

        }   // тестирование метода List<string> Decompressor.GetError2Decompress(string LCLParData); . Окончание

        [TestMethod]
        public void TestCanDecompress()
        {
            List<string> LABeginData = null;    // начальные данные
            List<Boolean> LAExpectedResuLts = null;

            int LNi = 0;    // перечислитель начальных данных
            int LNTestCount = 0;    // мощность начальных данных
            IDecompressInterface LOTestObj1 = null;
            string LCData = null;
            Boolean LLResuLt = false;
            Boolean LLExpectedResuLt = false;


            try
            {
                LABeginData = new List<string>()
                {
                    "g2e",
                    "G2e",
                    @"g5<u",
                    "y56wh781h",
                    ""
                };  // 5 элементов

                LAExpectedResuLts = new List<bool>
                {
                    true,
                    false,
                    false,
                    false,
                    true
                };  // 5 элементов 

                LNTestCount = LABeginData.Count;
                LOTestObj1 = new Decompressor();

                this.Inform("Тестирование метода Decompressor.CanDecompress начато");

                while (LNi < LNTestCount)
                {
                    LCData = LABeginData[LNi];
                    LLExpectedResuLt = LAExpectedResuLts[LNi];

                    LLResuLt = LOTestObj1.CanDecompress(LCData);
                    if (LLResuLt != LLExpectedResuLt)
                    {
                        this.Inform(
                            $"Тест № {LNi + 1} обнаружил ошибку. Результат {LLResuLt}, ожидалось {LLExpectedResuLt}");
                    }
                    else
                    {
                        this.Inform($"Тест № {LNi + 1} ошибок не выявил");
                    }

                    LNi += 1;
                }

            }
            catch (Exception e1)
            {
                this.Inform($"Выполнение теста Decompressor.CanDecompress вызвало исключение {e1.Message}");
            }   // конец попытки

        }   // тестирование Boolean CanDecompress(string LCLParData); . Окончание


        // тестирование метода развертки
        [TestMethod]
        public void TestDecompress()
        {
            List<string> LABeginData = null;
            List<IExeResuLt> LAExpectedResuLts = null;

            IDecompressInterface LOTestObj1 = null;
            int LNi = 0;
            int LNTestCount = 0;
            string LCData = null;
            IExeResuLt LOExpectedResuLt = null;

            IExeResuLt LOResuLt = null;
            Boolean LLTestSuccess = true;   // предполагаем, что ошибок нет
            Boolean LLListsAreEqual = false;

            try
            {
                LABeginData = new List<string>()
                {
                    "g2e",
                    "at15u3",
                    "at4ue",
                    "e",
                    "at15",
                    ""
                };  // 6 элементов

                LAExpectedResuLts = new List<IExeResuLt>()
                {
                    new TExeResuLtStr(false, new List<string>(){ }, "gge"),
                    new TExeResuLtStr(false, new List<string>(){ }, "atttttttttttttttuuu"),
                    new TExeResuLtStr(false, new List<string>(){ }, "attttue"),
                    new TExeResuLtStr(false, new List<string>(){ }, "e"),
                    new TExeResuLtStr(false, new List<string>(){ }, "attttttttttttttt"),
                    new TExeResuLtStr(false, new List<string>(){ }, "")
                };  // 6 элементов

                LOTestObj1 = new Decompressor();
                LNTestCount = LABeginData.Count;

                this.Inform("Тестирование метода Decompressor.Decompress начато");

                while (LNi < LNTestCount)
                {
                    LCData = LABeginData[LNi];
                    LOExpectedResuLt = LAExpectedResuLts[LNi];
                    LLTestSuccess = true;

                    LOResuLt = LOTestObj1.Decompress(LCData);

                    if (LOResuLt.LLErrorFound != LOExpectedResuLt.LLErrorFound)
                    {
                        this.Inform(
                            $"Тест № {LNi + 1}: найдено несовпадение IExeResuLt.LLErrorFound: Результат {LOResuLt.LLErrorFound}, ожидалось {LOExpectedResuLt.LLErrorFound}");
                        LLTestSuccess = false;
                    }

                    if ((LOResuLt as TExeResuLtStr).LCAim != (LOExpectedResuLt as TExeResuLtStr).LCAim)
                    {
                        this.Inform(
                            $"Тест № {LNi + 1}: найдено несовпадение TExeResuLtStr.LCAim: Результат \"{(LOResuLt as TExeResuLtStr).LCAim}\", ожидалось \"{(LOExpectedResuLt as TExeResuLtStr).LCAim}\"");
                        LLTestSuccess = false;
                    }

                    LLListsAreEqual =
                        this.CompareStringLists(LOResuLt.LAErrors, LOExpectedResuLt.LAErrors, LNi + 1);
                    if (!LLListsAreEqual)
                    {
                        LLTestSuccess = false;
                    }

                    if (LLTestSuccess)
                    {
                        this.Inform($"Тест № {LNi + 1} ошибок не выявил");
                    }

                    LNi += 1;
                }   // цикл перебора по данным для тестирования . Окончание

            }
            catch (Exception e1)
            {
                this.Inform($"Выполнения теста Decompressor.Decompress вызвало исключение {e1.Message}");

            }   // конец попытки

        }   // тестирование метода Decompress . Окончание

    }   // класс тестов . Окончание
}   // пространство имен тестов Task1
