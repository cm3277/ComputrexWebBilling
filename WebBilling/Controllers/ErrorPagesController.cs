using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBilling.Controllers
{
    public class ErrorPagesController : Controller
    {
        //
        // GET: /ErrorPages/
        public ActionResult Error()
        {
            return View();
        }
	}
}