using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Pt.Bl.AccountRepository;
using Pt.Bl.Settings;
using PT.Entity.IdentyModel;
using PT.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Pt.web.mvc.Controllers
{
    public class AcountController : Controller
    {
        // GET: Acount
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userManager = MemberShipTools.NewUserManager();
            var checkuser = userManager.FindByName(model.Username);
            if (checkuser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullacını zaten kayıtlı");
                return View(model);
            }
            checkuser = userManager.FindByEmail(model.Email);
            if (checkuser!=null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullacını zaten kayıtlı");
                return View(model);
            }

            var activationcode = Guid.NewGuid().ToString();
            ApplicationUser user = new ApplicationUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Username,
                ActivationCode = activationcode
            };
            var response = userManager.Create(user, model.Password);

            if (response.Succeeded)
            {
                string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                if (userManager.Users.Count() == 1)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    await SiteSettings.SendMail(new MailModel
                    {
                        To = user.Email,
                        Subject = "Hoşgeldin Adamım",
                        Message = "Siteyi Yönet Dayı"
                    });
                }
                else
                {
                    userManager.AddToRole(user.Id, "Passive");
                    await SiteSettings.SendMail(new MailModel
                    {
                        Message = $"Merhaba {user.Name}{user.Surname}, </br> Sisteme başarı ile kayıt oldunuz. <br/> Hesabınızı aktifleştirmek için <a href='{siteUrl}/Account/Activation?code={activationcode}'>Aktivasyon Kodu</a>",
                        Subject = "Personel Yönetimi - Aktivasyon",
                        To = user.Email
                    });
                }
                return RedirectToAction("Login", "Acount");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt işleminde bir hata oluştu");
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userManager = MemberShipTools.NewUserManager();
            var user = await userManager.FindAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Böyle Bir kullanıcı bulunamadı");
                return View(model);
            }
            var authManager = HttpContext.GetOwinContext().Authentication;
            var useridentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignIn(new AuthenticationProperties()
            {
                IsPersistent = model.RememberMe
            }, useridentity);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login", "Acount");
        }

        public async Task<ActionResult> Activation(string code)
        {
            var userStore = MemberShipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.ActivationCode == code);
            if (sonuc == null)
            {
                ViewBag.sonuc = "Aktivasyon işlemi Başarısız";
                return View();
            }
            sonuc.EmailConfirmed = true;
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();

            userManager.RemoveFromRole(sonuc.Id, "Passive");
            userManager.AddToRole(sonuc.Id, "User");

            ViewBag.sonuc = $"Merhaba{sonuc.Name} {sonuc.Surname}<br/> Aktivasyon işleminiz başarılı";

            await SiteSettings.SendMail(new MailModel()
            {
                To = sonuc.Email,
                Message = ViewBag.sonuc.ToString(),
                Subject = "Aktivasyon",
                Bcc = "cvtaydn53@gmail.com"
            });
            return View();
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoverPassword(string email)
        {
            var userStore = MemberShipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.Email == email);
            if (sonuc == null)
            {
                ViewBag.sonuc = "E mail Adresiniz sisteme kayıtlı değil";
                return View();
            }
            var randompass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            await userStore.SetPasswordHashAsync(sonuc, userManager.PasswordHasher.HashPassword(randompass));
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();

            await SiteSettings.SendMail(new MailModel()
            {
                To=sonuc.Email,
                Subject="Şifreniz Değişti",
                Message=$"Merhaba {sonuc.Name}{sonuc.Surname} <br/> Yeni Şifreniz:<b>{randompass}</b>"
            });
            ViewBag.sonuc = "E mail adresinize yeni şifreniz gönderilmiştir";
            return View();
        }


    }

}
