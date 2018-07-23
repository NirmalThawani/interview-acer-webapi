using System.Web.Http;
using InterviewAcer.Repository.Contract;
using InterviewAcer.Repository.Implementation;
using System.Threading.Tasks;
using InterviewAcer.Common.DTO;
using System;
using System.Security.Claims;
using System.Linq;
using InterviewAcer.RequestClasses;
using InterviewAcer.ResponseClasses;

namespace InterviewAcer.Controllers
{
    public class StageController : ApiController
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public StageController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [Authorize(Roles ="Administrator")]
        [Route("api/GetStageDetails")]
        [HttpGet]
        public IHttpActionResult GetStages(int interviewTypeId)
        {
            try
            {
                var stages = _unitOfWork.GetStageRepository().GetStages(interviewTypeId);
                if(stages != null && stages.Any())
                {
                    return Ok(stages);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Authorize(Roles = "Administrator")]
        [Route("api/GetGroups")]
        [HttpGet]
        public IHttpActionResult GetGroups(int stageId)
        {
            try
            {
                var stages = _unitOfWork.GetStageRepository().GetGroups(stageId);
                if (stages != null && stages.Any())
                {
                    return Ok(stages);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/AddGroup")]
        [Authorize(Roles ="Administrator")]
        [HttpPost]
        public async Task<IHttpActionResult> AddGroup(AddGroup groupDetails)
        {
            try
            {
                _unitOfWork.GetStageRepository().AddGroup(groupDetails.GroupName, groupDetails.StageId);
                await _unitOfWork.Save();
                return Ok();
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}