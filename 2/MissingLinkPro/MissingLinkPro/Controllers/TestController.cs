using IdentitySample.Models;
using System.Web.Mvc;

namespace MissingLinkPro.Controllers
{
    public class TestController : HttpsBaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Seed()
        {
            ApplicationDbInitializer.InitializeIdentityForEF(db);

            return Content("okay");
        }

        public ActionResult IDKFA()
        {
            return Content("ALL WEAPONS, KEYS, AMMO UNLOCKED");
        }
	}
}