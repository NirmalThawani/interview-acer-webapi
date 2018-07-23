using InterviewAcer.Models;
using InterviewAcer.Repository.Contract;
using InterviewAcer.Repository.Implementation;
using InterviewAcer.RequestClasses;
using InterviewAcer.ResponseClasses;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace InterviewAcer.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository.AuthRepository _repo = null;
        private IUnitOfWork _unitOfWork { get; set; }
        public AccountController()
        {
            _repo = new AuthRepository.AuthRepository();
            _unitOfWork = new UnitOfWork();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<HttpResponseMessage> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorResponse();
                error.Error = "Registration Failed";
                error.ErrorDescription = ModelState.Values.First().Errors.First().ErrorMessage;
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, error);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            HttpResponseMessage errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            else
            {
                return await LoginUser(userModel.Email, userModel.Password);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<HttpResponseMessage> LoginUser(string username, string password)
        {
            // Invoke the "token" OWIN service to perform the login: /api/token
            // Ugly hack: I use a server-side HTTP POST because I cannot directly invoke the service (it is deeply hidden in the OAuthAuthorizationServerHandler class)
            var request = HttpContext.Current.Request;
            var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/Token";
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            };
                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;
                var responseMsg = new HttpResponseMessage(responseCode)
                {
                    Content = new StringContent(responseString, Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }
        }

        /// <summary>
        /// if user exists, sends an OTP to registered Email address
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>If user is found, userid is returned. If user is not found NotFound status code is returned</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("FindUserAndSendOTP")]
        public async Task<IHttpActionResult> FindUser(string userName)
        {
            try
            {
                ApplicationUser user = await _repo.FindUser(userName);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    var OTP = _unitOfWork.GetForgotPasswordRepository().SaveOTP(new Common.DTO.ForgotPasswordDTO()
                    {
                        OTP = GetOTP(),
                        UserId = user.Id,
                        TokenCreationDate = DateTime.Now
                    });
                    await _unitOfWork.Save();
                    if (!string.IsNullOrEmpty(user.Email))
                        SendEmail(OTP, user.Email);
                    var userIdResponse = new UserIdResponse() { UserId = user.Id };
                    return Ok(userIdResponse);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        private void SendEmail(string OTP, string userEmailAddress)
        {
            string fromAddress = ConfigurationManager.AppSettings["ResetPasswordMailFromAddress"];
            MailMessage mailMessage = new MailMessage(fromAddress, userEmailAddress);
            mailMessage.Body = "Your OTP for reseting the password is " + OTP;
            mailMessage.Subject = "Reset Password OTP";
            SmtpClient client = new SmtpClient();
            client.Send(mailMessage);
        }

        /// <summary>
        /// verifies the OTP entered from user
        /// </summary>
        /// <param name="verifyOtp"></param>
        /// <returns>If OTP is valid status code 200 is returned. If OTP is not valid status code 404 is returned.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("VerifyOTP")]
        public async Task<IHttpActionResult> VerifyOTP(VerifyOtpRequest verifyOtp)
        {
            try
            {
                var isOTPValid = await _unitOfWork.GetForgotPasswordRepository().VerifyOTP(verifyOtp.OTP, verifyOtp.UserId);
                if (isOTPValid)
                {
                    return Ok("OTP is valid");
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
        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        /// <summary>
        /// Resets the password of the user.
        /// </summary>
        /// <param name="resetPasswordDetails"></param>
        /// <returns>returns 200 status code, if password reset is success</returns>
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordRequest resetPasswordDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var isOTPValid = await _unitOfWork.GetForgotPasswordRepository().VerifyOTP(resetPasswordDetails.OTP, resetPasswordDetails.UserId);
                if (isOTPValid)
                    await _repo.ResetPassword(resetPasswordDetails.NewPassword, resetPasswordDetails.UserId);
                return Ok("password reset successfully");
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SaveUserPersonalInformation")]
        public async Task<IHttpActionResult> SavePersonalInfo(UserPersonalInfo personalInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                IdentityResult result = await _repo.SavePersonalInfo(personalInfo);
                if (!result.Succeeded)
                {
                    if (result.Errors != null)
                    {
                        return BadRequest(result.Errors.First());
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserPeronalInformation")]
        public async Task<IHttpActionResult> GetPersonalInfo(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            try
            {
                UserPersonalInfo personalInfo = new UserPersonalInfo();
                ApplicationUser user = await _repo.FindUserById(userId);
                if(user == null)
                {
                    return NotFound();
                }
                else
                {
                    personalInfo.UserId = userId;
                    personalInfo.Name = user.Name;
                    personalInfo.UniversityName = user.UniversityName;
                    personalInfo.MobileNumber = user.PhoneNumber;
                    personalInfo.Specialization = user.Specialization;
                    personalInfo.AcadamicScore = user.AcadamicScore;
                    personalInfo.CountryCode = user.CountryCode;
                }
                return Ok(personalInfo);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private HttpResponseMessage GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    ErrorResponse error = new ErrorResponse();
                    error.Error = "Registration Failed";
                    error.ErrorDescription = result.Errors.First();
                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, error);
                }


                // No ModelState errors are available to send, so just return an empty BadRequest.
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            }

            return null;
        }

        private string GetOTP()
        {
            var token = new StringBuilder();

            //Prepare a 6-character random text
            using (RNGCryptoServiceProvider
                                rngCsp = new RNGCryptoServiceProvider())
            {
                var data = new byte[4];
                for (int i = 0; i < 6; i++)
                {
                    //filled with an array of random numbers
                    rngCsp.GetBytes(data);
                    //this is converted into a character from A to Z
                    var randomchar = Convert.ToChar(
                                              //produce a random number 
                                              //between 0 and 25
                                              BitConverter.ToUInt32(data, 0) % 26
                                              //Convert.ToInt32('A')==65
                                              + 65
                                     );
                    token.Append(randomchar);
                }
            }
            //This will be the password change identifier 
            //that the user will be sent out
            return token.ToString();
        }
    }
}
