using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models.DataTable
{
	public class DataTable<T>
	{
		public List<T> data { get; set; } = new List<T>();
	}
}
