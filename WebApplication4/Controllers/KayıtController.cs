﻿using Microsoft.AspNetCore.Http;
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
            kayıt.date = kayıt.date.ToLocalTime(); //saat eşitlemesi için.
           
            
            string dbConnectionString = _configuration.GetConnectionString("KayitDbConnection");
            SqlConnection con = new SqlConnection(dbConnectionString);
            SqlCommand cmd = new SqlCommand("KisilerEkle", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", kayıt.name);
            cmd.Parameters.AddWithValue("@surname", kayıt.surname);
            cmd.Parameters.AddWithValue("@gender", kayıt.gender);
            cmd.Parameters.AddWithValue("@mail", kayıt.mail);
            cmd.Parameters.AddWithValue("@date", kayıt.date);
            cmd.Parameters.AddWithValue("@jobID", kayıt.JobID);
           

                       
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

             
                string sqlQuery = "KisilerSil";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
            //SqlCommand cmd = new SqlCommand("SELECT  k.ID,k.name,k.surname,k.gender,k.mail,k.date from TblKisiler k left join TblJobs j on k.ID = j.ID", con);
            SqlCommand cmd = new SqlCommand("KisilerListele", con);
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
                kayıt.JobID= (int)reader["jobID"];
              
                kayıt.Jobname = reader["jobName"].ToString();

                kayıtlar.Add(kayıt);
            }
            con.Close();
            return kayıtlar;
        }
     


        



    }
    }

