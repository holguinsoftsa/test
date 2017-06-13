using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ScoutSystem.Entities;
using ScoutSystem.Models;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(ScoutSystem.Startup))]
namespace ScoutSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           
            createProductions();
            createRoles();
            createUsers();
            createUnits();
            createOnsiteIncident();

        }
        /// <summary>
        /// Create Default Productions
        /// </summary>
        private void createProductions()
        {
            ScoutSystemDevEntities db = new ScoutSystemDevEntities();
            var prods = db.Production.ToList();

            if (!prods.Exists(m => m.Name == "Admin"))
                db.Production.Add(new Production() { Name = "Admin" });

            if (!prods.Exists(m => m.Name == "Unassigned"))
                db.Production.Add(new Production() { Name = "Unassigned" });

            db.SaveChanges();
        }
        /// <summary>
        /// Create User roles. 
        /// </summary>
        private void createRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //Iterate through all the roles and create them if necessary
            foreach(var role in AppConfig.AllRoles)
            {
                if (!roleManager.RoleExists(role.Name))
                {
                    var newRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    newRole.Name = role.Name;
                    roleManager.Create(newRole);
                }
            }

        }
        /// <summary>
        ///  Creates Admin User for login.  
        /// </summary>
        private void createUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            ScoutSystemDevEntities db = new ScoutSystemDevEntities();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ////Here we create a Admin super user who will maintain the website                  
            //var user = new ApplicationUser();
            //user.UserName = "appsadmin@nbcuni.com";
            //user.Email = "appsadmin@nbcuni.com";

            //string userPWD = "@dmin2016";

            //var chkUser = UserManager.Create(user, userPWD);

            ////Add User to Role Admin   
            //if (chkUser.Succeeded)
            //{
            //    var result1 = UserManager.AddToRole(user.Id, "Admin");
            //}
            if (UserManager.FindByEmail("appsadmin@nbcuni.com") == null)
            {
                var user = new ScoutSystem.Areas.Api.Models.RegisterUser()
                {
                    Email = "appsadmin@nbcuni.com",
                    Password = "@dmin2016",
                    Role = "Admin",
                    ProductionID = 1, FirstName = "Admin", LastName = "Admin", SSO = 0
                };

                ScoutSystem.Areas.Api.Controllers.UsersController._createUser(user);
            }
        }
        /// <summary>
        /// Creates default OnSiteUnits
        /// </summary>
        private void createUnits()
        {
            ScoutSystemDevEntities db = new ScoutSystemDevEntities();
            
            if(db.Unit.Count() <= 0)
            {
                foreach (var item in AppConfig.DefaultOnSiteUnits)
                {
                    db.Unit.Add(new Unit() { Name = item });
                }
                db.SaveChanges();
            }

            db.SaveChanges();
        }
        /// <summary>
        /// Creates default OnSite Incident Categories
        /// </summary>
        private void createOnsiteIncident()
        {
            ScoutSystemDevEntities db = new ScoutSystemDevEntities();

            if (db.Category.Count() <= 0)
            {
                foreach (var item in AppConfig.OnsiteIncidentCategory)
                {
                    if(db.Category.FirstOrDefault(m => m.Name == item) == null)
                        db.Category.Add(new Category() { Name = item });
                }
                db.SaveChanges();
            }

        }
    }
}
