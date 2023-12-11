using EczaneV3.Entites.Common;
using EczaneV3.Entites.Models.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models
{
	public class Medicament : BaseEntity
	{
		public string? Name { get; set; }
		public string? Barcode { get; set; }
        public string? Description { get; set; }
        public Brand Brand { get; set; }
		public Category Category { get; set; }
        public float? Price { get; set; }
        public int? Stock { get; set; }
        public string? StockCode { get; set; }

    }
}
