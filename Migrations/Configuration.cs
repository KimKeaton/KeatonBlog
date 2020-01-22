namespace KeatonBlog.Migrations
{
    using System;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using KeatonBlog.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<KeatonBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "KeatonBlog.Models.ApplicationDbContext";
        }

        protected override void Seed(KeatonBlog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            #region Role Maintenance
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            roleManager = new RoleManager<IdentityRole>(
                  new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }
            #endregion

            #region User Maintenance
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "kk@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "kk@gmail.com",
                    Email = "kk@gmail.com",
                    FirstName = "Kim",
                    LastName = "Keaton",
                    DisplayName = "Kim Keaton"
                }, "Abc&123!");
            }


            var userId = userManager.FindByEmail("kk@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");


            //Moderator

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FirstName = "CF",
                    LastName = "Moderator",
                    DisplayName = "CFMOD"
                }, "Abc&123!");
            }

            userId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");
            #endregion
        }
    }
}
