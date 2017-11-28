using Microsoft.AspNet.Identity;
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
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt işleminde bir hata oluştu");
                return View();
            }
        }
    }
}