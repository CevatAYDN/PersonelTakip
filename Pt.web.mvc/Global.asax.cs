using Microsoft.AspNet.Identity;
using Pt.Bl.AccountRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pt.web.mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var roleManager = MemberShipTools.NewRoleManager();

            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new PT.Entity.IdentyModel.ApplicationRole
                {
                    Name = "Admin",
                    Description = "Sistem Yöneticisi"
                });
            }


            if (!roleManager.RoleExists("User"))
            {
                roleManager.Create(new PT.Entity.IdentyModel.ApplicationRole
                {
                    Name = "User",
                    Description = "Sistem Kullanıcısı"
                });
            }

            if (!roleManager.RoleExists("Passive"))
            {
                roleManager.Create(new PT.Entity.IdentyModel.ApplicationRole
                {
                    Name = "Passive",
                    Description = "E-mail Aktivasyon Gerekli"
                });
            }
        }
    }
}
