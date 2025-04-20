using Dapper;
using Lab.DTO.Masters.Interfaces;
using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.DALDapper.Implimantation.Masters
{
    public class DALEmployee : IMstEmployee
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        public List<DTOEmployee> GetAll(string comid)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            try
            {
                string query = "SELECT * FROM MST_EMPLOYEE WHERE COM_ID = @comid";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    return con.Query<DTOEmployee>(query, new { comid }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving MST_EMPLOYEE data", ex);
            }
        }
        public Int64 Create(DTOEmployee _objDtoEmployee)
        {
            try
            {
                string query = @"INSERT INTO MST_EMPLOYEE (EMP_ID,EMP_NAME,EMP_CONTACT,EMP_PASSWORD,COM_ID)";
                query = query + " VALUES(@EMP_ID,@EMP_NAME,@EMP_CONTACT,@EMP_PASSWORD,@COM_ID);SELECT @EMP_ID";
                Int64 i;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i = con.Query<Int64>(query, _objDtoEmployee).Single();
                }
                return i;
            }
            catch
            {
                throw;
            }
        }

        public Int64 Edit(DTOEmployee _objDtoEmployee)
        {
            try
            {
                string query = @"UPDATE MST_EMPLOYEE SET ";
                query += " EMP_NAME = '" + _objDtoEmployee.EMP_NAME + "'";
                query += " ,EMP_CONTACT = '" + _objDtoEmployee.EMP_CONTACT + "'";
                query += " ,EMP_PASSWORD = '" + _objDtoEmployee.EMP_PASSWORD + "'";
                query += " WHERE EMP_ID = '" + _objDtoEmployee.EMP_ID + "'";
                Int64 i;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i = con.Execute(query, _objDtoEmployee);
                }
                return _objDtoEmployee.EMP_ID;
            }
            catch
            {
                throw;
            }
        }

        public Int64 Delete(DTOEmployee _objDelete)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"DELETE FROM MST_EMPLOYEE WHERE EMP_ID = @EMP_ID";

                Int64 i = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    i = con.Execute(query, new
                    {
                        EMP_ID = _objDelete.EMP_ID
                    });
                }

                return _objDelete.EMP_ID;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while permanently deleting record: " + ex.Message, ex);
            }
        }

        public DTOEmployee GetExisting(Int64 code)
        {
            try
            {
                string query = "SELECT * FROM MST_EMPLOYEE WHERE EMP_ID = @code";
                DTOEmployee lst = new DTOEmployee();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    lst = con.Query<DTOEmployee>(query, new { code }).FirstOrDefault();
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetLastEmployeeIdForFixedParts(string fixedPart, string fixedPartSec)
        {
            string lastTestId = null;

            string likePattern = fixedPart + fixedPartSec + "%";

            string query = "SELECT TOP 1 EMP_ID FROM MST_EMPLOYEE WHERE EMP_ID LIKE @likePattern ORDER BY EMP_ID DESC";

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

        public List<DTOEmployee> GetEmployeeList(string comid)
        {
            try
            {
                string query = @"SELECT * FROM MST_EMPLOYEE WHERE COM_ID = @comid ORDER BY EMP_ID";

                List<DTOEmployee> lst;

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    //lst = con.Query<DTODoctor>(query).ToList();
                    lst = con.Query<DTOEmployee>(query, new { ComId = comid }).ToList();
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
