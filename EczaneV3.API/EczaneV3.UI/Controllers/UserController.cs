using EczaneV3.Entites.Models.Authentication;
using EczaneV3.Entites.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web;
using EczaneV3.Entites.Models.DataTable;
using System;


namespace EczaneV3.UI.Controllers
{
	public class UserController : Controller
	{
		readonly UserManager<AppUser> _userManager;
		readonly SignInManager<AppUser> _signInManager;
		public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			AppUser user = await _userManager.FindByEmailAsync(model.Email);
			if (user != null)
			{
				//İlgili kullanıcıya dair önceden oluşturulmuş bir Cookie varsa siliyoruz.
				await _signInManager.SignOutAsync();
				var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

		
                if (result.Succeeded && !user.LockoutEnabled)
				{
					await _userManager.ResetAccessFailedCountAsync(user); //Önceki hataları girişler neticesinde +1 arttırılmış tüm değerleri 0(sıfır)a çekiyoruz.

                    

                    return Redirect("/stock/AllStockList");
				}
				else
				{
					await _userManager.AccessFailedAsync(user); //Eğer ki başarısız bir account girişi söz konusu ise AccessFailedCount kolonundaki değer +1 arttırılacaktır. 

					int failcount = await _userManager.GetAccessFailedCountAsync(user); //Kullanıcının yapmış olduğu başarısız giriş deneme adedini alıyoruz.
					if (failcount == 3)
					{
						await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(1))); //Eğer ki başarısız giriş denemesi 3'ü bulduysa ilgili kullanıcının hesabını kitliyoruz.
						ViewBag.Error = "Art arda 3 başarısız giriş denemesi yaptığınızdan dolayı hesabınız 1 dk kitlenmiştir.";
					}
					else
					{
                        if (result.IsLockedOut)
                            ViewBag.Error = "Art arda 3 başarısız giriş denemesi yaptığınızdan dolayı hesabınız 1 dk kitlenmiştir.";
                        else
                            if (user.LockoutEnabled)
                                ViewBag.Error = "Hesabınız henüz onaylanmamıştır";
                            else
                                ViewBag.Error = "E-posta veya şifre yanlış.";

                    }

				}
			}
			else
			{
                ViewBag.Error = "Böyle bir kullanıcı bulunmamaktadır.";
			}

			return View(model);
		}

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/user/login");
        }
        public async Task<IActionResult> Profile()
        {
            UserDetailViewModel userDetail = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(userDetail);
        }
        public IActionResult EditPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (!result.Succeeded)
                    {
                        result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                        return View(model);
                    }
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);
                }
                else
                {
                    ModelState.AddModelError("Password error", "Eski şifrenizi hatalı girdiniz");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("Form error", "Formu yanlış doldurdunuz");
                return View(model);
            }
            ModelState.AddModelError("Başarılı", "Şifreniz başarı ile değiştirildi");
            return View(model);
        }
        public async Task<IActionResult> EditProfile()
        {
            UserDetailViewModel userDetail = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(userDetail);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.PhoneNumber = model.PhoneNumber;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                    return View(model);
                }
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, true);
            }
            return RedirectToAction("Index");
        }
        [HttpGet("[action]/{userId}/{token}")]
        public IActionResult UpdatePassword(string userId, string token)
        {
            return View();
        }
        [HttpPost("[action]/{userId}/{token}")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model, string userId, string token)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), model.Password);
            if (result.Succeeded)
            {
                ViewBag.State = true;
                await _userManager.UpdateSecurityStampAsync(user);
            }
            else
                ViewBag.State = false;
            return View();
        }
        public IActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel model)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(user.Email);
                mail.From = new MailAddress("devayyil@gmail.com", "Şifre Güncelleme", System.Text.Encoding.UTF8);
                mail.Subject = "Şifre Güncelleme Talebi";
                mail.Body = $"<a target=\"_blank\" href=\"https://localhost:5001{Url.Action("UpdatePassword", "User", new { userId = user.Id, token = HttpUtility.UrlEncode(resetToken) })}\">Yeni şifre talebi için tıklayınız</a>";
                mail.IsBodyHtml = true;
                SmtpClient smp = new SmtpClient();
                smp.Credentials = new NetworkCredential("devayyil@gmail.com", "23asjDnsd2");
                smp.Port = 587;
                smp.Host = "smtp.gmail.com";
                smp.EnableSsl = true;
                smp.Send(mail);

                ViewBag.State = true;
            }
            else
                ViewBag.State = false;

            return View();
        }
        public IActionResult Login(string ReturnUrl)
        {
            TempData["returnUrl"] = ReturnUrl;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListWaitUser()
        {
            return View(_userManager.Users.Where(q=>q.LockoutEnabled == true));
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> SignIn(AppUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = appUserViewModel.UserName,
                    Email = appUserViewModel.Email,
                    Memleket="Mardin",
                    Cinsiyet=true,
                    EmailConfirmed=true,
                    PhoneNumberConfirmed=false,
                    TwoFactorEnabled=false,
                    LockoutEnabled=true,
                    AccessFailedCount=0,
                    
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, appUserViewModel.Sifre);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            }
            return View();
        }

		public IActionResult ListUser()
		{

			return View();
		}

		public async Task<JsonResult> ListUserData()
		{
			DataTable<AppUser> data = new DataTable<AppUser>();
			var users = _userManager.Users.ToList();
            data.data= users;
			return Json(data);
		}

        public async Task<IActionResult> ConfirmUser(int id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            user.LockoutEnabled = false;
            await _userManager.UpdateAsync(user);
            return Redirect("/User");
        }

        public async Task<IActionResult> ConfirmWaitUser(int id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            user.LockoutEnabled = false;
            await _userManager.UpdateAsync(user);
            return Redirect("/User/ListWaitUser");
        }

        public async Task<IActionResult> RejectUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.LockoutEnabled = true;
            await _userManager.UpdateAsync(user);
            return Redirect("/User");
        }
    }
}
