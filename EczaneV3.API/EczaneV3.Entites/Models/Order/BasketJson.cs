using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models.Order
{
    public class BasketJson
    {
        public string id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public int adet { get; set; }
        public float totalPrice { get; set; }
    }
}
