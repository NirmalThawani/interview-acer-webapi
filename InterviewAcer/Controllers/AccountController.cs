﻿using InterviewAcer.Models;
using InterviewAcer.Repository.Contract;
using InterviewAcer.Repository.Implementation;
using InterviewAcer.RequestClasses;
using InterviewAcer.ResponseClasses;
using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
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
            catch(Exception e)
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
            catch(Exception e)
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
            if(!ModelState.IsValid)
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
            catch(Exception e)
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

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
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
