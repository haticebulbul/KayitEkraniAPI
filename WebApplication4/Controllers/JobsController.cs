using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class JobsController : ControllerBase
    {
      
      
        private readonly IConfiguration _configuration;

        public JobsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Jobs")]

        public IEnumerable<Jobs> GetJobs()
        {

            string dbConnectionString = _configuration.GetConnectionString("JobsDbConnection");
            SqlConnection con = new SqlConnection(dbConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM TblJobs", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Jobs> jobs = new List<Jobs>();
            while (reader.Read())
            {
                Jobs job = new Jobs();
                job.ID = (int)reader["ID"];
                job.name = reader["name"].ToString();
              

                jobs.Add(job);
            }
            con.Close();
            return jobs;
        }

    }
}
