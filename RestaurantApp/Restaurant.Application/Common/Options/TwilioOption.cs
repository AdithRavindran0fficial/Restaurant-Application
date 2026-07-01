using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Common.Options
{
    public class TwilioOption
    {
        public TwilioOption()
        {
            
        }
        public const string SectionName = "Twilio";
        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string VerifyServiceSid { get; set; } = string.Empty;
    }
}
