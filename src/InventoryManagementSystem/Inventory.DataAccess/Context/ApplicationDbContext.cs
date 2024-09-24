using Inventory.DataAccess.Entites;
using Inventory.DataAccess.IdentityManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser,IdentityRole<Guid>,Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> User {  get; set; }
    }
}
