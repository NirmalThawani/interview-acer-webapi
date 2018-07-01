using System.Web.Http;
using InterviewAcer.Repository.Contract;
using InterviewAcer.Repository.Implementation;
using InterviewAcer.Repository;
using System.Threading.Tasks;
using InterviewAcer.Common.DTO;
using System;
using System.Security.Claims;
using System.Linq;

namespace InterviewAcer.Controllers
{
    public class InterviewController : ApiController
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public InterviewController()
        {
            _unitOfWork = new UnitOfWork();
        }


        /// <summary>
        /// Gets the list of interviews added by the logged In User
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/GetInterview")]
        public async Task<IHttpActionResult> GetInterviewDetails()
        {
            try
            {
                var claimsIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
                var userName = claimsIdentity.Claims.Where(x => x.Type == "sub").Select(y => y.Value).SingleOrDefault();
                var interviewDetails = await _unitOfWork.GetInterviewRepository().GetInterviewDetails(userName);
                if (interviewDetails == null || interviewDetails.Count == 0)
                {
                    return NotFound();
                }
                return Ok(interviewDetails);
            }
            catch
            {
                return InternalServerError();
            }
        }



        /// <summary>
        /// Saves the interview details against the logged in user
        /// </summary>
        /// <param name="interviewDetails"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("api/SaveInterview")]
        public async Task<IHttpActionResult> SaveInterviewDetails(InterviewDetailsDTO interviewDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (interviewDetails != null)
                {
                    var claimsIdentity = RequestContext.Principal.Identity as ClaimsIdentity;
                    var userName = claimsIdentity.Claims.Where(x => x.Type == "sub").Select(y => y.Value).SingleOrDefault();
                    _unitOfWork.GetInterviewRepository().SaveInterviewDetails(interviewDetails, userName);
                    await _unitOfWork.Save();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }

       
    }
}
