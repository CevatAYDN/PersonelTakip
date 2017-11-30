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
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var userManager = MemberShipTools.NewUserManager();
            var users = userManager.Users.Select(x=> new UsersViewModel
            {
                Email=x.Email,
                Name=x.Name,
                RegisterDate=x.RegistryDate,
                Salary=x.Salary,
                SurName=x.Surname,
                UserId=x.Id,
                UserName=x.UserName,
                RoleId=x.Roles.FirstOrDefault().RoleId,
                RoleName=MemberShipTools.NewRoleManager().FindById(x.Roles.FirstOrDefault().RoleId).Name
            }).ToList();

            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            List<SelectListItem> rolist = new List<SelectListItem>();
            roles.ForEach(x => new SelectListItem()
            {
                Text=x.Name,
                Value=x.Id
            });
            ViewBag.roles = rolist;

            return View(users);
        }
    }
}