using EczaneV3.UI.CustomValidations;
using Microsoft.AspNetCore.Identity;
using EczaneV3.Entites.Models.Authentication;
using EczaneV3.Data.Contexts;
using EczaneV3.Data;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;

namespace EczaneV3.UI
{
	public class Program
	{
		public static void Main(string[] args)
		{

			//serilog ayarlarý
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.WithThreadId()
				.Enrich.WithProcessId()
				.Enrich.WithEnvironmentName()
				.Enrich.WithMachineName()
				.WriteTo.Console(new CompactJsonFormatter())
				.WriteTo.File(new CompactJsonFormatter(), "Log/log.txt",rollingInterval: RollingInterval.Day)
				//.WriteTo.PostgreSQL("User ID=postgres;Password=root123;Host=localhost;Port=5432;Database=EczaneV3;","SerilogUI")
				.CreateLogger();

			var builder = WebApplication.CreateBuilder(args);

			//loglama sisteminde serilog un kullanýlmasý için eklendi
			builder.Host.UseSerilog();

			// Add services to the container.
			builder.Services.AddServiceRegistration();

			builder.Services.AddIdentity<AppUser, AppRole>(_ =>
			{
				_.Password.RequiredLength = 5; //En az kaç karakterli olmasý gerektiðini belirtiyoruz.
				_.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunluluðunu kaldýrýyoruz.
				_.Password.RequireLowercase = false; //Küçük harf zorunluluðunu kaldýrýyoruz.
				_.Password.RequireUppercase = false; //Büyük harf zorunluluðunu kaldýrýyoruz.
				_.Password.RequireDigit = false; //0-9 arasý sayýsal karakter zorunluluðunu kaldýrýyoruz.

				_.User.RequireUniqueEmail = true; //Email adreslerini tekilleþtiriyoruz.
				_.User.AllowedUserNameCharacters = "abcçdefghiýjklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789-._@+"; //Kullanýcý adýnda geçerli olan karakterleri belirtiyoruz.
			}).AddPasswordValidator<CustomPasswordValidation>()
			  .AddUserValidator<CustomUserValidation>()
			  .AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>()

              .AddDefaultTokenProviders();
			

			builder.Services.ConfigureApplicationCookie(_ =>
			{
				_.LoginPath = new PathString("/User/Login");
				_.LogoutPath = new PathString("/User/Logout");
				_.Cookie = new CookieBuilder
				{
					Name = "AspNetCoreIdentityExampleCookie", //Oluþturulacak Cookie'yi isimlendiriyoruz.
					HttpOnly = false, //Kötü niyetli insanlarýn client-side tarafýndan Cookie'ye eriþmesini engelliyoruz.
					SameSite = SameSiteMode.Lax, //Top level navigasyonlara sebep olmayan requestlere Cookie'nin gönderilmemesini belirtiyoruz.
					SecurePolicy = CookieSecurePolicy.Always //HTTPS üzerinden eriþilebilir yapýyoruz.
				};
				_.SlidingExpiration = true; //Expiration süresinin yarýsý kadar süre zarfýnda istekte bulunulursa eðer geri kalan yarýsýný tekrar sýfýrlayarak ilk ayarlanan süreyi tazeleyecektir.
				_.ExpireTimeSpan = TimeSpan.FromMinutes(30); //CookieBuilder nesnesinde tanýmlanan Expiration deðerinin varsayýlan deðerlerle ezilme ihtimaline karþýn tekrardan Cookie vadesi burada da belirtiliyor.
				_.AccessDeniedPath = new PathString("/authority/page");
			});

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}