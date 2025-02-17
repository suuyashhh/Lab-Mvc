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


namespace Lab.DALDapper.Implimantation.Masters
{
    public class DALCasePaper : IMstCasePaper
    {
        public Int64 Create(DTOCasePaper _objDtoCasePaper) // Change int → long
        {
            try
            {
                Int64 patientId = 0;  // Change int → long
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"INSERT INTO MST_PATIENT (TRN_NO, PATIENT_NAME, GENDER, CON_NUMBER, DOCTOR_REF) 
                         VALUES (@TRN_NO, @PATIENT_NAME, @GENDER, @CON_NUMBER, @DOCTOR_REF); 
                         SELECT @TRN_NO;";  // Return TRN_NO directly

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    patientId = con.Query<Int64>(query, _objDtoCasePaper).Single();  // Change int → long
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


    }
}
