using EczaneV3.Entites.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models.Stock
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
