using Microsoft.AspNet.Identity.EntityFramework;
using PT.Entity.IdentyModel;
using PT.Entity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pt.Dal
{
    public class Mycontext:IdentityDbContext<ApplicationUser>
    {
        public Mycontext() : base("name=Mycon") { }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<LaborLog> LaborLogss { get; set; }
        public virtual DbSet<SalaryLog> SalaryLogs { get; set; }

    }
}
