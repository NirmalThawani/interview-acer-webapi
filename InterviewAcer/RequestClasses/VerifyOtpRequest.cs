﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewAcer.RequestClasses
{
    public class VerifyOtpRequest
    {
        public string OTP { get; set; }
        public string UserId { get; set; }
    }
}