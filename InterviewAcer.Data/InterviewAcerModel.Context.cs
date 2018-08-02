﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class InterviewAcerDbContext : DbContext
    {
        public InterviewAcerDbContext()
            : base("name=InterviewAcerDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<InterviewDetail> InterviewDetails { get; set; }
        public virtual DbSet<InterviewType> InterviewTypes { get; set; }
        public virtual DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public virtual DbSet<GroupCheckList> GroupCheckLists { get; set; }
        public virtual DbSet<StageGroup> StageGroups { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<InterviewCheckListMapping> InterviewCheckListMappings { get; set; }
        public virtual DbSet<InterviewCompletedStageMapping> InterviewCompletedStageMappings { get; set; }
        public virtual DbSet<InterviewStageFeedback> InterviewStageFeedbacks { get; set; }
    
        public virtual ObjectResult<GetInterviewStage_Result> GetInterviewStage(Nullable<int> interviewId)
        {
            var interviewIdParameter = interviewId.HasValue ?
                new ObjectParameter("InterviewId", interviewId) :
                new ObjectParameter("InterviewId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetInterviewStage_Result>("GetInterviewStage", interviewIdParameter);
        }
    
        public virtual ObjectResult<usp_GetCompletedStages_Result> usp_GetCompletedStages(Nullable<int> interviewId)
        {
            var interviewIdParameter = interviewId.HasValue ?
                new ObjectParameter("interviewId", interviewId) :
                new ObjectParameter("interviewId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetCompletedStages_Result>("usp_GetCompletedStages", interviewIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> usp_GetUserTotalScore(string userId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("usp_GetUserTotalScore", userIdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> usp_UpdateCheckList(Nullable<int> checklistId, Nullable<int> interviewId)
        {
            var checklistIdParameter = checklistId.HasValue ?
                new ObjectParameter("checklistId", checklistId) :
                new ObjectParameter("checklistId", typeof(int));
    
            var interviewIdParameter = interviewId.HasValue ?
                new ObjectParameter("interviewId", interviewId) :
                new ObjectParameter("interviewId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("usp_UpdateCheckList", checklistIdParameter, interviewIdParameter);
        }
    }
}
