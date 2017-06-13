//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScoutSystem.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserInfo()
        {
            this.DistributionList = new HashSet<DistributionList>();
            this.LocationReport = new HashSet<LocationReport>();
            this.IncidentReportMailingHistory = new HashSet<IncidentReportMailingHistory>();
        }
    
        public int ProductionID { get; set; }
        public string AspUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SSO { get; set; }
    
        public virtual Production Production { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DistributionList> DistributionList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationReport> LocationReport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidentReportMailingHistory> IncidentReportMailingHistory { get; set; }
    }
}
