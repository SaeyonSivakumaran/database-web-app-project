using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using DatabaseWebApp.Models;

namespace DatabaseWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(FormCollection formInputs)
        {
            try
            {
                // Creating the connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = formInputs["server"];
                builder.UserID = formInputs["user"];
                builder.Password = formInputs["pass"];
                builder.InitialCatalog = formInputs["database"];
                string conStr = builder.ConnectionString;
                // Generating a markup string to use in the view
                PersonMarkupGenerator markupGen = new PersonMarkupGenerator();
                markupGen.setPeople(conStr);
                string markupStr = markupGen.generateMarkup();
                ViewData["markup"] = markupStr;

                return View();
            } catch(Exception e)
            {
                return RedirectToAction("SignIn", "Home", new { signinpass = false });
            }
        }

        public ActionResult SignIn(bool signinpass = true)
        {
            if (!signinpass)
            {
                ViewData["errormsg"] = "Database Connection Failed";
                return View();
            } else
            {
                ViewData["errormsg"] = "";
                return View();
            }         
        }

    }
}