using Lab.DTO.Masters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab.DTO.Masters.Objects;
using System.Data.SqlClient;

namespace Lab.DALDapper.Implimantation.Masters
{
    public class DALCasePaper : IMstCasePaper
    {
        public int Create(DTOCasePaper _objDtoCasePaper)
        {
            int i=0;
            return i;
            //try
            //{
            //    int patientId = 0;
            //    string connectionString = "Your_Connection_String_Here"; 

            //    string query = @"INSERT INTO LAB_PATIENT (PATIENT_NAME, GENDER, CONTACT, ADDRESS, DOCTOR, DATE) 
            //             VALUES (@PATIENT_NAME, @GENDER, @CONTACT, @ADDRESS, @DOCTOR, @DATE); 
            //             SELECT CAST(SCOPE_IDENTITY() AS INT);"; 

            //    using (SqlConnection con = new SqlConnection(connectionString))
            //    {
            //        con.Open();
            //        patientId = con.Query<int>(query, _objDtoCasePaper).Single();
            //    }

            //    return patientId;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error while inserting patient record: " + ex.Message, ex);
            //}
        }

        public async Task<string> GetLastPatientIdForDate(string datePart)
        {
            string lastPatientId = null;
            //string query = "SELECT TOP 1 PATIENT_ID FROM CasePaper WHERE PATIENT_ID LIKE @datePart + '%' ORDER BY PATIENT_ID DESC";

            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    await conn.OpenAsync();
            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@datePart", datePart);
            //        object result = await cmd.ExecuteScalarAsync();
            //        if (result != null)
            //            lastPatientId = result.ToString();
            //    }
            //}
            return lastPatientId;
        }


    }
}
