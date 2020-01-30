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
using XMLParse;

namespace NbpCurrencyCoursesWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            var currencyCode = Currency.Text;
            var startDate = StartDate.SelectedDate;
            var endDate = EndDate.SelectedDate;

            try
            {
                if (!startDate.HasValue)
                {
                    throw new Exception("Niepoprawna data początku zakresu");
                }

                if (!endDate.HasValue)
                {
                    throw new Exception("Niepoprawna data końca zakresu");
                }

                if (startDate > endDate)
                {
                    throw new Exception("Data początku zakresu nie może być większa niż data końca zakresu");
                }
                Results.Text = "Pobieranie danych...";

                var couresData = NbpCurrenciesDataFinder.GetCurrencyCoursesData(currencyCode, startDate.Value, endDate.Value);
                var currencyCourseCalculator = new CurrencyCourseCalculator(couresData);

                sb.AppendLine($"Kurs średni sprzedaży {currencyCourseCalculator.CalculateSellRatesAverage()}");
                sb.AppendLine($"Odchylenie standardowe sprzedaży {currencyCourseCalculator.CalculateSellRatesStandardDeviation()}");
                sb.AppendLine($"Kurs maksymalny sprzedaży {currencyCourseCalculator.CalculateSellRatesMaxValue()}");
                sb.AppendLine($"Kurs minimalny sprzedaży {currencyCourseCalculator.CalculateSellRatesMinValue()}");

                sb.AppendLine();

                sb.AppendLine($"Kurs średni kupna {currencyCourseCalculator.CalculateBuyRatesAverage()}");
                sb.AppendLine($"Odchylenie standardowe kupna {currencyCourseCalculator.CalculateBuyRatesStandardDeviation()}");
                sb.AppendLine($"Kurs maksymalny kupna {currencyCourseCalculator.CalculateBuyRatesMaxValue()}");
                sb.AppendLine($"Kurs minimalny kupna {currencyCourseCalculator.CalculateBuyRatesMinValue()}");

                var courseDifference = currencyCourseCalculator.CalculateMaxCurseDifference();
                sb.AppendLine($"Największa różnica między kursem sprzedaży i kupna wystąpiła dnia {courseDifference.Day} wynosiła {courseDifference.Difference}");

                Results.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Results.Text = ex.Message;
            }
        }
    }
}
