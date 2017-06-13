using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoutSystem.Areas.Api.Models
{
    public class UserListItem
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Role { get; set; }
        public string Production { get; set; }
        public int SSO { get; set; }

        public UserListItem(IdentityUser User, IdentityRole Role, Entities.UserInfo Info)
        {
            this.Username = Info.FirstName + " " + Info.LastName;
            this.Email = User.Email;
            this.Active = (User.LockoutEndDateUtc <= DateTime.Now || User.LockoutEndDateUtc == null);
            this.Role = Role.Name;
            this.SSO = Info.SSO;

            if(Info != null)
                this.Production = Info.Production.Name;
        }
    }
    public class RegisterUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SSO { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public int ProductionID { get; set; }
    }

    public class ChangePasswordIn
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ChangeProductionIn
    {
        public string Email { get; set; }
        public int ProductionID { get; set; }
    }
    public class ToggleActiveIn
    {
        public string Email { get; set; }
    }

}