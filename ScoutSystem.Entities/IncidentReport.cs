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
    
    public partial class IncidentReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IncidentReport()
        {
            this.Attachment = new HashSet<Attachment>();
            this.IncidentReportMailingHistory = new HashSet<IncidentReportMailingHistory>();
        }
    
        public int IncidentReportID { get; set; }
        public int LocationReportID { get; set; }
        public int CategoryID { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string Notes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attachment> Attachment { get; set; }
        public virtual Category Category { get; set; }
        public virtual LocationReport LocationReport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidentReportMailingHistory> IncidentReportMailingHistory { get; set; }
    }
}
