﻿using Lab.DTO.Masters.Interfaces;
using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Security.Cryptography;

namespace Lab.DALDapper.Implimantation.Masters
{
    public class DALDoctor: IMstDoctor
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        public List<DTODoctor> GetAll(string comid)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            try
            {
                string query = "SELECT * FROM MST_DOCTOR WHERE COM_ID = @comid";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    return con.Query<DTODoctor>(query, new { comid }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving MST_DOCTOR data", ex);
            }
        }
        public Int64 Create(DTODoctor _objDtoDoctor)
        {
            try
            {
                string query = @"INSERT INTO MST_DOCTOR (DOCTOR_CODE,DOCTOR_NAME,DOCTOR_ADDRESS,DOCTOR_NUMBER,COM_ID,CRT_BY)";
                query = query + " VALUES(@DOCTOR_CODE,@DOCTOR_NAME,@DOCTOR_ADDRESS,@DOCTOR_NUMBER,@COM_ID,@CRT_BY);SELECT @DOCTOR_CODE";
                Int64 i;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i = con.Query<Int64>(query, _objDtoDoctor).Single();
                }
                return i;
            }
            catch
            {
                throw;
            }
        }

        public Int64 Edit(DTODoctor _objDtoDoctor)
        {
            try
            {
                string query = @"UPDATE MST_DOCTOR SET ";
                query += " DOCTOR_NAME = '" + _objDtoDoctor.DOCTOR_NAME + "'";
                query += " ,DOCTOR_ADDRESS = '" + _objDtoDoctor.DOCTOR_ADDRESS + "'";
                query += " ,DOCTOR_NUMBER = '" + _objDtoDoctor.DOCTOR_NUMBER + "'";
                query += " WHERE DOCTOR_CODE = '" + _objDtoDoctor.DOCTOR_CODE + "'";
                Int64 i;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i = con.Execute(query, _objDtoDoctor);
                }
                return _objDtoDoctor.DOCTOR_CODE;
            }
            catch
            {
                throw;
            }
        }

        public Int64 Delete(DTODoctor _objDelete)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"DELETE FROM MST_DOCTOR WHERE DOCTOR_CODE = @DOCTOR_CODE";

                Int64 i = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    i = con.Execute(query, new
                    {
                        DOCTOR_CODE = _objDelete.DOCTOR_CODE
                    });
                }

                return _objDelete.DOCTOR_CODE;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while permanently deleting record: " + ex.Message, ex);
            }
        }

        public DTODoctor GetExisting(Int64 code)
        {
            try
            {
                string query = "SELECT * FROM MST_DOCTOR WHERE DOCTOR_CODE = @code";
                DTODoctor lst = new DTODoctor();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    lst = con.Query<DTODoctor>(query, new { code }).FirstOrDefault();
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetLastDoctorIdForFixedParts(string fixedPart, string fixedPartSec)
        {
            string lastTestId = null;
                        
            string likePattern = fixedPart + fixedPartSec + "%";

            string query = "SELECT TOP 1 DOCTOR_CODE FROM MST_DOCTOR WHERE DOCTOR_CODE LIKE @likePattern ORDER BY DOCTOR_CODE DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@likePattern", likePattern);

                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                        lastTestId = result.ToString();
                }
            }
            return lastTestId;
        }

        public List<DTODoctor> GetDoctorList(string comid)
        {
            try
            {
                string query = @"SELECT * FROM MST_DOCTOR WHERE COM_ID = @comid ORDER BY DOCTOR_CODE";

                List<DTODoctor> lst;

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    //lst = con.Query<DTODoctor>(query).ToList();
                    lst = con.Query<DTODoctor>(query, new { ComId = comid }).ToList();
                }

                return lst;
            }
            catch
            {
                throw;
            }
        }


    }
}
