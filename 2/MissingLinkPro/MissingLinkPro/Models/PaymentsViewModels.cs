using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class PayViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName;
        [Display(Name = "Last Name")]
        public string LastName;
        [Display(Name = "Current Package")]
        public int SubscribedPackageId;

        public int PackageId;
        public string PackageName;
        public decimal PackageCost;
        public int SearchesPerMonth;
        public int MaxResults;

        public bool UserHasCreditCard { get; set; }
        public bool IsActive { get; set; }
    }

    public class PostPayViewModel
    {
        public string stripeToken { get; set; }
        public int PackageId { get; set; }
    }

    public class PayErrorModel
    {
        public string Error { get; set; }
    }

    public class PostCancelModel
    {
        public int PackageId { get; set; }
    }
}