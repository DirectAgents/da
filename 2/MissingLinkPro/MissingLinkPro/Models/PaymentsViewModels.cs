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
}