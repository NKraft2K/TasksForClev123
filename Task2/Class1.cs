using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task2
{
    public static class Server1
    {
        private const int LNConstReaders = 0;
        private const int LNConstWriters = 1;

        private static object x;   // объект для синхронизации
        private static List<int> LAActionInfo;  // сведения о действиях
        private static int LNMilliSeconds;

        private static int count;

        // конструктор
        static Server1()
        {
            x = new object();
            LAActionInfo = new List<int>() { 0, 0 };    // пока никто не читает и не пишет
            LNMilliSeconds = 20;

            // разделяемое свойство
            count = 0;
        }

        // методы для чтения
        // получить доступ для читателя
        private static Boolean GetAccess2Reader()
        {
            Boolean LLAccessGranted = false;
            int LNWritersNumber = 0;    // количество писателей

            lock (x)
            {
                LNWritersNumber = Server1.LAActionInfo[LNConstWriters];

                // если писатель пишет, доступ запрещён, иначе - разрешён
                LLAccessGranted = LNWritersNumber > 0 ? false : true;
                if (LLAccessGranted)
                {
                    // занимаем доступ для чтения
                    Server1.LAActionInfo[LNConstReaders] += 1;
                }
            }   // критическая секция . Окончание

            return LLAccessGranted;

        }   // получить доступ для читателя . Окончание


        // после чтения
        private static void FinishReading()
        {
            lock (x)
            {
                Server1.LAActionInfo[LNConstReaders] -= 1;
            }
        }   // закончить чтение . Окончание

        // метод чтения
        public static int GetCount()
        {
            int LNResuLt = 0;
            Boolean LLAccessGranted = false;    // предположим, доступ запрещён

            while (!LLAccessGranted)
            {
                LLAccessGranted = Server1.GetAccess2Reader();
                if (!LLAccessGranted)
                {
                    // ожидаем, если доступ получить не удалось
                    Thread.Sleep(Server1.LNMilliSeconds);
                }
            }

            LNResuLt = Server1.count;
            Server1.FinishReading();

            return LNResuLt;

        }   // метод чтения . Окончание

        // получить доступ для писателя
        private static Boolean GetAccess2Writer()
        {
            Boolean LLAccessGranted = false;
            int LNActingCount = 0;  // количество действующих запросов

            lock (x)
            {
                LNActingCount = Server1.LAActionInfo[LNConstReaders] + Server1.LAActionInfo[LNConstWriters];

                // если никто не читает и не пишет, доступ разрешён
                LLAccessGranted = LNActingCount > 0 ? false : true;

                // захватываем доступ
                if (LLAccessGranted)
                {
                    Server1.LAActionInfo[LNConstWriters] += 1;
                }
            }   // критическая секция . Окончание

            return LLAccessGranted;
        }   // получить доступ для писателя . Окончание

        // закончить запись
        private static void FinishWriting()
        {
            lock (x)
            {
                Server1.LAActionInfo[LNConstWriters] -= 1;
            }
        }   // закончить запись . Окончание

        // метод записи
        public static void AddToCount(int value)
        {
            Boolean LLAccessGranted = false;    // доступ изначально отсутствует

            while (!LLAccessGranted)
            {
                LLAccessGranted = Server1.GetAccess2Writer();
                if (!LLAccessGranted)
                {
                    Thread.Sleep(Server1.LNMilliSeconds);
                }
            }   // цикл получения доступа на запись . Окончание

            // доступ получен
            Server1.count += value;

            // освобождаем доступ к разделяемому ресурсу
            Server1.FinishWriting();

        }   // метод записи . Окончание

    }   // класс Server1 . Окончание

    
}
