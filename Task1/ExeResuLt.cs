using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public interface IExeResuLt
    {
        Boolean LLErrorFound { get; }
        List<string> LAErrors { get; }
        string ErrorsArrayToString();
    }

    // класс-результат выполнения
    public class TExeResuLtStr : IExeResuLt
    { 
        public Boolean LLErrorFound { get;}
        public List<string> LAErrors { get; }
        public string LCAim { get; }

        public TExeResuLtStr(Boolean LLLParErrorFound, List<string> LALParErrors,
            string LCLParAim)
        { 
            this.LLErrorFound = LLLParErrorFound;
            this.LAErrors = LALParErrors;
            this.LCAim = LCLParAim;
        }

        public string ErrorsArrayToString()
        {
            StringBuilder LOSB1 = new StringBuilder("");
            int LNi = 0;
            int LNErrorsCount = this.LAErrors.Count;

            while (LNi < LNErrorsCount)
            {
                if (LNi == 0)
                {
                    LOSB1.Append($"Ошибка №{LNi + 1}: {this.LAErrors[LNi]}");
                }
                else
                {
                    LOSB1.Append("\n\r");
                    LOSB1.Append($"Ошибка №{LNi + 1}: {this.LAErrors[LNi]}");
                }
                LNi += 1;
            }

            return LOSB1.ToString();
        }   // список ошибок преобразовать в одну строку . Окончание
    }   // TExeResuLtStr . Окончание
}
