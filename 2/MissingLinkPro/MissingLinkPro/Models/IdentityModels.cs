using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using MissingLinkPro.Models;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Searches This Month")]
        public int QueriesPerformed { get; set; }
        [Display(Name = "Total Lifetime Queries")]
        public int TotalQueriesPerformed { get; set; }
        [Display(Name = "DateTime: Last Query")]
        public DateTime DateTimeStamp { get; set; }
        [Display(Name = "Activation Date")]
        public DateTime Anniversary { get; set; }
        [Display(Name = "Package")]
        public int? PackageId { get; set; }
        [Display(Name = "Package")]
        public virtual Package Package { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Setting
    {
        public int Id { get; set; }
        [Display(Name = "Setting")]
        public string SettingName { set; get; }
        [Display(Name = "Value")]
        public string Value { get; set; }
    }

    public class Package
    {
        [Display(Name = "Package ID")]
        public int Id { get; set; }
        [Display(Name = "Package")]
        public string Name { get; set; }
        [Display(Name = "Searches/Month")]
        public int SearchesPerMonth { get; set; }
        [Display(Name = "Max Results")]
        public int MaxResults { get; set; }
        [Display(Name = "Cost/Month")]
        public decimal CostPerMonth { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Package> Packages { get; set; }
        //public DbSet<SearchResult> SearchResults { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}