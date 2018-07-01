using InterviewAcer.Common.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewAcer.Repository.Contract
{
    interface IInterviewRepository
    {
        Task<List<InterviewDetailsDTO>> GetInterviewDetails(string userName);
        void SaveInterviewDetails(InterviewDetailsDTO interviewDetails, string userName);
    }
}
