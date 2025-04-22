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
using System.Security.Cryptography;

namespace Lab.DALDapper.Implimantation.Masters
{
    public class DALTest : IMSTTest
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        public List<DTOTest> GetAll(string comid)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            try
            {
                string query = "SELECT * FROM MST_TEST WHERE COM_ID = @comid";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    return con.Query<DTOTest>(query, new { comid }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving MST_TEST data", ex);
            }
        }
        public async Task<List<DTOTest>> GetTestsAsync(string searchtext, string comid)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TEST_CODE, TEST_NAME, PRICE, LAB_PRICE 
                         FROM MST_TEST 
                         WHERE TEST_NAME LIKE @SearchText + '%' 
                         AND COM_ID = @CompanyId
                         ORDER BY TEST_NAME";

                var parameters = new
                {
                    SearchText = searchtext,
                    CompanyId = comid
                };

                var tests = (await db.QueryAsync<DTOTest>(query, parameters)).ToList();

                return tests;
            }
        }

        public Int64 Create(DTOTest _objDtoTest)
        {
            try
            {
                string query = @"INSERT INTO MST_TEST (TEST_CODE,TEST_NAME,PRICE,LAB_PRICE,COM_ID,CRT_BY,STATUS_CODE)";
                query = query + " VALUES(@TEST_CODE,@TEST_NAME,@PRICE,@LAB_PRICE,@COM_ID,@CRT_BY,@STATUS_CODE);SELECT @TEST_CODE";
                Int64 i;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i= con.Query<Int64>(query, _objDtoTest).Single();                    
                }
                return i;
            }
            catch
            {
                throw;
            }
        }

        public Int64 Edit(DTOTest _objDtoTest)
        {
            try
            {
                string query = @"UPDATE MST_TEST SET ";
                query += " TEST_NAME = '" + _objDtoTest.TEST_NAME + "'";
                query += " ,PRICE = '" + _objDtoTest.PRICE + "'";
                query += " ,LAB_PRICE = '" + _objDtoTest.LAB_PRICE + "'";
                query += " WHERE TEST_CODE = '" + _objDtoTest.TEST_CODE + "'";
                Int64 i;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i = con.Execute(query, _objDtoTest);
                }
                return _objDtoTest.TEST_CODE;
            }
            catch
            {
                throw;
            }
        }

        public Int64 Delete(DTOTest _objDelete)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"DELETE FROM MST_TEST WHERE TEST_CODE = @TEST_CODE";

                Int64 i = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    i = con.Execute(query, new
                    {
                        TEST_CODE = _objDelete.TEST_CODE
                    });
                }

                return _objDelete.TEST_CODE;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while permanently deleting record: " + ex.Message, ex);
            }
        }
        public DTOTest GetExisting(Int64 code)
        {
            try
            {
                string query = "SELECT * FROM MST_TEST WHERE TEST_CODE = @code";
                DTOTest lst = new DTOTest();
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    lst = con.Query<DTOTest>(query, new { code }).FirstOrDefault();
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetLastTestIdForFixedParts(string fixedPart, string fixedPartSec)
        {
            string lastTestId = null;

            string likePattern = fixedPart + fixedPartSec + "%";

            string query = "SELECT TOP 1 TEST_CODE FROM MST_TEST WHERE TEST_CODE LIKE @likePattern ORDER BY TEST_CODE DESC";

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

    }
    }
