using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatabaseWebApp.Controllers
{
    public class RemovePersonController : Controller
    {
        public ActionResult Index()
        {
            if (Convert.ToBoolean(Session["loginsuccess"]))
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { signinpass = false });
            }
        }
    }
}