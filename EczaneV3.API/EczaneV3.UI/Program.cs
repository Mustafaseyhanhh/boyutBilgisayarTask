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

			//serilog ayarlar�
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

			//loglama sisteminde serilog un kullan�lmas� i�in eklendi
			builder.Host.UseSerilog();

			// Add services to the container.
			builder.Services.AddServiceRegistration();

			builder.Services.AddIdentity<AppUser, AppRole>(_ =>
			{
				_.Password.RequiredLength = 5; //En az ka� karakterli olmas� gerekti�ini belirtiyoruz.
				_.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunlulu�unu kald�r�yoruz.
				_.Password.RequireLowercase = false; //K���k harf zorunlulu�unu kald�r�yoruz.
				_.Password.RequireUppercase = false; //B�y�k harf zorunlulu�unu kald�r�yoruz.
				_.Password.RequireDigit = false; //0-9 aras� say�sal karakter zorunlulu�unu kald�r�yoruz.

				_.User.RequireUniqueEmail = true; //Email adreslerini tekille�tiriyoruz.
				_.User.AllowedUserNameCharacters = "abc�defghi�jklmno�pqrs�tu�vwxyzABC�DEFGHI�JKLMNO�PQRS�TU�VWXYZ0123456789-._@+"; //Kullan�c� ad�nda ge�erli olan karakterleri belirtiyoruz.
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
					Name = "AspNetCoreIdentityExampleCookie", //Olu�turulacak Cookie'yi isimlendiriyoruz.
					HttpOnly = false, //K�t� niyetli insanlar�n client-side taraf�ndan Cookie'ye eri�mesini engelliyoruz.
					SameSite = SameSiteMode.Lax, //Top level navigasyonlara sebep olmayan requestlere Cookie'nin g�nderilmemesini belirtiyoruz.
					SecurePolicy = CookieSecurePolicy.Always //HTTPS �zerinden eri�ilebilir yap�yoruz.
				};
				_.SlidingExpiration = true; //Expiration s�resinin yar�s� kadar s�re zarf�nda istekte bulunulursa e�er geri kalan yar�s�n� tekrar s�f�rlayarak ilk ayarlanan s�reyi tazeleyecektir.
				_.ExpireTimeSpan = TimeSpan.FromMinutes(30); //CookieBuilder nesnesinde tan�mlanan Expiration de�erinin varsay�lan de�erlerle ezilme ihtimaline kar��n tekrardan Cookie vadesi burada da belirtiliyor.
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