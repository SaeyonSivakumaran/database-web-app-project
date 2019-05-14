using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseWebApp.Models
{
    public class Person
    {
        // Instance attributes
        public string name;
        public string age;
        public string sex;
        public string location;
        public string school;
        public int id;

        // Constructor for Person class
        public Person(string name, string age, string sex, string location, string school, int id)
        {
            this.name = name;
            this.age = age;
            this.sex = sex;
            this.location = location;
            this.school = school;
            this.id = id;
        }
    }
}