
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
    
public partial class GroupCheckList
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public GroupCheckList()
    {

        this.InterviewCheckListMappings = new HashSet<InterviewCheckListMapping>();

    }


    public int Id { get; set; }

    public int GroupId { get; set; }

    public string Name { get; set; }

    public int Points { get; set; }



    public virtual StageGroup StageGroup { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<InterviewCheckListMapping> InterviewCheckListMappings { get; set; }

}

}
