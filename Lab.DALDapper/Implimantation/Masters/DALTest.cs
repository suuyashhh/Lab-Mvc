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
    public class DALTest : IMSTTest
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        public async Task<List<DTOTest>> GetTestsAsync(string searchtext)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TOP 10 TEST_CODE, TEST_NAME, PRICE, LAB_PRICE 
                                 FROM MST_TEST 
                                 WHERE TEST_NAME LIKE @SearchText + '%' 
                                 ORDER BY TEST_NAME";

                var parameters = new { SearchText = searchtext };

                var tests = (await db.QueryAsync<DTOTest>(query, parameters)).ToList();

                return tests;
            }
        }
    }
    }
