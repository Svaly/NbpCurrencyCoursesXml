using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLParse
{
    public class CurrencyCourseCalculator
    {
        private readonly ICollection<CurrencyCourseInfoDto> _currencyCourseInfos;

        private readonly ICollection<decimal> _buyRates;
        private readonly ICollection<decimal> _sellRates;

        public CurrencyCourseCalculator(ICollection<CurrencyCourseInfoDto> currencyCourseInfos)
        {
            _currencyCourseInfos = currencyCourseInfos;

            _buyRates = currencyCourseInfos.Select(c => c.BuyRate).ToArray();
            _sellRates = currencyCourseInfos.Select(c => c.SellRate).ToArray();
        }

        public decimal CalculateSellRatesAverage() => _sellRates.Average();
        public decimal CalculateBuyRatesAverage() => _buyRates.Average();


        public decimal CalculateSellRatesMaxValue() => _sellRates.Max();
        
        public decimal CalculateBuyRatesMaxValue() => _buyRates.Max();


        public decimal CalculateSellRatesMinValue() => _sellRates.Min();
        
        public decimal CalculateBuyRatesMinValue() => _buyRates.Min();


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
              .FirstOrDefault(x => x.Difference == maxCurseDifference);

            return maxCurseDifferenceWithDay;
        }

        private static double CalculateStandardDeviation(IEnumerable<decimal> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                double avg = decimal.ToDouble(values.Average()) ;
                double sum = values.Sum(d => Math.Pow(decimal.ToDouble(d) - avg, 2));
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }
    }
}