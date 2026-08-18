using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task1
{
    public class Compressor :ICompressInterface
    {
        public List<string> GetError2Compress(string LCLParData)
        {
            Boolean LLErrorFound = false;
            string LCErrorDescr = "";

            List<string> LOResuLt = new List<string>() { };
            string LCRegexExpr = @"^[a-z]+$";

            if (!LLErrorFound)
            {
                try
                {
                    // если данные не соответствуют шаблону, это ошибка
                    if (LCLParData.Length > 0)
                    {
                        LLErrorFound = !(Regex.IsMatch(LCLParData, LCRegexExpr));

                        if (LLErrorFound)
                        {
                            LCErrorDescr = "Ошибка: строка для сжатия должна состоять из строчных букв латиницы";
                            LOResuLt.Add(LCErrorDescr);
                        }
                    }
                }
                catch (Exception e1)
                {
                    LOResuLt.Add(e1.Message);
                }   // конец попытки 
            }   // не было ошибки . Окончание

            return LOResuLt;
        }   // GetError2Compress . Окончание

        public Boolean CanCompress(string LCLParData)
        {
            return this.GetError2Compress(LCLParData).Count == 0;   // нет ошибок => можно сжать
        }   // может ли выполниться компрессия данных . Окончание

        public IExeResuLt Compress(string LCLParData)
        { 
            IExeResuLt LOResuLt = null;
            Boolean LLErrorFound = false;
            
            List<string> LAErrorDescrs = new List<string>() { };

            string LCPrevChar = " ";
            string LCCurrChar = "";
            int LNi = 0;    // перечислитель строки из начальных данных
            StringBuilder LOSB1 = new StringBuilder("");
            int LNDataLength = LCLParData.Length;
            int LNRepeatCount = 0;  // количество одинаковых символов

            if (!LLErrorFound)
            {
                try
                {
                    while (LNi < LNDataLength)
                    {
                        LCCurrChar = LCLParData.Substring(LNi, 1);
                        if (LCCurrChar != LCPrevChar)
                        {
                            // текущий символ не равен предыдущему
                            if (LCPrevChar == " ")
                            {
                                LNRepeatCount = 1;
                            }
                            else
                            {
                                // предыдущий символ - не пробел, а текущий символ не равен предыдущему
                                // => серия одинаковых символов закончилась
                                LOSB1.Append(LCPrevChar);
                                if (LNRepeatCount > 1)
                                {
                                    LOSB1.Append(LNRepeatCount.ToString());
                                }
                                LNRepeatCount = 1;
                            }   // предыдущий символ - пробел или другой символ

                            LCPrevChar = LCCurrChar;

                        }
                        else
                        {
                            // текущий символ равен предыдущему
                            LNRepeatCount += 1;
                        }   // текущий символ не равен предыдущему или равен предыдущему . Окончание

                        LNi += 1;
                        if (LNi == LNDataLength)
                        {
                            LOSB1.Append(LCCurrChar);
                            if (LNRepeatCount > 1)
                            {
                                LOSB1.Append(LNRepeatCount.ToString());
                            }
                        }   // текущий символ - последний . Окончание
                    }   // перебор по строке . Окончание
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LAErrorDescrs.Add(e1.Message);
                }   // конец попытки

            }   // не было ошибки . Окончание

            LOResuLt = new TExeResuLtStr(LLErrorFound, LAErrorDescrs, LOSB1.ToString());

            return LOResuLt;

        }   // метод сжатия . Окончание

    }   // класс для сжатия . Окончание

    // класс для развертки
    public class Decompressor : IDecompressInterface
    {
        public List<string> GetError2Decompress(string LCLParData)
        {
            Boolean LLErrorFound = false;
            string LCErrorDescr = "";

            List<string> LOResuLt = new List<string>() { };
            string LCRegex1 = @"^(?:[a-z]|(?:[a-z](?:\d+)))+$";
            string LCRegex2 = @"([a-z])\d*\1";
            Regex LOR2 = null;
            MatchCollection LAMatches = null;
            string LCRegex0 = @"^[a-z]";

            if (!LLErrorFound)
            {
                try
                {
                    if ((LCLParData.Length > 0) && (!Regex.IsMatch(LCLParData, LCRegex0)))
                    {
                        LLErrorFound = true;
                        LOResuLt.Add(
                            "Строка для развертки должна начинаться со строчной латинской буквы");
                    }
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LOResuLt.Add(e1.Message);
                }
            }   // не было ошибки . Окончание

            if (!LLErrorFound)
            {
                try
                {
                    if (LCLParData.Length > 0)
                    {
                        // если данные не соответствуют шаблону, это ошибка
                        LLErrorFound = !(Regex.IsMatch(LCLParData, LCRegex1));

                        if (LLErrorFound)
                        {
                            LCErrorDescr = "Ошибка: строка для декомпресии должна состоять из строчных букв латиницы и цифр";
                            LOResuLt.Add(LCErrorDescr);
                        }
                    }   // строка-данные для компрессии не пуста . Окончание
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LOResuLt.Add(e1.Message);
                }   // конец попытки 
            }   // не было ошибки . Окончание

            if (!LLErrorFound)
            {
                try
                {
                    if (LCLParData.Length > 0)
                    {
                        LOR2 = new Regex(LCRegex2);
                        LAMatches = LOR2.Matches(LCLParData);

                        if (LAMatches.Count > 0)
                        {
                            LLErrorFound = true;
                            LCErrorDescr =
                                $"Ошибка: найдены повторяющиеся подряд одинаковые буквы, позиция символа {LAMatches[0].Index + 1}";
                            LOResuLt.Add(LCErrorDescr);
                        }
                    }   // строка начальных данных не пуста . Окончание
                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LOResuLt.Add(e1.Message);
                }   // конец попытки
            }   // не было ошибки . Окончание

            return LOResuLt;
        }   // получить ошибки развертки . Окончание

        public Boolean CanDecompress(string LCLParData)
        {
            return this.GetError2Decompress(LCLParData).Count == 0; // нет ошибок => можно развернуть
        }   // может ли выполняться развертка данных . Окончание

        // развертка сжатых данных
        public IExeResuLt Decompress(string LCLParData)
        {
            char[] LAChars = null;
            Boolean LLErrorFound = false;
            string LCErrorDescr = "";
            List<string> LAErrorDescrs = new List<string>();
            IExeResuLt LOResuLt = null;

            int LNi = 0;    // перечислитель массива символов начальной строки
            int LNDataLength = 0;   // количество символов в строке
            char LCCurrSymboL = '\0';
            char LCSymboL2 = '\0';
            StringBuilder LOSB1 = null;
            StringBuilder LOSBDigits = null;    // строка для цифр
            int LNLettersCount = 0;

            if (!LLErrorFound)
            {
                try
                {
                    LAChars = LCLParData.ToCharArray();
                    LNDataLength = LAChars.Length;
                    LOSB1 = new StringBuilder("");

                    while (LNi < LNDataLength)
                    {
                        if (LNi == 0)
                        {
                            LCCurrSymboL = LAChars[LNi];
                        }

                        LNi += 1;
                        if (LNi == LNDataLength)
                        {
                            // строка закончилась
                            LOSB1.Append(LCCurrSymboL);
                            continue;
                        }

                        LCSymboL2 = LAChars[LNi];
                        if (char.IsDigit(LCSymboL2))
                        {
                            // символ2 - цифра
                            LOSBDigits = new StringBuilder("");
                            LOSBDigits.Append(LCSymboL2);

                            while (char.IsDigit(LCSymboL2) && (LNi < LNDataLength))
                            {
                                LNi += 1;
                                if (LNi < LNDataLength)
                                {
                                    // не конец строки
                                    LCSymboL2 = LAChars[LNi];
                                    if (char.IsDigit(LCSymboL2))
                                    {
                                        LOSBDigits.Append(LCSymboL2);
                                    }
                                    else
                                    {
                                        // другая буква
                                        LNLettersCount = int.Parse(LOSBDigits.ToString());
                                        LOSB1.Append("".PadRight(LNLettersCount, LCCurrSymboL));
                                        LOSBDigits = new StringBuilder("");
                                    }   // символ - цифра или буква . Окончание
                                }
                                else
                                {
                                    // конец строки, и символ2 - цифра
                                    LNLettersCount = int.Parse(LOSBDigits.ToString());
                                    LOSB1.Append("".PadRight(LNLettersCount, LCCurrSymboL));

                                }   // не конец строки или конец строки . Окончание

                            }   // конец перебора по подстроке из цифр
                        }
                        else
                        {
                            // символ2 - буква
                            LOSB1.Append(LCCurrSymboL);
                        }   // символ2 - цифра или буква . Окончание

                        if (LNi < LNDataLength)
                        {
                            LCCurrSymboL = LCSymboL2;
                        }
                    }   // конец перебора по символам начальной строки

                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LCErrorDescr = e1.Message;
                    LAErrorDescrs.Add(LCErrorDescr);

                }   // конец попытки
            }   // не было ошибки . Окончание

            LOResuLt = new TExeResuLtStr(LLErrorFound, LAErrorDescrs,
                LLErrorFound ? "" : LOSB1.ToString());

            return LOResuLt;
        }   // развёртка сжатых данных . Окончание

    }   // класс для развертки . Окончание

}   // пространство имен Task1
