using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Backend.Models;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration configuration;

        public UsersController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpPost]
        [Route("Login")]
        public JsonResult Login(Users user)
        {
            string query = @"select * from userslogin where Email = '" + user.Email + "' and password = '" + user.Password + "'";

            string sqlDataSource = configuration.GetConnectionString("LoginApp");
            SqlDataReader myReader;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, conn))
                {
                    myReader = myCommand.ExecuteReader();

                    if (myReader.Read())
                    {
                        conn.Close();
                        return new JsonResult("Login Successful");
                    }
                    else
                    {
                        conn.Close();
                        return new JsonResult("Invalid Credentials");
                    }
                }
            }
        }

        [Route("GetByEmail")]
        [HttpGet("{Email}")]
        public JsonResult GetByEmail(string Email)
        {
            string query = @"select * from userslogin where Email = '" + Email + "'";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("LoginApp");
            SqlDataReader myReader;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, conn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    conn.Close();
                }
            }

            return new JsonResult(table);
        }

        

        [HttpPost]
        [Route("Add")]
        public JsonResult Add(Users user)
        {
            string query = @"insert into userslogin(FirstName, LastName, Email, Password, City) 
                            values(@FirstName, @LastName, @Email, @Password, @City)";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("LoginApp");
            SqlDataReader myReader;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, conn))
                {
                    myCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", user.LastName);
                    myCommand.Parameters.AddWithValue("@Email", user.Email);
                    myCommand.Parameters.AddWithValue("@Password", user.Password);
                    myCommand.Parameters.AddWithValue("@City", user.City);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    conn.Close();
                }
            }

            return new JsonResult("Data Added Successfully");
        }

        [HttpPut("{Email}")]
        [Route("Update")]
        public JsonResult Update(string Email, Users user)
        {
            string query = @"update userslogin set FirstName = @FirstName, LastName = @LastName, Password = @Password, City = @City where Email = '"+Email+"'";

            DataTable table = new DataTable();
            string sqlDataSource = configuration.GetConnectionString("LoginApp");
            SqlDataReader myReader;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, conn))
                {
                    myCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", user.LastName);
                    myCommand.Parameters.AddWithValue("@Password", user.Password);
                    myCommand.Parameters.AddWithValue("@City", user.City);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    conn.Close();
                }
            }

            return new JsonResult("Updation done Successfully");
        }

        

    }
}
