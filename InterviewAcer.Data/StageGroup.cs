//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InterviewAcer.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class StageGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StageGroup()
        {
            this.GroupCheckLists = new HashSet<GroupCheckList>();
        }
    
        public int Id { get; set; }
        public int StageId { get; set; }
        public string GroupName { get; set; }
        public int Sequence { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupCheckList> GroupCheckLists { get; set; }
        public virtual Stage Stage { get; set; }
    }
}
