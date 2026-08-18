using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Task1
{
    [ValueConversion(typeof(object), typeof(string))]
    internal class ConvToBooL : IValueConverter
    {

        // из модели в видимое представление модели (на форму)
        public object Convert(
         object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            Boolean x1 = (Boolean)System.Convert.ChangeType(value, typeof(Boolean));
            string LCResuLt = "";

            //    System.Windows.MessageBox.Show($"Значение {x1}");
            LCResuLt = x1 ? "True" : "False";

            return LCResuLt;
        }

        public object ConvertBack(
         object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            string LCVal = value.ToString();    // строка в булево
            return LCVal == "False" ? (bool)false : (bool)true;
        }
    }   // из булева на форму . Окончание

    [ValueConversion(typeof(object), typeof(string))]
    public class DBLConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double x = (double)value;
            NumberFormatInfo LOFormat = new NumberFormatInfo();
            LOFormat.NumberGroupSeparator = "";
            LOFormat.NumberDecimalSeparator = ".";

            return x.ToString("#####.######", LOFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double x = (double)value;

            return x;
        }
    }   // конвертация из double на форму
}   // пространство имён Task1
