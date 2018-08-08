using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewAcer.Common.DTO
{
    public class UserSpecificDetailsDTO : UserGeneralDetailsDTO
    {
        public string Specialization { get; set; }
        public string AcadamicScore { get; set; }
        public int OngoingInterviews { get; set; }
        public int TotalNumberOfCheckListCompleted { get; set; }
        public int TotalNumberOfPointsScored { get; set; }
        public string imagePath { get; set; }
        public List<InterviewDetailsDTO> InterviewDetails { get; set; }
    }
}
