using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "DirectAgents";
            ViewBag.Title = "DirectLink | Linkless Mention Finder";
            ViewBag.MetaDescription = "DirectLink is the first enterprise level tool that finds out who’s talking about your brand without linking back. Try it free!";
            return View();
        }

        public ActionResult DirectAgents()
        {
            return Redirect("http://www.directagents.com/");
        }

        public ActionResult About()
        {
            ViewBag.Message = "DirectAgents";
            ViewBag.Title = "About DirectLink";
            ViewBag.MetaDescription = "DirectLink is the first enterprise level tool that finds out who’s talking about your brand without linking back. Try it free!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Manual()
        {
            ViewBag.Message = "User Manual";

            return View();
        }
    }
}
