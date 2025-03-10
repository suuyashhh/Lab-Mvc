using Lab.DTO.Masters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab.DTO.Masters.Objects;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data;


namespace Lab.DALDapper.Implimantation.Masters
{
    public class DALCasePaper : IMstCasePaper
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public List<DTOCasePaper> GetAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            try
            {
                string query = "SELECT * FROM MST_PATIENT";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open(); 
                    return con.Query<DTOCasePaper>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving MST_PATIENT data", ex);
            }
        }
        public Int64 Create(DTOCasePaper _objDtoCasePaper) 
        {
            try
            {
                Int64 patientId = 0; 
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"INSERT INTO MST_PATIENT (TRN_NO, PATIENT_NAME, GENDER, CON_NUMBER, DOCTOR_REF,DISCOUNT,TOTAL_PROFIT,TOTAL_AMOUNT) 
                         VALUES (@TRN_NO, @PATIENT_NAME, @GENDER, @CON_NUMBER, @DOCTOR_REF,@DISCOUNT,@TOTAL_PROFIT,@TOTAL_AMOUNT); 
                         SELECT @TRN_NO;"; 

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    patientId = con.Query<Int64>(query, _objDtoCasePaper).Single(); 
                }

                return patientId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while inserting patient record: " + ex.Message, ex);
            }
        }


        public async Task<string> GetLastPatientIdForDate(string datePart)
        {
            string lastPatientId = null;
            string query = "SELECT TOP 1 TRN_NO FROM MST_PATIENT WHERE TRN_NO LIKE @datePart + '%' ORDER BY TRN_NO DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@datePart", datePart);
                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                        lastPatientId = result.ToString();
                }
            }
            return lastPatientId;
        }


        public DTOCasePaper GetExisting(Int64 code)
        {
            try
            {
                string query = "SELECT * FROM MST_PATIENT WHERE TRN_NO = @code";
                DTOCasePaper lst = new DTOCasePaper();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    lst = con.Query<DTOCasePaper>(query, new { code }).FirstOrDefault();
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }       
}
