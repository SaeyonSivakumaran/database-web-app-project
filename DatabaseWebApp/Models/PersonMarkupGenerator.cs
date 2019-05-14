using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;

namespace DatabaseWebApp.Models
{
    public class PersonMarkupGenerator
    {

        List<Person> personList;
        public PersonMarkupGenerator()
        {
            this.personList = new List<Person>();
        }

        public void setPeople(string connectionStr)
        {
            // Creating SQL Connection
            string conStr = connectionStr;
            SqlConnection connection = new SqlConnection(conStr);
            connection.Open();

            // Creating the SQL Command and reading the database
            string query = "SELECT * FROM InfoTable";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader["name"].ToString();
                string age = reader["age"].ToString();
                string gender = reader["sex"].ToString();
                string location = reader["location"].ToString();
                string school = reader["school"].ToString();
                string id = reader["id"].ToString();
                personList.Add(new Person(name, age, gender, location, school, Int32.Parse(id)));
            }
        }

        public string generateMarkup()
        {
            // Creating a markup string 
            string markupStr = "<table style='width: 100%'>" +
                "<tr><th>Name</th><th>Age</th><th>Gender</th><th>Location</th><th>School</th><th>ID</th></tr>";
            StringBuilder table = new StringBuilder(markupStr);

            // Adding each person from the database as a row in our markup
            foreach (Person person in personList)
            {
                table.Append($"<tr><td>{person.name}</td>" +
                    $"<td>{person.age}</td>" +
                    $"<td>{person.sex}</td>" +
                    $"<td>{person.location}</td>" +
                    $"<td>{person.school}</td>" +
                    $"<td>{person.id}</td>");
            }
            table.Append("</table>");
            return table.ToString();

        }
    }
}