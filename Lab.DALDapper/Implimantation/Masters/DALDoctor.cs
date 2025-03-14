using Lab.DTO.Masters.Interfaces;
using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Lab.DALDapper.Implimantation.Masters
{
    public class DALDoctor: IMstDoctor
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        public List<DTODoctor> GetAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            try
            {
                string query = "SELECT * FROM MST_DOCTOR";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    return con.Query<DTODoctor>(query).ToList();
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
                string query = @"INSERT INTO MST_DOCTOR (DOCTOR_CODE,DOCTOR_NAME,DOCTOR_ADDRESS,DOCTOR_NUMBER)";
                query = query + " VALUES(@DOCTOR_CODE,@DOCTOR_NAME,@DOCTOR_ADDRESS,@DOCTOR_NUMBER);SELECT @DOCTOR_CODE";
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

        public List<DTODoctor> GetDoctorList()
        {
            try
            {
                string query = @"SELECT * FROM MST_DOCTOR ORDER BY DOCTOR_CODE";

                List<DTODoctor> lst;

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    lst = con.Query<DTODoctor>(query).ToList();
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
