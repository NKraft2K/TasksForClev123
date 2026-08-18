using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Task3
{

    public interface IExeResuLt
    {
        Boolean LLErrorFound { get; }
        IList<string> LAErrors { get; }

    }

    public interface ISplitResuLt : IExeResuLt
    { 
        Boolean LLStringTypeIsFound { get; }
        IList<string> LAStringParts { get; }
    }

    internal class TSpLitResuLt : ISplitResuLt
    { 
        public Boolean LLErrorFound { get; }
        public IList<string> LAErrors { get; }
        public Boolean LLStringTypeIsFound { get; }
        public IList<string> LAStringParts { get; }

        public TSpLitResuLt(bool lLErrorFound, IList<string> lAErrors, bool lLStringTypeIsFound, IList<string> lAStringParts)
        {
            LLErrorFound = lLErrorFound;
            LAErrors = lAErrors;
            LLStringTypeIsFound = lLStringTypeIsFound;
            LAStringParts = lAStringParts;
        }
    }   // результат разбивки строки . Окончание

    public interface IFileFormatType
    {
        ISplitResuLt SplitString(string LCStr1);
    }
    internal class FileFormatT1 : IFileFormatType
    {
        private string LCRegex;
        public FileFormatT1()
        {
            this.LCRegex =
                @"(?i)^(\d{2}\.\d{2}\.\d{4})\s(\d{2}\:\d{2}\:\d{2}(?:\.\d+)?)\s(INFORMATION|WARNING|DEBUG|ERROR)\s(.*)$";
        }

        public ISplitResuLt SplitString(string LCLParStr1)
        {
            ISplitResuLt LOResuLt = null;

            Match LOMatch = null;
            Boolean LLErrorFound = false;
            string LCErrorDescr = "";
            List<string> LAErrorDescrs = new List<string>() { };
            Boolean LLFormatIsFound = false;
            List<string> LAFields = null;   // поля записи
            string LCDate = null;
            string LCTime = null;
            string LCLogLeveL = null;
            string LCMethod = null;
            string LCMessage = null;
            Boolean LLChoiceIsMade = false;

            if (!LLErrorFound)
            {
                try
                {

                    LOMatch = Regex.Match(LCLParStr1, this.LCRegex);
                    LLFormatIsFound = LOMatch.Success;
                    if (LLFormatIsFound)
                    {
                        LCDate = LOMatch.Groups[1].Value.Replace(".", "-");
                        LCTime = LOMatch.Groups[2].Value;
                        LCLogLeveL = LOMatch.Groups[3].Value;

                        if (!LLChoiceIsMade)
                        {
                            if (Regex.IsMatch(LCLogLeveL, @"(?i)INFORMATION"))
                            {
                                LCLogLeveL = "INFO";
                                LLChoiceIsMade = true;
                            }
                        }

                        if (!LLChoiceIsMade)
                        {
                            if (Regex.IsMatch(LCLogLeveL, @"(?i)WARNING"))
                            {
                                LCLogLeveL = "WARN";
                                LLChoiceIsMade = true;
                            }
                        }   // уровень логирования пока не определён . Окончание

                        if (!LLChoiceIsMade)
                        {
                            if (Regex.IsMatch(LCLogLeveL, @"(?i)DEBUG"))
                            {
                                LCLogLeveL = "DEBUG";
                                LLChoiceIsMade = true;
                            }
                        }   // уровень логирования пока не определён . Окончание

                        if (!LLChoiceIsMade)
                        {
                            if (Regex.IsMatch(LCLogLeveL, @"(?i)ERROR"))
                            {
                                LCLogLeveL = "ERROR";
                                LLChoiceIsMade = true;
                            }
                        }   // уровень логирования пока не определён . Окончание

                        LCMethod = "DEFAULT";
                        LCMessage = LOMatch.Groups[4].Value;

                        LAFields = new List<string>()
                            {
                                LCDate, LCTime, LCLogLeveL, LCMethod, LCMessage
                            };

                    }
                    else
                    {
                        // строка не соответствует формату
                        LAFields = new List<string>() { };

                    }   // строка соответствует формату или нет

                    LOResuLt = new TSpLitResuLt(
                        LLErrorFound,
                        new List<string>() { },
                        LLFormatIsFound,
                        LAFields // формат не найден => проблема
                        );

                }   
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LCErrorDescr = e1.Message;
                    LAErrorDescrs = new List<string>() { LCErrorDescr };

                    LOResuLt = new TSpLitResuLt(
                        LLErrorFound,
                        LAErrorDescrs,
                        false,
                        new List<string>() { }
                        );
                }   // конец попытки
            }   // не было ошибки . Окончание

            return LOResuLt;
        }   // метод "разбить строку" . Окончание

    }   // первый тип строки . Окончание

    // второй тип строки
    public class FileFormatT2 : IFileFormatType
    {
        private string LCRegex;
        public FileFormatT2()
        {
            this.LCRegex =
                @"(?i)^(\d{4}\-\d{2}\-\d{2})\s(\d{2}\:\d{2}\:\d{2}(?:\.\d+)?)\|\s*(INFO|WARN|DEBUG|ERROR)\|(?:\d*)\|((?:[^\|])*)\|(.*)$";
        }

        public ISplitResuLt SplitString(string LCLParStr1)
        {
            ISplitResuLt LOResuLt = null;

            Match LOMatch = null;
            Boolean LLErrorFound = false;
            string LCErrorDescr = "";
            List<string> LAErrorDescrs = new List<string>() { };
            Boolean LLFormatIsFound = false;
            List<string> LAFields = null;   // поля записи
            string LCDate = null;
            string LCTime = null;
            string LCLogLeveL = null;
            string LCMethod = null;
            string LCMessage = null;

            string LCDateF2Regex = @"^(\d{4})\-(\d{2})\-(\d{2})$";
            Match LODateF2Match = null;


            if (!LLErrorFound)
            {
                try
                {
                    LOMatch = Regex.Match(LCLParStr1, this.LCRegex);
                    LLFormatIsFound = LOMatch.Success;

                    if (LLFormatIsFound)
                    {
                        LCDate = LOMatch.Groups[1].Value;
                        LODateF2Match = Regex.Match(LCDate, LCDateF2Regex);
                        LCDate =
                            $"{LODateF2Match.Groups[3].Value}-{LODateF2Match.Groups[2].Value}-{LODateF2Match.Groups[1].Value}";

                        LCTime = LOMatch.Groups[2].Value;
                        LCLogLeveL = LOMatch.Groups[3].Value;
                        LCMethod =
                            LOMatch.Groups[4].Value.Trim().Length > 0 ?
                                LOMatch.Groups[4].Value.Trim() : "DEFAULT";

                        LCMessage = LOMatch.Groups[5].Value;

                        LAFields = new List<string>()
                            {
                                LCDate, LCTime, LCLogLeveL, LCMethod, LCMessage
                            };

                    }
                    else
                    {
                        LAFields = new List<string>() { };
                    }   // формат определён или не определён

                    LOResuLt = new TSpLitResuLt(
                        LLErrorFound,
                        new List<string>() { },
                        LLFormatIsFound,
                        LAFields 
                        );

                }
                catch (Exception e1)
                {
                    LLErrorFound = true;
                    LCErrorDescr = e1.Message;
                    LAErrorDescrs = new List<string>() { LCErrorDescr };

                    LOResuLt = new TSpLitResuLt(
                        LLErrorFound,
                        LAErrorDescrs,
                        false,  // тип не найден
                        new List<string>() { }
                        );
                }   // конец попытки

            }   // не было ошибки . Окончание

            return LOResuLt;
        }   // метод "разбить строку" для типа 2 . Окончание
    }   // тип файла 2 . Окончание

    public static class TypeConstants
    { 
        public static readonly ReadOnlyCollection<Type> LAStrTypes =
            new ReadOnlyCollection<Type>(new List<Type>() 
            { typeof(FileFormatT1), typeof(FileFormatT2) });
    }

}   // пространство имён задача 3 . Окончание
