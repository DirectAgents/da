using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class ContactFormViewModel
    {
        //public bool LeadGeneration { get; set; }
        //public bool AffiliateMarketing { get; set; }
        //public bool PaidSearchAndSEO { get; set; }
        //public bool SocialMedia { get; set; }
        //public bool CreativeDesign { get; set; }
        //public bool MobileMarketing { get; set; }
        //public bool ProgrammaticMediaBuying { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Message { get; set; }

        public bool ApplicationMsgAvailable { get; set; }
        public string ApplicationMsg { get; set; }
    }
}