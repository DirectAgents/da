using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MissingLinkPro.Controllers
{
    [AllowAnonymous]
    public class DebugController : Controller
    {
        //
        // GET: /Debug/
        public ActionResult Index()
        {
            return View();
        }
	}
}