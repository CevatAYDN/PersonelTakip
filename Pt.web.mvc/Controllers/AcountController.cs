using Microsoft.AspNet.Identity;
using Pt.Bl.AccountRepository;
using PT.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            var userManager = MemberShipTools.NewUserManager();
            var checkuser = userManager.FindByName(model.Username);
            if (checkuser!= null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullacını zaten kayıtlı");
                return View(model);
            }
            return View();
        }
    }
}