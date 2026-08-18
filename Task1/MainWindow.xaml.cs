using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Task1VM LOVM1 = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // форма загружена
            this.LOVM1 = new Task1VM();
            this.DataContext = this.LOVM1;

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // при изменении размеров
            if (this.LOVM1 != null)
            {
                double _LFOLDH = e.PreviousSize.Height;
                double _LFOLDW = e.PreviousSize.Width;

                double koefX = (e.NewSize.Width) / _LFOLDW;
                double koefY = (e.NewSize.Height) / _LFOLDH;

                double LFFontKoef = Math.Sqrt(((koefX * koefX) + (koefY * koefY)) / 2);

                double LFNewH = e.NewSize.Height;
                double LFNewW = e.NewSize.Width;

                double LFNewFontSize = this.LOVM1.LFontSize * LFFontKoef;
                double LFNewAdorFontSize = this.LOVM1.LFAdorFontSize * LFFontKoef;

                this.LOVM1.FHeight = LFNewH;
                this.LOVM1.FWidth = LFNewW;
                this.LOVM1.LFontSize = LFNewFontSize;
                this.LOVM1.LFAdorFontSize = LFNewAdorFontSize;
            }   // модель вида не null . Окончание
        }   // после изменения размера

        private void Window_Initialized(object sender, EventArgs e)
        {
            // при открытии
            const double LFDefaultWidth = 2560;
            const double LFDefaultHeight = 1600;

            double LFRealWidth = 0;
            double LFRealHeight = 0;

            double LFkoefX = 0;
            double LFkoefY = 0;

            double LFNewW = 0;
            double LFNewH = 0;

            try
            {
                LFRealWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                LFRealHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
                if (this.LOVM1 != null)
                {
                    if ((LFRealWidth != LFDefaultWidth) || (LFRealHeight != LFDefaultHeight))
                    {
                        LFkoefX = LFRealWidth / LFDefaultWidth;
                        LFkoefY = LFRealHeight / LFDefaultHeight;

                        LFNewW = this.LOVM1.FWidth * LFkoefX;
                        LFNewH = this.LOVM1.FHeight * LFkoefY;

                        this.LOVM1.FWidth = LFNewW;
                        this.LOVM1.FHeight = LFNewH;

                    }
                }   // модель вида не null . Окончание
            }
            catch (Exception e1)
            {
                System.Windows.MessageBox.Show(e1.Message);
            }

        }   // при создании


    }   // главная форма . Окончание
}
