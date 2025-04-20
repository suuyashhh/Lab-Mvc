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

                string query = @"INSERT INTO MST_PATIENT (TRN_NO, PATIENT_NAME,DATE, GENDER, CON_NUMBER, DOCTOR_CODE,DISCOUNT,TOTAL_PROFIT,TOTAL_AMOUNT,STATUS_CODE,ADDRESS,PAYMENT_AMOUNT,PAYMENT_METHOD,COLLECTION_TYPE,CRT_BY,PAYMENT_STATUS,COM_ID) 
                         VALUES (@TRN_NO, @PATIENT_NAME, @DATE, @GENDER, @CON_NUMBER, @DOCTOR_CODE,@DISCOUNT,@TOTAL_PROFIT,@TOTAL_AMOUNT,@STATUS_CODE,@ADDRESS,@PAYMENT_AMOUNT,@PAYMENT_METHOD,@COLLECTION_TYPE,@CRT_BY,@PAYMENT_STATUS,@COM_ID); 
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

        public Int64 Edit(DTOCasePaper _objDtoCasePaper)
        {
            try
            {
                Int64 patientId = 0;
                                
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"UPDATE MST_PATIENT SET ";
                query += " PATIENT_NAME = '" + _objDtoCasePaper.PATIENT_NAME + "'";
                query += " ,GENDER = '" + _objDtoCasePaper.GENDER + "'";
                query += " ,CON_NUMBER = '" + _objDtoCasePaper.CON_NUMBER + "'";
                query += " ,DOCTOR_CODE = '" + _objDtoCasePaper.DOCTOR_CODE + "'";
                query += " ,DISCOUNT = '" + _objDtoCasePaper.DISCOUNT + "'";
                query += " ,TOTAL_PROFIT = '" + _objDtoCasePaper.TOTAL_PROFIT + "'";
                query += " ,TOTAL_AMOUNT = '" + _objDtoCasePaper.TOTAL_AMOUNT + "'";
                query += " ,STATUS_CODE = '" + _objDtoCasePaper.STATUS_CODE + "'";
                query += " ,DATE = '" + _objDtoCasePaper.DATE + "'";
                query += " ,ADDRESS = '" + _objDtoCasePaper.ADDRESS + "'";
                query += " ,PAYMENT_AMOUNT = '" + _objDtoCasePaper.PAYMENT_AMOUNT + "'";
                query += " ,PAYMENT_METHOD = '" + _objDtoCasePaper.PAYMENT_METHOD + "'";
                query += " ,COLLECTION_TYPE = '" + _objDtoCasePaper.COLLECTION_TYPE + "'";
                query += " ,PAYMENT_STATUS = '" + _objDtoCasePaper.PAYMENT_STATUS + "'";
                query += " WHERE TRN_NO = '" + _objDtoCasePaper.TRN_NO + "'";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            patientId = _objDtoCasePaper.TRN_NO; 
                        }
                        else
                        {
                            throw new Exception("No record updated.");
                        }
                    }
                }

                return patientId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating patient record: " + ex.Message, ex);
            }
        }

        public Int64 Delete(DTOCasePaper _objDelete)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"DELETE FROM MST_PATIENT WHERE TRN_NO = @TRN_NO";

                Int64 i = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    i = con.Execute(query, new
                    {
                        TRN_NO = _objDelete.TRN_NO
                    });
                }

                return _objDelete.TRN_NO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while permanently deleting record: " + ex.Message, ex);
            }
        }

        public Int64 InvoiceSave(DTOCasePaper _objDtoCasePaper)
        {
            try
            {
                Int64 InvNo = 0;

                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"UPDATE MST_PATIENT SET ";
                query += " INVOICE_NO = '" + _objDtoCasePaper.INVOICE_NO + "'";
                query += " WHERE TRN_NO = '" + _objDtoCasePaper.TRN_NO + "'";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            InvNo = _objDtoCasePaper.TRN_NO;
                        }
                        else
                        {
                            throw new Exception("No record updated.");
                        }
                    }
                }

                return InvNo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating patient record: " + ex.Message, ex);
            }
        }

        public Int64 Approve(DTOCasePaper _objDtoCasePaper)
        {
            try
            {
                Int64 patientId = 0;

                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"UPDATE MST_PATIENT SET ";
                query += " PATIENT_NAME = '" + _objDtoCasePaper.PATIENT_NAME + "'";
                query += " ,GENDER = '" + _objDtoCasePaper.GENDER + "'";
                query += " ,CON_NUMBER = '" + _objDtoCasePaper.CON_NUMBER + "'";
                query += " ,DOCTOR_CODE = '" + _objDtoCasePaper.DOCTOR_CODE + "'";
                query += " ,DISCOUNT = '" + _objDtoCasePaper.DISCOUNT + "'";
                query += " ,TOTAL_PROFIT = '" + _objDtoCasePaper.TOTAL_PROFIT + "'";
                query += " ,TOTAL_AMOUNT = '" + _objDtoCasePaper.TOTAL_AMOUNT + "'";
                query += " ,STATUS_CODE = '" + _objDtoCasePaper.STATUS_CODE + "'";
                query += " ,DATE = '" + _objDtoCasePaper.DATE + "'";
                query += " ,ADDRESS = '" + _objDtoCasePaper.ADDRESS + "'";
                query += " ,PAYMENT_AMOUNT = '" + _objDtoCasePaper.PAYMENT_AMOUNT + "'";
                query += " ,PAYMENT_METHOD = '" + _objDtoCasePaper.PAYMENT_METHOD + "'";
                query += " ,COLLECTION_TYPE = '" + _objDtoCasePaper.COLLECTION_TYPE + "'";
                query += " ,PAYMENT_STATUS = '" + _objDtoCasePaper.PAYMENT_STATUS + "'";
                query += " WHERE TRN_NO = '" + _objDtoCasePaper.TRN_NO + "'";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            patientId = _objDtoCasePaper.TRN_NO;
                        }
                        else
                        {
                            throw new Exception("No record updated.");
                        }
                    }
                }

                return patientId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating patient record: " + ex.Message, ex);
            }
        }
        public int Approve(string TrnNos)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            try
            {
                string query = @"UPDATE MST_PATIENT SET 
                         STATUS_CODE = 101
                         WHERE TRN_NO IN (" + TrnNos + ")";

                int i = 0;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    i = con.Execute(query);
                }

                return i;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while approving records: " + ex.Message, ex);
            }
        }

        public async Task<string> GetLastPatientIdForDate(string dateComboKey)
        {
            string lastPatientId = null;
            string query = "SELECT TOP 1 TRN_NO FROM MST_PATIENT WHERE TRN_NO LIKE @dateComboKey + '%' ORDER BY TRN_NO DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dateComboKey", dateComboKey);
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

        public List<DTOCasePaper> GetApprovalPendingList(string comid)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = "SELECT * FROM MST_PATIENT WHERE STATUS_CODE = 0 AND COM_ID = @comid ORDER BY TRN_NO DESC";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    return con.Query<DTOCasePaper>(query, new { comid }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching approval pending list: " + ex.Message, ex);
            }
        }


        public async Task<List<DTOCasePaper>> GetDateWiseAllAsync(string strStartDate, string strEndDate, string ComId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

                string query = @"SELECT * 
                         FROM MST_PATIENT 
                         WHERE DATE >= @StartDate 
                         AND DATE <= @EndDate 
                         AND COM_ID = @ComId
                         ORDER BY DATE DESC, TRN_NO DESC";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    return con.Query<DTOCasePaper>(query, new
                    {
                        StartDate = strStartDate,
                        EndDate = strEndDate,
                        ComId = ComId
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching case papers by date: " + ex.Message, ex);
            }
        }

        public async Task<string> GetLastInvoiceNoAsync(string comid)
        {
            string lastInvoiceNo = null;
            string query = "SELECT TOP 1 INVOICE_NO FROM MST_PATIENT WHERE INVOICE_NO IS NOT NULL AND COM_ID = @comid ORDER BY INVOICE_NO DESC";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameter to prevent SQL injection
                    cmd.Parameters.AddWithValue("@comid", comid);

                    object result = await cmd.ExecuteScalarAsync();
                    if (result != null)
                        lastInvoiceNo = result.ToString();
                }
            }

            return lastInvoiceNo;
        }

        public async Task<int> GetCountByDate(string currentDate, string comid)
        {
            string query = @"SELECT COUNT(*) 
                     FROM MST_PATIENT 
                     WHERE DATE = @CurrentDate 
                     AND COM_ID = @ComId";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
            {
                await con.OpenAsync();
                return await con.ExecuteScalarAsync<int>(query, new
                {
                    CurrentDate = currentDate,
                    ComId = comid
                });
            }
        }

        public async Task<int> GetCountApprovePending(string comid)
        {
            string query = @"SELECT COUNT(*) 
                     FROM MST_PATIENT 
                     WHERE STATUS_CODE = 0 
                     AND COM_ID = @ComId";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
            {
                await con.OpenAsync();
                return await con.ExecuteScalarAsync<int>(query, new
                {
                    ComId = comid
                });
            }
        }

    }
}
