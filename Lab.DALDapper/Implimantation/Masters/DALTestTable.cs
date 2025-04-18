﻿using Lab.DTO.Masters.Interfaces;
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
                string query = @"INSERT INTO MST_TRN_TEST (TEST_CODE,TRN_NO,SR_NO,PRICE,LAB_PRICE,COM_ID)";
                query = query + " VALUES(@TEST_CODE,@TRN_NO,@SR_NO,@PRICE,@LAB_PRICE,@COM_ID);SELECT @TRN_NO";
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

        public DTOTestTable GetExisting(Int64 TEST_CODE)
        {
            string query = "SELECT MT.TEST_NAME, MTT.* FROM MST_TRN_TEST MTT INNER JOIN MST_TEST MT ON MT.TEST_CODE = MTT.TEST_CODE WHERE MTT.TEST_CODE = @TEST_CODE";
            DTOTestTable lst = new DTOTestTable();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                lst = con.Query<DTOTestTable>(query, new { TEST_CODE }).FirstOrDefault();
            }

            return lst;

        }

        public List<DTOTestTable> GetITableList(Int64 Trnno)
        {
            try
            {
                string query = "SELECT MT.TEST_NAME, MTT.* FROM MST_TRN_TEST MTT INNER JOIN MST_TEST MT ON MT.TEST_CODE = MTT.TEST_CODE WHERE MTT.TRN_NO =@TRN_NO ORDER BY MTT.SR_NO";
                List<DTOTestTable> lst = new List<DTOTestTable>();

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    lst = con.Query<DTOTestTable>(query, new { TRN_NO = Trnno }).ToList();
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 DelPermenantData(Int64 TestCode)
        {
            try
            {
                string query = "DELETE FROM MST_TRN_TEST WHERE TRN_NO = @TRN_NO";
                Int64 i = 0;

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    i = con.Execute(query, new { TRN_NO = TestCode });
                }

                return TestCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting record: " + ex.Message, ex);
            }
        }


    }
}
