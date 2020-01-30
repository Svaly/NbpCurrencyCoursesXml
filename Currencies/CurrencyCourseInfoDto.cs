using System;

namespace XMLParse
{
    public class CurrencyCourseInfoDto
    {
        public string CurrencyCode { get; set; }
        public decimal SellRate { get; set; }
        public decimal BuyRate { get; set; }
        public DateTime Day { get; set; }
    }
}