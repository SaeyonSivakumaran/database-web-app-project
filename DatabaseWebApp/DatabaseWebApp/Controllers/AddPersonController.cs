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

            return View();
        }

        [HttpPost]
        public ActionResult AddPerson(FormCollection formInputs)
        {
            // Getting information from view
            string name = formInputs["name"].ToString();
            string age = formInputs["age"].ToString();
            string gender = formInputs["gender"].ToString();
            string location = formInputs["location"].ToString();
            string school = formInputs["school"].ToString();

            // Creating SQL Connection
            string conStr = "Data Source=LAPTOP-L1T7M28M;Initial Catalog=PeopleDatabase;Integrated Security=True";
            SqlConnection connection = new SqlConnection(conStr);
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

    }
}