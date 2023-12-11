

using EczaneV3.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EczaneV3.Data.Abstract;
using EczaneV3.Data.Repositories;

namespace EczaneV3.Data
{
	public static  class ServiceRegistration
	{
		public static void AddServiceRegistration(this IServiceCollection services)
		{
			
			services.AddDbContext<EczaneV3DbContext>(
				  
				   options => options.UseNpgsql(Configuration.ConnectionString));

			services.AddDbContext<EczaneV3DbContext>(options => {
				options.UseNpgsql(Configuration.ConnectionString);
				options.UseLazyLoadingProxies(false);
			});

			services.AddDbContext<AppDbContext>(

                   options => options.UseNpgsql(Configuration.ConnectionString));

			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IBrandRepository, BrandRepository>();
			services.AddScoped<IMedicamentRepository, MedicamentRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IOrderItemRepository, OrderItemRepository>();

        }
	}
}
