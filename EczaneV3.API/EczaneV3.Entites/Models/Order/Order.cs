using EczaneV3.Entites.Common;
using EczaneV3.Entites.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models.Order
{
	public class Order : BaseEntity
	{
		public List<OrderItem> OrderItems = new List<OrderItem>();
        public float TotalPrice { get; set; }
		public string UserName { get; set; }
        public bool Status { get; set; } = false;
    }
}
