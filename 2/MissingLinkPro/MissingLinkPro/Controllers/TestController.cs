using IdentitySample.Models;
using System.Web.Mvc;

namespace MissingLinkPro.Controllers
{
    public class TestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Seed()
        {
            ApplicationDbInitializer.InitializeIdentityForEF(db);

            return Content("okay");
        }
	}
}