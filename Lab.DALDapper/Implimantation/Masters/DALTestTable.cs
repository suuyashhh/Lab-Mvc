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
    public class DALTestTable : IMstTestTable
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public Int64 Create(DTOTestTable _objTestTestDetails)
        {
            try
            {
                string query = @"INSERT INTO MST_TRN_TEST (TEST_CODE,TRN_NO,SR_NO,PRICE,LAB_PRICE)";
                query = query + " VALUES(@TEST_CODE,@TRN_NO,@SR_NO,@PRICE,@LAB_PRICE);SELECT @TRN_NO";
                Int64 i;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i = con.Query<Int64>(query, _objTestTestDetails).Single();
                }
                return i;
            }
            catch
            {
                throw;
            }
        }
    }
}
