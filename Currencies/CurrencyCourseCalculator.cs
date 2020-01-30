using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLParse
{
    public class CurrencyCourseCalculator
    {
        private readonly ICollection<CurrencyCourseInfoDto> _currencyCourseInfos;

        private readonly ICollection<double> _buyRates;
        private readonly ICollection<double> _sellRates;

        public CurrencyCourseCalculator(ICollection<CurrencyCourseInfoDto> currencyCourseInfos)
        {
            _currencyCourseInfos = currencyCourseInfos;

            _buyRates = currencyCourseInfos.Select(c => c.BuyRate).ToArray();
            _sellRates = currencyCourseInfos.Select(c => c.SellRate).ToArray();
        }

        public double CalculateSellRatesAverage() => _sellRates.Average();
        public double CalculateBuyRatesAverage() => _buyRates.Average();


        public double CalculateSellRatesMaxValue() => _sellRates.Max();
        
        public double CalculateBuyRatesMaxValue() => _buyRates.Max();


        public double CalculateSellRatesMinValue() => _sellRates.Min();
        
        public double CalculateBuyRatesMinValue() => _buyRates.Min();


        public double CalculateSellRatesStandardDeviation() => CalculateStandardDeviation(_sellRates);
        
        public double CalculateBuyRatesStandardDeviation() => CalculateStandardDeviation(_buyRates);

        public CourseDifferenceDto CalculateMaxCurseDifference()
        {
            var maxCurseDifference = _currencyCourseInfos.Max(c => c.BuyRate - c.SellRate);
            
            var maxCurseDifferenceWithDay = _currencyCourseInfos
              .Select(c => new CourseDifferenceDto
              {
                  Difference = c.BuyRate - c.SellRate,
                  Day = c.Day
              })
              .FirstOrDefault(x => Math.Abs(x.Difference - maxCurseDifference) < 0.000000001);

            return maxCurseDifferenceWithDay;
        }

        private static double CalculateStandardDeviation(IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                double avg = values.Average();
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }
    }
}