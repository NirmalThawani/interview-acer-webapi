using InterviewAcer.Common.DTO;
using InterviewAcer.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewAcer.Repository.Implementation
{
    
    public class StageRepository
    {
        private InterviewAcerDbContext _dbContext;
        public StageRepository(InterviewAcerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Stage> GetStages(int interviewTypeId)
        {
            return _dbContext.Stages.Where(x => x.InterviewTypeId == interviewTypeId);
        }

        public IQueryable<StageGroup> GetGroups(int stageId)
        {
            return _dbContext.StageGroups.Where(x => x.StageId == stageId);
        }

        public IQueryable<GroupCheckList> GetCheckList(int grouId)
        {
            return _dbContext.GroupCheckLists.Where(x => x.GroupId == grouId);
        }

        public List<StageDTO> GetAllStageData(int interviewType)
        {
            List<StageDTO> stageDetails = new List<StageDTO>();
            var stages = GetStages(interviewType);
            foreach(var stage in stages)
            {
                StageDTO stageDetailItem = new StageDTO();
                stageDetailItem.Name = stage.StageName;
                stageDetailItem.StageId = stage.Id;
                stageDetailItem.Sequence = stage.Sequence;
                var stageGroups = GetGroups(stageDetailItem.StageId);
                stageDetailItem.StageGroups = new List<GroupDTO>();
                foreach(var group in stageGroups)
                {
                    GroupDTO groupItem = new GroupDTO();
                    groupItem.GroupId = group.Id;
                    groupItem.Name = group.GroupName;
                    groupItem.Sequence = group.Sequence;
                    var groupCheckList = GetCheckList(groupItem.GroupId);
                    groupItem.GroupCheckList = new List<CheckListDTO>();
                    foreach(var checkList in groupCheckList)
                    {
                        CheckListDTO checkListItem = new CheckListDTO();
                        checkListItem.CheckListId = checkList.Id;
                        checkListItem.Name = checkList.Name;
                        checkListItem.Points = checkList.Points;
                        groupItem.GroupCheckList.Add(checkListItem);
                    }
                    stageDetailItem.StageGroups.Add(groupItem);
                }
                stageDetails.Add(stageDetailItem);
            }
            return stageDetails;
        }
    }
}
