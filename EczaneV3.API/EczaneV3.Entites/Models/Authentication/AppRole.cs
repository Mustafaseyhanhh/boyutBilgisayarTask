using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models.Authentication
{
	public class AppRole : IdentityRole<int>
	{
		public DateTime OlusturulmaTarihi { get; set; }
	}
}
