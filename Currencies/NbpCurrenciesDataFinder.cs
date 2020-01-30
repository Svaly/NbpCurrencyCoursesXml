using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace XMLParse
{
    public static class NbpCurrenciesDataFinder
    {
        public static ICollection<CurrencyCourseInfoDto> GetCurrencyCoursesData(string currencyCode, DateTime startDate, DateTime endDate)
        {
            var currencyCourseInfos = new List<CurrencyCourseInfoDto>();
            var allAvilableCoursesDocumentsNames = GetAllAvilableCoursesDocumentsNames();

            // iteracja po dniach z podanego zakresu dat
            for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))
            {
                // formatowanie nazwy dokumentu dla danego dnia
                var documentDate = $"{day:yy}{day.Month:D2}{day.Day:D2}";

                // sprawdzenie czy dokument dla danego dnia istnieje na stronie nbp
                var xmlDocumentName = allAvilableCoursesDocumentsNames.SingleOrDefault(c => c.Contains(documentDate));

                // jeżeli istnije to jego nazwa nie bedzie nulem
                if (xmlDocumentName != null)
                {
                    string url = @$"http://www.nbp.pl/kursy/xml/{xmlDocumentName}";
                    var xmlData = MakeWebRequest(url);
                    var currencyData = ParseXmlWithCurrencyCourses(xmlData, currencyCode, day);
                    currencyCourseInfos.Add(currencyData);
                }
            }

            return currencyCourseInfos;
        }

        private static ICollection<string> GetAllAvilableCoursesDocumentsNames()
        {
            var xmlDocumentsListHtml = MakeWebRequest("http://www.nbp.pl/kursy/xml/dir.aspx?tt=C");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(xmlDocumentsListHtml);
            var xmlDocumentNames = htmlDoc.DocumentNode.SelectNodes("//body/pre/a").Select(c => c.InnerHtml).ToList();

            return xmlDocumentNames;
        }

        private static string MakeWebRequest(string url)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            Encoding encode = Encoding.UTF8;
            using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
            using (Stream receiveStream = myHttpWebResponse.GetResponseStream())
            using (StreamReader readStream = new StreamReader(receiveStream, encode))
            {
                return readStream.ReadToEnd();
            }
        }
        private static CurrencyCourseInfoDto ParseXmlWithCurrencyCourses(string data, string currencyCode, DateTime day)
        {
            XElement xmlData = XElement.Parse(data);

            return xmlData.Descendants(NbpCurrencyXmlNodes.Position)
              .Select(
                xmlCurrencyPosition =>
                  new CurrencyCourseInfoDto
                  {
                      CurrencyCode = xmlCurrencyPosition.Element(NbpCurrencyXmlNodes.CurrencyCode).Value,
                      SellRate = Convert.ToDecimal(xmlCurrencyPosition.Element(NbpCurrencyXmlNodes.SellRate).Value),
                      BuyRate = Convert.ToDecimal(xmlCurrencyPosition.Element(NbpCurrencyXmlNodes.BuyRate).Value),
                      Day = day
                  })
              .SingleOrDefault(c => c.CurrencyCode == currencyCode);
        }
    }
}