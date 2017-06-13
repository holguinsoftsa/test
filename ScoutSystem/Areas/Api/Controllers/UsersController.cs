using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ScoutSystem.Entities;
using ScoutSystem.Areas.Api.Models;
using Microsoft.AspNet.Identity;
using ScoutSystem.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace ScoutSystem.Areas.Api.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private ScoutSystem.Models.ApplicationDbContext context = new ScoutSystem.Models.ApplicationDbContext();
        private ScoutSystemDevEntities db = new ScoutSystemDevEntities();

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResponse GetUserList()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var userList = new List<UserListItem>();
            var dbUsers = UserManager.Users;
            var roles = roleManager.Roles;
            var userInfoList = db.UserInfo.ToList();

            foreach (var user in dbUsers)
            {
                Entities.UserInfo userInfo = null;
                //Checks if contains userInfo record otherwise leave it null
                var item2 = userInfoList.Where(m => m.AspUserID == user.Id);
                if (item2.Count() > 0)
                    userInfo = item2.First();
                userList.Add(new UserListItem(user, roleManager.FindById(user.Roles.First().RoleId), userInfo));
            }

            return new JsonResponse(true, userList.OrderByDescending(m => m.Email).ToArray());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResponse ChangePassword(ChangePasswordIn model)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var userID = UserManager.FindByEmail(model.Email).Id;
            var removePassword = UserManager.RemovePassword(userID);

            if (removePassword.Succeeded)
            {
                //Removed Password Success
                var AddPassword = UserManager.AddPassword(userID, model.Password);
                if (AddPassword.Succeeded)
                    return new JsonResponse(true);
                else
                    return new JsonResponse(false, AddPassword.Errors.FirstOrDefault());
            }
            else
                return new JsonResponse(false, removePassword.Errors.FirstOrDefault());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResponse ChangeProduction(ChangeProductionIn model)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var userID = UserManager.FindByEmail(model.Email).Id;
            bool success = false;

            var prod = db.Production.Find(model.ProductionID);
            if (prod != null)
            {
                db.UserInfo.Find(userID).ProductionID = prod.ProductionID;
                db.SaveChanges();
                success = true;
            }
            return new JsonResponse(success, null);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResponse ToggleActive(ToggleActiveIn model)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = UserManager.FindByEmail(model.Email);

            //Return here, User can not deactivate own account.
            var currUserId = User.Identity.GetUserId();
            var isLocked = UserManager.IsLockedOut(currUserId);
            if (User.Identity.GetUserName() == model.Email && !isLocked)
                return new JsonResponse(false, "You cannot deactivate your own account.");


            //Lockout user
            if (user.LockoutEnabled)
            {
                user.LockoutEnabled = false;
                user.LockoutEndDateUtc = DateTime.Now;
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEndDateUtc = DateTime.MaxValue;
            }

            var result = UserManager.Update(user);
            return new JsonResponse(result.Succeeded, result.Errors.FirstOrDefault());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResponse CreateUser(RegisterUser model)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //Errors
            if (model.Password != model.ConfirmPassword)
                return new JsonResponse(false, "Passwords do not match.");

            if (model.Role == null)
                return new JsonResponse(false, "Role not selected.");

            if (!roleManager.RoleExists(model.Role))
                return new JsonResponse(false, "Role does not exist.");

            //Verify Email Address
            var emailChecker = new EmailAddressAttribute();
            if (!emailChecker.IsValid(model.Email))
                return new JsonResponse(false, "Email is not a valid format.");

            //Create User
            return _createUser(model);
        }

        [HttpGet]
        public JsonResponse GetAllRoles()
        {
            return new JsonResponse(true, AppConfig.AllRoles);
        }

        [HttpGet]
        public JsonResponse GetUserInfoMe ()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var userID = this.User.Identity.GetUserId();
            var user = UserManager.Users.First(m => m.Id == userID);
            var role = roleManager.FindById(user.Roles.First().RoleId);
            var userInfo = db.UserInfo.First(m => m.AspUserID == userID);

            return new JsonResponse(true, new UserListItem(user, role, userInfo));
        }
        [HttpGet]
        public JsonResponse GetProduction()
        {
            var id = User.Identity.GetUserId();
            var prod = db.UserInfo.Include(m => m.Production).First(m => m.AspUserID == id).Production.Name;
            return new JsonResponse(true, (object)prod);
        }

        //Made this public static so it could be used in StartUp.cs to create the admin account
        public static JsonResponse _createUser(RegisterUser model)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            ScoutSystemDevEntities db = new ScoutSystemDevEntities();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            bool success = true;
            string error = "";

            //Create User
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = UserManager.Create(user, model.Password);
            if (!result.Succeeded)
            {
                success = false;
                error = result.Errors.First();
            }

            //Add to ASPRole
            IdentityResult roleResult;
            if (success)
            {
                roleResult = UserManager.AddToRole(user.Id, model.Role);
                if (!roleResult.Succeeded)
                {
                    success = false;
                    error = roleResult.Errors.First();
                }
            }

            //Add custom UserInfo
            if (success)
            {
                var info = new UserInfo();
                info.AspUserID = user.Id;

                //Assign Production
                int prodID = 0;
                if (model.Role == UserRole.Admin.ToString()) //If Role is admin, production should be admin.
                    prodID = db.Production.Where(m => m.Name == "Admin").First().ProductionID;
                else if (model.ProductionID == 0) //Defualt to Unassigned
                    prodID = db.Production.Where(m => m.Name == "Unassigned").First().ProductionID;
                else
                    prodID = db.Production.First(m => m.ProductionID == model.ProductionID).ProductionID;

                info.ProductionID = prodID;
                info.LastName = model.LastName;
                info.FirstName = model.FirstName;
                info.SSO = model.SSO;
                db.UserInfo.Add(info);
                db.SaveChanges();
            }
            else
            {
                UserManager.Delete(user);
            }

            return new JsonResponse(success, error);
        }
    }
}
