using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class Task1VM : INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // IDataErrorInfo
        private string _error;
        public string this[string columnName]
        {
            get
            {
                List<string> LAErrors = null;
                switch (columnName)
                {
                    case nameof(LCData2Compress):
                        LAErrors = LOCompressor.GetError2Compress(LCData2Compress);
                        if (LAErrors.Count > 0)
                        {
                            this.ClearErrors(columnName);
                            this.AddErrors(columnName, LAErrors);
                            this.LLData2CompressHaveErrors = true;
                        }
                        else
                        {
                            // нет ошибок
                            this.LLData2CompressHaveErrors = false;
                            this.ClearErrors(columnName);

                        }
                        break;

                    case nameof(LCData2Decompress):
                        LAErrors = LODecompressor.GetError2Decompress(LCData2Decompress);
                        if (LAErrors.Count > 0)
                        {
                            this.ClearErrors(columnName);
                            this.AddErrors(columnName, LAErrors);
                            this.LLData2DecompressHaveErrors = true;
                        }
                        else
                        {
                            // нет ошибок
                            this.LLData2DecompressHaveErrors = false;
                            this.ClearErrors(columnName);

                        }
                        break;
                    default:
                        break;
                }
                return string.Empty;
            }
        }   // индексатор от IDataErrorInfo . Окончание

        // остановился здесь 07.04.2026 20:00
        public string Error
        {
            get { return _error; }
        }

        // INotifyDataErrorChanged
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private readonly Dictionary<string, List<string>> _errors =
            new Dictionary<string, List<string>>();
        public bool HasErrors => _errors.Count != 0;
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return _errors.Values;
            }
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }


        // начальные данные для сжатия
        private string _LCData2Compress;
        public string LCData2Compress
        {
            get => _LCData2Compress;
            set
            {
                _LCData2Compress = value;
                NotifyPropertyChanged("LCData2Compress");
            }
        }

        // начальные данные для развёртки
        private string _LCData2Decompress;
        public string LCData2Decompress
        {
            get => _LCData2Decompress;
            set
            {
                _LCData2Decompress = value;
                NotifyPropertyChanged("LCData2Decompress");
            }
        }

        // компрессор
        private ICompressInterface LOCompressor;

        // декомпрессор
        private IDecompressInterface LODecompressor;


        // признак ошибки в начальных данных для сжатия
        private Boolean _LLData2CompressHaveErrors;
        public Boolean LLData2CompressHaveErrors
        {
            get => _LLData2CompressHaveErrors;
            set
            {
                _LLData2CompressHaveErrors = value;
                NotifyPropertyChanged("LLData2CompressHaveErrors");
            }
        }

        // признак ошибки в начальных данных для развертки
        private Boolean _LLData2DecompressHaveErrors;
        public Boolean LLData2DecompressHaveErrors
        {
            get => _LLData2DecompressHaveErrors;
            set
            {
                _LLData2DecompressHaveErrors = value;
                NotifyPropertyChanged("LLData2DecompressHaveErrors");
            }
        }

        // результат сжатия данных
        private string _LCCompressedData;
        public string LCCompressedData
        {
            get => _LCCompressedData;
            set
            {
                _LCCompressedData = value;
                NotifyPropertyChanged("LCCompressedData");
            }
        }

        private string _LCDecompressedData;
        public string LCDecompressedData
        {
            get => _LCDecompressedData;
            set
            {
                _LCDecompressedData = value;
                NotifyPropertyChanged("LCDecompressedData");
            }
        }

        // кнопка для сжатия строки
        public CommandCompress LCommandCompress { get; }

        // кнопка для развертки строки
        public CommandDecompress LCommandDecompress { get; }

        // высота и ширина формы
        private double _FHeight;
        public double FHeight
        {
            get => _FHeight;
            set
            {
                _FHeight = value;
                NotifyPropertyChanged("FHeight");
            }
        }

        private double _FWidth;
        public double FWidth
        {
            get => _FWidth;
            set
            {
                _FWidth = value;
                NotifyPropertyChanged("FWidth");
            }
        }   // ширина формы

        // размер шрифта
        private double _LFontSize;
        public double LFontSize
        {
            get => _LFontSize;
            set
            {
                _LFontSize = value;
                NotifyPropertyChanged("LFontSize");
            }
        }

        // размер шрифта AdornedElement
        private double _LFAdorFontSize;
        public double LFAdorFontSize
        {
            get => _LFAdorFontSize;
            set
            {
                _LFAdorFontSize = value;
                NotifyPropertyChanged("LFAdorFontSize");
            }
        }


        // конструктор модели вида
        public Task1VM()
        {
            // размеры формы
            this.FHeight = 450;
            this.FWidth = 800;

            this.LFontSize = 12;
            this.LFAdorFontSize = 20;


            this.LOCompressor = new Compressor();
            this.LODecompressor = new Decompressor();

            this.LCData2Compress = "";
            this.LCData2Decompress = "";

            // пока нет ошибок
            this.LLData2CompressHaveErrors = false;
            this.LLData2DecompressHaveErrors = false;

            // результаты операций
            this.LCCompressedData = "";
            this.LCDecompressedData = "";

            // кнопки
            this.LCommandCompress = new CommandCompress(this);
            this.LCommandDecompress = new CommandDecompress(this);

        }

        // добавить ошибки
        private void AddErrors(string propertyName, IList<string> errors)
        {
            Boolean LLChanged = false;
            if (!_errors.ContainsKey(propertyName))
            {
                _errors.Add(propertyName, new List<string>());
                LLChanged = true;
            }

            foreach (string err in errors)
            {
                if (_errors[propertyName].Contains(err)) continue;
                _errors[propertyName].Add(err);
                LLChanged = true;
            }

            if (LLChanged)
            {
                OnErrorsChanged(propertyName);
            }
        }   // добавить ошибки . Окончание

        // добавить одну ошибку
        private void AddError(string propertyName, string error)
        {
            AddErrors(propertyName, new List<string> { error });
        }   // добавить одну ошибку . Окончание

        // убрать ошибки
        protected void ClearErrors(string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                _errors.Clear();
            }
            else
            {
                _errors.Remove(propertyName);
            }
            OnErrorsChanged(propertyName);
        }   // убрать ошибки . Окончание

        // метод модели - можно ли выполнить сжатие данных
        public Boolean CanCompress(string LCData)
        {
            return this.LOCompressor.CanCompress(LCData);
        }

        // метод модели вида - выполнить сжатие данных
        public void Compress(string LCData)
        {
            IExeResuLt LOResuLt = null; 
            TExeResuLtStr LOStrResuLt = null;

            Boolean LLErrorFound = false;   // признак ошибки
            string LCErrorDescr = "";

            try
            {
                LOResuLt = this.LOCompressor.Compress(LCData);
                LOStrResuLt = LOResuLt as TExeResuLtStr;

                
                if (LOResuLt.LLErrorFound)
                {
                    LLErrorFound = true;
                    LCErrorDescr = LOResuLt.ErrorsArrayToString();
                    this.LCCompressedData = "";
                }
                else
                {
                    // ошибки не было
                    this.LCCompressedData = LOStrResuLt.LCAim;
                }   // была ошибка или не было ошибки
            }
            catch (Exception e1)
            {
                LLErrorFound = true;
                LCErrorDescr = $"Ошибка {e1.Source}.{e1.TargetSite.Name}: {e1.Message}";
                this.LCCompressedData = "";
            }

            if (LLErrorFound)
            {
                System.Windows.MessageBox.Show(LCErrorDescr);
            }

        }   // метод модели вида - выполнить сжатие данных . Окончание

        // можно ли развернуть строку
        public Boolean CanDecompress(string LCData)
        {
            return this.LODecompressor.CanDecompress(LCData);
        }   // метод модели вида - можно ли развернуть строку

        public void Decompress(string LCData)
        {
            IExeResuLt LOResuLt = null;
            TExeResuLtStr LOStrResuLt = null;

            Boolean LLErrorFound = false;   // ошибки пока не было
            string LCErrorDescr = "";

            try
            {
                LOResuLt = this.LODecompressor.Decompress(LCData);
                LOStrResuLt = LOResuLt as TExeResuLtStr;

                if (LOResuLt.LLErrorFound)
                {
                    LLErrorFound = true;
                    LCErrorDescr = LOResuLt.ErrorsArrayToString();
                    this.LCDecompressedData = "";
                }
                else
                {
                    this.LCDecompressedData = LOStrResuLt.LCAim;
                }   // была ошибка или ошибки не было
            }
            catch (Exception e1)
            { 
                // ошибка
                LLErrorFound = true;
                LCErrorDescr = $"Ошибка {e1.Source}.{e1.TargetSite}: {e1.Message}";
                this.LCDecompressedData = "";
            }

            if (LLErrorFound)
            {
                System.Windows.MessageBox.Show(LCErrorDescr);
            }

        }   // метод модели вида - выполнить развертку строки

    }   // модель вида главной формы задачи 1 . Окончание
}
