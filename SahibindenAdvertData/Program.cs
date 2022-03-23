using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SahibindenAdvertData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<AdvertDetails> adverts = new List<AdvertDetails>(); //vitrindeki ilanları listede tuttum.

            HtmlWeb web = new HtmlWeb();
            string urlHomePage = "https://www.sahibinden.com"; //Verileri çekeceğim sayfanın url atamasını yaptım.
            HtmlDocument htmlDocument = web.Load(urlHomePage);
            HtmlDocument advertDocument; // İlan doküman sayfası için
            HtmlNode productNode;

            string productUrl;
            string hrefValue;
            string advertPrice;
            int i = 0;

            while (i < 30) // fazla istek atınca sahibinden otamatik atıyor
            {
                AdvertDetails advertDetail = new AdvertDetails();
                i++;
                productNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li[" + i + "]/a/span");
                if (productNode != null) //reklamları almamak için , çünkü reklamlarda ilan olmadığından dolayı.
                {

                    var hrefNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li[" + i + "]/a[@href]");
                    hrefValue = hrefNode.GetAttributeValue("href", string.Empty);
                    productUrl = urlHomePage + hrefValue;

                    Thread.Sleep(500);
                    advertDocument = web.Load(productUrl);
                    var ilanNode = advertDocument.DocumentNode.SelectSingleNode("//div[@class='classifiedInfo ']/h3"); //İlan fiyatının bulunduğu node

                    if (ilanNode != null)
                    {
                        advertPrice = ilanNode.InnerText.Substring(1).Trim();   
                        if (advertPrice.Substring(0, 1) == "€")
                        {

                            advertDetail.PriceUnit = priceUnit.Euro;
                            advertPrice = advertPrice.Substring(2);
                        }
                        else if (advertPrice.Substring(0, 1) == "$")
                        {
                            advertDetail.PriceUnit = priceUnit.Dolar;
                            advertPrice = advertPrice.Substring(2);
                        }
                        else
                        {
                            advertDetail.PriceUnit = priceUnit.TL;

                        }
                        int index = advertPrice.IndexOf(' ');
                        advertPrice = advertPrice.Substring(0, index);
                    }
                    else      //ücretsiz ilanlar olduğunda getirmek için.
                    {
                        advertPrice = "0";
                    }
                    advertDetail.Name = productNode.InnerText;
                    advertDetail.AdvertLink = productUrl;
                    advertDetail.Price = Decimal.Parse(advertPrice);
                    adverts.Add(advertDetail);
                }

            }
            i = 0;
            foreach (var item in adverts)
            {
                i++;
                Console.WriteLine(i + " ) " + item.Name + "\nİlan Fiyat: " + item.Price + item.PriceUnit + "\n");
            }
        }
    }
}
