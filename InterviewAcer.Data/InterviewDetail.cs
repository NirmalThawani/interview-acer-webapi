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
    
    public partial class InterviewDetail
    {
        public int InterviewDetailId { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public System.DateTime InterviewDate { get; set; }
        public int InterviewTypeId { get; set; }
        public string HiringIndividualName { get; set; }
        public string UserName { get; set; }
    
        public virtual InterviewType InterviewType { get; set; }
    }
}
