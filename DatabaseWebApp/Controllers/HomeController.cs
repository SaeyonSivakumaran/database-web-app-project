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
            // Checking if the user is logged in
            if (Convert.ToBoolean(Session["loginsuccess"]))
            {
                // Generating a markup string to use in the view
                PersonMarkupGenerator markupGen = new PersonMarkupGenerator();
                markupGen.setPeople(Session["connection"].ToString());
                string markupStr = markupGen.generateMarkup();
                ViewData["markup"] = markupStr;
                Session["tablemarkup"] = markupStr;
                return View();
            }
            else
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
                    Session["connection"] = conStr;
                    Session["loginsuccess"] = true;
                    return View();
                }
                catch (Exception e)
                {
                    return RedirectToAction("SignIn", "Home", new { signinpass = false });
                }
            }
        }

        public ActionResult SignIn(bool signinpass = true)
        {
            if (!signinpass)
            {
                ViewData["errormsg"] = "Database Connection Failed";
                return View();
            }
            else
            {
                ViewData["errormsg"] = "";
                return View();
            }
        }

        public ActionResult AddPerson(string name, string age, string gender, string location, string school)
        {
            // Creating SQL Connection
            SqlConnection connection = new SqlConnection(Session["connection"].ToString());
            connection.Open();

            // Creating the SQL Command
            string query = "INSERT INTO InfoTable VALUES(@name, @age, @gender, @location, @school)";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@age", age);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@school", school);

            //Execute SQL query
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            // Reset session values and redirect to sign in page
            Session["loginsuccess"] = false;
            Session["connection"] = null;
            return RedirectToAction("SignIn");
        }

    }
}