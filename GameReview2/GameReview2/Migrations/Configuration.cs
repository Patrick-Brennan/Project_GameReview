namespace GameReview2.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using GameReview2.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GameReview2.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GameReview2.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == RoleName.CanManage))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = RoleName.CanManage };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@admin.com", FullName = "admin", Email = "admin@admin.com" };

                manager.Create(user, "SecretPass0!");
                manager.AddToRole(user.Id, RoleName.CanManage);
            }

            if (!context.Roles.Any(r => r.Name == RoleName.CanDo))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = RoleName.CanDo };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "critic1@critic.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "critic1@critic.com", FullName = "critic1", Email = "critic1@critic.com" };

                manager.Create(user, "SecretPass1!");
                manager.AddToRole(user.Id, RoleName.CanDo);
            }

            if (!context.Users.Any(u => u.UserName == "critic2@critic.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "critic2@critic.com", FullName = "critic2", Email = "critic2@critic.com" };

                manager.Create(user, "SecretPass2!");
                manager.AddToRole(user.Id, RoleName.CanDo);
            }

            if (!context.Users.Any(u => u.UserName == "critic3@critic.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "critic3@critic.com", FullName = "critic3", Email = "critic3@critic.com" };

                manager.Create(user, "SecretPass3!");
                manager.AddToRole(user.Id, RoleName.CanDo);
            }
        }
    }
}


