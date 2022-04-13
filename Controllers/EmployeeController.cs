using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
namespace employ.Controllers {

    [Route ("[controller]")]
    [Produces ("application/json")]
    public class EmployeeController : ControllerBase {

        public class Employee {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

        }
        private readonly IConfiguration _configuration;

        public EmployeeController (IConfiguration configuration) {
            _configuration = configuration;
        }
        //https: //localhost:5001/Employee/GetData
            [HttpGet ("GetData")]
        public IEnumerable<Employee> Get () {
            //Console.WriteLine ("1");
            var employees = GetEmployees ();
            return employees;
        }

        private IEnumerable<Employee> GetEmployees () {
            var employees = new List<Employee> ();

            using (var connection = new SqlConnection (_configuration.GetConnectionString ("EmployeeDatabase"))) {
                var sql = "SELECT Id, FirstName, LastName, Email, PhoneNumber FROM Employee";

                connection.Open ();
                using SqlCommand command = new SqlCommand (sql, connection);
                using SqlDataReader reader = command.ExecuteReader ();

                while (reader.Read ()) {
                    var employee = new Employee () {
                        Id = (long) reader["Id"],
                        FirstName = reader["FirstName"].ToString (),
                        LastName = reader["LastName"].ToString (),
                        Email = reader["Email"].ToString (),
                        PhoneNumber = reader["PhoneNumber"].ToString (),
                    };

                    employees.Add (employee);
                }
            }

            return employees;
        }
    }
}