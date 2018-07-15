using System.Collections.Generic;

namespace InterviewAcer.Common.DTO
{
    public class StageDTO
    {
        public int StageId { get; set; }
        public string Name { get; set; }
        public List<GroupDTO> StageGroups { get; set; }
        public int Sequence { get; set; }
    }
}
