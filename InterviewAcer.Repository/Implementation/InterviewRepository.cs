using InterviewAcer.Common.DTO;
using InterviewAcer.Data;

using InterviewAcer.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewAcer.Repository.Implementation
{
    public class InterviewRepository: IInterviewRepository
    {
        private InterviewAcerDbContext _dbContext;
        public InterviewRepository(InterviewAcerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<InterviewDetailsDTO>> GetInterviewDetails(string userName)
        {
            List<InterviewDetailsDTO> interviewDetailsList = new List<InterviewDetailsDTO>();
            var interviewDetails = await _dbContext.InterviewDetails.Where(x => x.UserName == userName).ToListAsync();
            foreach(var item in interviewDetails)
            {
                var interviewDetailItem = new InterviewDetailsDTO();
                interviewDetailItem.CompanyName = item.CompanyName;
                interviewDetailItem.Designation = item.Designation;
                interviewDetailItem.HiringIndividualName = item.HiringIndividualName;
                interviewDetailItem.InterviewDate = item.InterviewDate;
                interviewDetailItem.InterviewTypeId = item.InterviewTypeId;
                interviewDetailsList.Add(interviewDetailItem);
            }
            return interviewDetailsList;
        }

        public void SaveInterviewDetails(InterviewDetailsDTO interviewDetails, string userName)
        {
            var interviewDetailEntity = new InterviewDetail();
            interviewDetailEntity.CompanyName = interviewDetails.CompanyName;
            interviewDetailEntity.Designation = interviewDetails.Designation;
            interviewDetailEntity.HiringIndividualName = interviewDetails.HiringIndividualName;
            interviewDetailEntity.InterviewDate = interviewDetails.InterviewDate;            
            interviewDetailEntity.InterviewTypeId = interviewDetails.InterviewTypeId;
            interviewDetailEntity.UserName = userName;
            _dbContext.InterviewDetails.Add(interviewDetailEntity);
        }
    }
}
