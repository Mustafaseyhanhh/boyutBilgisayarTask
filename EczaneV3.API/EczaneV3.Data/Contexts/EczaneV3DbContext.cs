using EczaneV3.Entites.Models;
using EczaneV3.Entites.Models.Authentication;
using EczaneV3.Entites.Models.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EczaneV3.Data.Contexts
{
	public class EczaneV3DbContext : DbContext
	{
		public EczaneV3DbContext(DbContextOptions<EczaneV3DbContext> options) : base(options)
		{
			this.ChangeTracker.LazyLoadingEnabled = false;
		}
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


    }
}
