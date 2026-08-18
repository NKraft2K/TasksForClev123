using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    // для сжатия
    public interface ICompressInterface
    {
        List<string> GetError2Compress(string LCLParData);    // проверка на соответствие формату начальных данных
        Boolean CanCompress(string LCLParData); // можно ли произвести сжатие
        IExeResuLt Compress(string LCLParData); // получить сжатые данные

    }

    public interface IDecompressInterface
    {
        List<string> GetError2Decompress(string LCLParData);  // проверка на соответствие формату начальных данных
        Boolean CanDecompress(string LCLParData);   // можно ли произвести развертку
        IExeResuLt Decompress(string LCLParData);   // получить развёрнутые данные
    }

}   // пространство имён Task1 . Окончание
