using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Pt.Dal;
using PT.Entity.IdentyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.Bl.AccountRepository
{
    public class MemberShipTools
    {
        public static UserStore<ApplicationUser> NewUserStore() => new UserStore<ApplicationUser>(new Mycontext());
        public static UserManager<ApplicationUser> NewUserManager() => new UserManager<ApplicationUser>(NewUserStore());
        public static RoleStore<ApplicationRole> NewRoleStore() => new RoleStore<ApplicationRole>(new Mycontext());
        public static RoleManager<ApplicationRole> NewRoleManager() => new RoleManager<ApplicationRole>(NewRoleStore());
    }
}
