using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using WebApplication4.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using Microsoft.EntityFrameworkCore;


namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KayıtController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public KayıtController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Kayıt")]

        public string Kayıt(Kayıt kayıt)
        {
            kayıt.date = kayıt.date.ToLocalTime();
           
            //string dbConnectionString = _configuration.GetValue<string>("ConnectionStrings: Kay�tDbConnection");
            string dbConnectionString = _configuration.GetConnectionString("KayitDbConnection");
            SqlConnection con = new SqlConnection(dbConnectionString);

            SqlCommand cmd = new SqlCommand("INSERT INTO TblKisiler(name, surname, gender, mail, date) VALUES (@name, @surname, @gender, @mail, @date)", con);


            cmd.Parameters.AddWithValue("@name", kayıt.name);
            cmd.Parameters.AddWithValue("@surname", kayıt.surname);
            cmd.Parameters.AddWithValue("@gender", kayıt.gender);
            cmd.Parameters.AddWithValue("@mail", kayıt.mail);
            cmd.Parameters.AddWithValue("@date", kayıt.date);

            
            //cmd.ExecuteNonQuery();
            //SqlCommand cmd = new SqlCommand("INSERT INTO TblKisiler(name,surname,gender,mail,date)VALUES('"   + kayıt.name + "','" + kayıt.surname + "','" + kayıt.gender + "','" + kayıt.mail + "','" + kayıt.date + "')", con);            
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
            {
                return "Data inserted";
            }
            else
            {
                return "Error";
            }
            return "";
        }

        [HttpDelete]
        [Route("Kayıt/{id}")]
        public IActionResult DeleteKayıt(int id)
        {
            string dbConnectionString = _configuration.GetConnectionString("KayitDbConnection");
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();

             
                string sqlQuery = "DELETE FROM TblKisiler WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                       
                        return NoContent();
                    }
                    else
                    {
                       
                        return NotFound();
                    }
                }
            }
        }








        [HttpGet]
        [Route("Kayıt")]

        public IEnumerable<Kayıt> GetKayıt()
        {

            string dbConnectionString = _configuration.GetConnectionString("KayitDbConnection");
            SqlConnection con = new SqlConnection(dbConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM TblKisiler", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Kayıt> kayıtlar = new List<Kayıt>();
            while (reader.Read())
            {
                Kayıt kayıt = new Kayıt();
                kayıt.ID = ( int )reader["ID"];
                kayıt.name = reader["name"].ToString();
                kayıt.surname = reader["surname"].ToString();
                kayıt.gender = reader["gender"].ToString();
                kayıt.mail = reader["mail"].ToString();
                kayıt.date = ((DateTime)reader["date"]).Date;
              
                kayıtlar.Add(kayıt);
            }
            con.Close();
            return kayıtlar;
        }
     


        



    }
    }

