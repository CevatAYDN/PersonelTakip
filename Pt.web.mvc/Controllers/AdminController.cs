using Microsoft.AspNet.Identity;
using Pt.Bl.AccountRepository;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            var userManager = MemberShipTools.NewUserManager();
            var users = userManager.Users.ToList().Select(x => new UsersViewModel
            {
                Email = x.Email,
                Name = x.Name,
                RegisterDate = x.RegistryDate,
                Salary = x.Salary,
                SurName = x.Surname,
                UserId = x.Id,
                UserName = x.UserName,
                RoleId = x.Roles.FirstOrDefault().RoleId,
                RoleName = roles.FirstOrDefault(y => y.Id == userManager.FindById(x.Id).Roles.FirstOrDefault().RoleId).Name
            }).ToList();

            //List<SelectListItem> rolist = new List<SelectListItem>();
            //roles.ForEach(x => new SelectListItem()
            //{
            //    Text=x.Name,
            //    Value=x.Id
            //});
            //ViewBag.roles = rolist;

            return View(users);
        }

        public ActionResult EditUser(string id)
        {
            if (id == null)
                RedirectToAction("Index");

            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            List<SelectListItem> rolList = new List<SelectListItem>();
            roles.ForEach(x => rolList.Add(new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id
            }));
            ViewBag.roles = rolList;
            var userManager = MemberShipTools.NewUserManager();
            var user = userManager.FindById(id);
            if (user == null)
                return RedirectToAction("Index");

            var model = new UsersViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                SurName = user.Surname,
                Name = user.Name,
                RegisterDate = user.RegistryDate,
                RoleId = user.Roles.ToList().FirstOrDefault().RoleId,
                RoleName = roles.FirstOrDefault(r => r.Id == userManager.FindById(user.Id).Roles.FirstOrDefault().RoleId).Name,
                Salary = user.Salary,
                UserId = user.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UsersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            var userStore = MemberShipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindById(model.UserId);
            if (user == null)
            {
                return View("Index");
            }
            user.UserName = model.UserName;
            user.Name = model.Name;
            user.Surname = model.SurName;
            user.Salary = model.Salary;
            user.Email = model.Email;
            user.RegistryDate = model.RegisterDate;

            if (model.RoleId!=user.Roles.ToList().First().RoleId)
            {
                var yeniroladi = roles.First(x => x.Id == model.RoleId).Name;
                userManager.AddToRole(model.UserId, yeniroladi);
                var eskiroladi = roles.First(x => x.Id == user.Roles.ToList().First().RoleId).Name;
                userManager.RemoveFromRole(model.UserId, eskiroladi);
            }


            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            return RedirectToAction("EditUser",new { id=model.UserId});

        }
    }
}
