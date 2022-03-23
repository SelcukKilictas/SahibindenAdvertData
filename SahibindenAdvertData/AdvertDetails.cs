using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahibindenAdvertData
{
    public class AdvertDetails
    {
        public string Name { get; set; }

        public string AdvertLink { get; set; }

        public decimal Price { get; set; }

        public priceUnit PriceUnit { get; set; }

    }

    public enum priceUnit
    {
        TL,
        Dolar,
        Euro
    }
}
