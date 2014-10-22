using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IdentitySample.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Runs Performed Today")]
        [Range(0, Int32.MaxValue, ErrorMessage = "This field cannot be less than 0.")]
        public int QueriesPerformed { get; set; }

        [Display(Name = "Lifetime Runs")]
        public int TotalQueriesPerformed { get; set; }

        [Display(Name = "Last Query")]
        public DateTime LastQueryTime { get; set; }

        [Display(Name = "Package")]
        public int? PackageId { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}