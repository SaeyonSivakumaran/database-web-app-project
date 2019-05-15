using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseWebApp.Models;
using System.Data.SqlClient;

namespace DatabaseWebApp.Controllers
{
    public class AddPersonController : Controller
    {
        public ActionResult Index()
        {
            if (Convert.ToBoolean(Session["loginsuccess"]))
            {
                return View();
            } else
            {
                return RedirectToAction("SignIn", "Home", new { signinpass = false });
            }
            
        }

        [HttpPost]
        public ActionResult AddPerson(FormCollection formInputs)
        {
            // Getting information from view
            string name1 = formInputs["name"].ToString();
            string age1 = formInputs["age"].ToString();
            string gender1 = formInputs["gender"].ToString();
            string location1 = formInputs["location"].ToString();
            string school1 = formInputs["school"].ToString();
            return RedirectToAction("AddPerson", "Home", new { name = name1, age = age1, gender = gender1, location = location1, school = school1 });
        }

    }
}