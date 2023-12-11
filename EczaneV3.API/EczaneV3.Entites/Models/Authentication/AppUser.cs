using EczaneV3.Entites.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Entites.Models.Authentication
{
	public class AppUser : IdentityUser<int>
	{
		public string Memleket { get; set; }
		public bool Cinsiyet { get; set; }

		public static implicit operator UserDetailViewModel(AppUser user)
		{
			return new UserDetailViewModel
			{
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				UserName = user.UserName
			};
		}
	}
}
