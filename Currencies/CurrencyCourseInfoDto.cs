using System;

namespace XMLParse
{
    public class CurrencyCourseInfoDto
    {
        public string CurrencyCode { get; set; }
        public double SellRate { get; set; }
        public double BuyRate { get; set; }
        public DateTime Day { get; set; }
    }
}