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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Admin  user seed 

            var adminEmail = "admin@admin.com";
            var adminPassword = "Admin123@";
            var adminUserId = Guid.NewGuid();

            var hasher = new PasswordHasher<ApplicationIdentityUser>();
            var adminUser = new ApplicationIdentityUser
            {
                Id = adminUserId,
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, adminPassword); 

            modelBuilder.Entity<ApplicationIdentityUser>().HasData(adminUser);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminUserId, 
                Name = "Admin User",
                Email = adminEmail
            });

        }

        public DbSet<User> User {  get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
