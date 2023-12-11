using EczaneV3.Entites.Common;
using EczaneV3.Entites.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models.Order
{
	public class OrderItem :BaseEntity
	{
        public Medicament Medicament { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public float TotalPrice { get; set; }
    }
}
