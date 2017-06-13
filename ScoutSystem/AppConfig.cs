using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoutSystem
{
    public static class AppConfig
    {
        /// <summary>
        /// All of the roles in the application.
        /// </summary>
        public static UserRole[] AllRoles
        {
            get
            {
                return new UserRole[] {
                    UserRole.Admin,
                    UserRole.Scout,
                    UserRole.Manager,
                    UserRole.BusinessAffair,
                    UserRole.OnSiteManager,
                    UserRole.Executive
                };
            }
        }

        public static int TimeZoneOffset
        {
            get { return -5; }
        }

        public static string[] DefaultOnSiteUnits
        {
            get {
                return new string[]
                {
                    "UNIT 1",
                    "UNIT 2",
                    "UNIT 3",
                    "UNIT BREAK DOWN",
                    "UNIT SETUP"
                };
            }
        }

        public static string[] OnsiteIncidentCategory
        {
            get
            {
                return new string[]
                {
                    "EHS",
                    "Fleet",
                    "Facilities",
                    "Production"
                };
            }
        }

        public static string OnSiteManager_Email = Properties.Settings.Default.OnSiteReport_DistributionList;
    }

    public struct UserRole
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public UserRole(string Name)
        {
            this.Name = Name;
        }

        public static readonly UserRole Admin = new UserRole("Admin");
        public static readonly UserRole Scout = new UserRole("Scout");
        public static readonly UserRole Manager = new UserRole("Manager");
        public static readonly UserRole BusinessAffair = new UserRole("Business Affairs");
        public static readonly UserRole OnSiteManager = new UserRole("OnSite Manager");
        public static readonly UserRole Executive = new UserRole("Executive");

    }
}