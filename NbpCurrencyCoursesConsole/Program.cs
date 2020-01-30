using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLParse
{
    class Program
    {
        private static List<string> AllowedCurrencyCodes = new List<string>
            {
              "USD",
              "EUR",
              "CHF",
              "GBP",
            };

        static void Main(string[] args)
        {
            try
            {
                if (args is null || args.Count() < 3)
                {
                    throw new Exception("Zbyt mało parametrów. Proszę podać walutę datę startu i końca zakresu");
                }

                if (!AllowedCurrencyCodes.Contains(args[0]))
                {
                    throw new Exception("Nieobsługiwana waluta.");
                }
                var currencyCode = args[0];

                if (!DateTime.TryParse(args[1], out var startDate))
                {
                    throw new Exception("Niepoprawna data początku zakresu");
                }

                if (!DateTime.TryParse(args[2], out var endDate))
                {
                    throw new Exception("Niepoprawna data końca zakresu");
                }

                if (startDate > endDate)
                {
                    throw new Exception("Data początku zakresu nie może być większa niż data końca zakresu");
                }

                Console.WriteLine($"Pobieranie danych...");

                var couresData = NbpCurrenciesDataFinder.GetCurrencyCoursesData(currencyCode, startDate, endDate);
               
                var currencyCourseCalculator = new CurrencyCourseCalculator(couresData);

                Console.WriteLine($"Kurs średni sprzedaży {currencyCourseCalculator.CalculateSellRatesAverage()}");
                Console.WriteLine($"Odchylenie standardowe sprzedaży {currencyCourseCalculator.CalculateSellRatesStandardDeviation()}");
                Console.WriteLine($"Kurs maksymalny sprzedaży {currencyCourseCalculator.CalculateSellRatesMaxValue()}");
                Console.WriteLine($"Kurs minimalny sprzedaży {currencyCourseCalculator.CalculateSellRatesMinValue()}");

                Console.WriteLine();

                Console.WriteLine($"Kurs średni kupna {currencyCourseCalculator.CalculateBuyRatesAverage()}");
                Console.WriteLine($"Odchylenie standardowe kupna {currencyCourseCalculator.CalculateBuyRatesStandardDeviation()}");
                Console.WriteLine($"Kurs maksymalny kupna {currencyCourseCalculator.CalculateBuyRatesMaxValue()}");
                Console.WriteLine($"Kurs minimalny kupna {currencyCourseCalculator.CalculateBuyRatesMinValue()}");

                var courseDifference = currencyCourseCalculator.CalculateMaxCurseDifference();
                Console.WriteLine($"Największa różnica kursu wystąpiła dnia {courseDifference.Day} wynosiła {courseDifference.Difference}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Koniec");
                Console.ReadKey();
            }
        }
    }
}