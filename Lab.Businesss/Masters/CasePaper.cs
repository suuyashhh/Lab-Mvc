using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab.DTO.Masters.Objects;
using Lab.DTO.Masters.Interfaces;
using System.Threading.Tasks;
using Lab.DALDapper.Implimantation.Masters;
using System.Transactions;
using System.Security.Cryptography;
using System.Globalization;

namespace Lab.Businesss.Masters
{
    public class CasePaper
    {
        public static IMstCasePaper _dalCasePaper;
        public static IMSTTest _dalTest;
        public static IMstTestTable _dalTestTable;
        public static IMstDoctor _dalDoctor;
        public Int64 TrnNo { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string ConNumber { get; set; }
        public string Address { get; set; }
        public int DoctorCode { get; set; }
        public string Date { get; set; }
        public int StatusCode { get; set; }
        public IList<TestTable> MatIs { get; set; }
        public string ShortTrnNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal Discount { get; set; }
        public string DeleteReason { get; set; }
        public string InvoiceNo { get; set; }
        public decimal PaymentAmount { get; set; }
        public int CollectionType { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string CrtBy { get; set; }
        public string ComId { get; set; }
        public string DoctorName { get; set; }
        public static CasePaper New()
        {
            try
            {
                return new CasePaper();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Failed. " + ex.Message);
            }
        }

       
        public static async Task<List<CasePaper>> GetDateWiseAll(string strStartDate, string strEndDate, string ComId)
        {
            try
            {
                _dalCasePaper = new DALCasePaper();
                List<CasePaper> lstTD = new List<CasePaper>();

                List<DTOCasePaper> objLstCasePaper = await _dalCasePaper.GetDateWiseAllAsync(strStartDate, strEndDate, ComId);
              

                if (objLstCasePaper != null)
                {
                    lstTD = (from cp in objLstCasePaper                       
                             select new CasePaper
                             {
                                 TrnNo = cp.TRN_NO,
                                 PatientName = cp.PATIENT_NAME,
                                 Gender = cp.GENDER,
                                 ConNumber = cp.CON_NUMBER,
                                 Address = cp.ADDRESS,
                                 DoctorCode = cp.DOCTOR_CODE,
                                 Date = DateUtility.GetFormatedDate(cp.DATE, 0),
                                 StatusCode = cp.STATUS_CODE,
                                 Discount = cp.DISCOUNT,
                                 InvoiceNo = cp.INVOICE_NO,
                                 ShortTrnNo = cp.TRN_NO.ToString().Substring(0, 6) + "-" +
                                              cp.TRN_NO.ToString().Substring(cp.TRN_NO.ToString().Length - 2),
                                 CrtBy = cp.CRT_BY,
                                 PaymentStatus = cp.PAYMENT_STATUS,
                              
                             }).ToList();
                }

                return lstTD;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }


        public static async Task<CasePaper> GetExistingAsync(Int64 code)
        {
            try
            {
                _dalCasePaper = new DALCasePaper();

                DTOCasePaper dtoCasePaper = await Task.Run(() => { return _dalCasePaper.GetExisting(code); });

                if (dtoCasePaper != null)
                    return new CasePaper()
                    {
                        TrnNo = dtoCasePaper.TRN_NO,
                        Date = DateUtility.GetFormatedDate(dtoCasePaper.DATE, 0),
                        PatientName = dtoCasePaper.PATIENT_NAME,
                        Gender = dtoCasePaper.GENDER,
                        ConNumber = dtoCasePaper.CON_NUMBER,
                        Address = dtoCasePaper.ADDRESS,
                        DoctorCode = dtoCasePaper.DOCTOR_CODE,
                        StatusCode = dtoCasePaper.STATUS_CODE,
                        Discount = dtoCasePaper.DISCOUNT,
                        PaymentMethod = dtoCasePaper.PAYMENT_METHOD,
                        PaymentAmount = dtoCasePaper.PAYMENT_AMOUNT,
                        CollectionType = dtoCasePaper.COLLECTION_TYPE,
                        MatIs = TestTable.GetITableList(dtoCasePaper.TRN_NO)

                    };
                else
                    return null;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }
        
        public static async Task<CasePaper> GetExistingAsyncInvoice(Int64 code, string comid)
        {
            try
            {
                _dalCasePaper = new DALCasePaper();

                DTOCasePaper dtoCasePaper = await Task.Run(() => { return _dalCasePaper.GetExisting(code); });
                string invoiceNo = dtoCasePaper.INVOICE_NO;
                List<Doctor> ObjDoctor = Doctor.GetDoctorList(comid);
                string doctorName = ObjDoctor
                .FirstOrDefault(d => d.DoctorCode == dtoCasePaper.DOCTOR_CODE)?
                .DoctorName ?? string.Empty;
                if (invoiceNo == null || invoiceNo == "")
                {
                    invoiceNo = await GenerateInvoiceNoAsync(comid);
                }
                if (dtoCasePaper != null)
                    return new CasePaper()
                    {
                        TrnNo = dtoCasePaper.TRN_NO,
                        Date = DateUtility.GetFormatedDate(dtoCasePaper.DATE, 0),
                        PatientName = dtoCasePaper.PATIENT_NAME,
                        Gender = dtoCasePaper.GENDER,
                        ConNumber = dtoCasePaper.CON_NUMBER,
                        Address = dtoCasePaper.ADDRESS,
                        DoctorCode = dtoCasePaper.DOCTOR_CODE,
                        StatusCode = dtoCasePaper.STATUS_CODE,
                        Discount = dtoCasePaper.DISCOUNT,
                        InvoiceNo = invoiceNo,
                        MatIs = TestTable.GetITableList(dtoCasePaper.TRN_NO),
                        DoctorName = doctorName,

                    };
                else
                    return null;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }
        public static async Task<List<CasePaper>> GetAllAsync()
        {
            try
            {
                _dalCasePaper = new DALCasePaper();
                List<CasePaper> lstCity = await Task.Run(() => { return fillCasePaperList(_dalCasePaper.GetAll()); });
                return lstCity;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }

        private static List<CasePaper> fillCasePaperList(List<DTOCasePaper> dtoCasePaper)
        {

            var _citylist = from dtocasepaper in dtoCasePaper
                            select new CasePaper()
                            {
                                TrnNo = dtocasepaper.TRN_NO,
                                PatientName = dtocasepaper.PATIENT_NAME,
                                Gender = dtocasepaper.GENDER,
                                ConNumber = dtocasepaper.CON_NUMBER,
                                Address = dtocasepaper.ADDRESS,
                                DoctorCode = dtocasepaper.DOCTOR_CODE,
                                Date = DateUtility.GetFormatedDate(dtocasepaper.DATE, 0),
                                StatusCode = dtocasepaper.STATUS_CODE,
                                Discount = dtocasepaper.DISCOUNT,
                                ShortTrnNo = dtocasepaper.TRN_NO.ToString().Substring(2, 6) + "-" + dtocasepaper.TRN_NO.ToString().Substring(dtocasepaper.TRN_NO.ToString().Length - 2),
                                CrtBy = dtocasepaper.CRT_BY,
                                PaymentStatus = dtocasepaper.PAYMENT_STATUS,
                            };

            return _citylist.AsEnumerable<CasePaper>().ToList();
        }

        public static async Task<Int64> Create(CasePaper _ObjCsPaper)
        {
            try
            {
                Int64 result = 0;
                _dalCasePaper = new DALCasePaper();
                _dalTest = new DALTest();
                _dalTestTable = new DALTestTable();

                int intStatusCode = 0;
                _ObjCsPaper.StatusCode = intStatusCode;
                string strTranDate = DateUtility.GetFormatedDate(_ObjCsPaper.Date, 1);
                string Comid = _ObjCsPaper.ComId;
                string datePart = DateUtility.GetCurrDateForGenId();
             
                Int64 newPatientId = await GeneratePatientId(datePart, Comid);

                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    TRN_NO = newPatientId, 
                    PATIENT_NAME = _ObjCsPaper.PatientName,
                    DATE = strTranDate,
                    GENDER = _ObjCsPaper.Gender,
                    CON_NUMBER = _ObjCsPaper.ConNumber,
                    DOCTOR_CODE = _ObjCsPaper.DoctorCode,
                    DISCOUNT = _ObjCsPaper.Discount,
                    TOTAL_PROFIT = _ObjCsPaper.TotalProfit,
                    TOTAL_AMOUNT = _ObjCsPaper.TotalAmount,
                    STATUS_CODE= _ObjCsPaper.StatusCode,
                    ADDRESS = _ObjCsPaper.Address,
                    PAYMENT_AMOUNT = _ObjCsPaper.PaymentAmount,
                    PAYMENT_METHOD = _ObjCsPaper.PaymentMethod,
                    COLLECTION_TYPE = _ObjCsPaper.CollectionType,
                    CRT_BY = _ObjCsPaper.CrtBy,
                    PAYMENT_STATUS = _ObjCsPaper.PaymentStatus,
                    COM_ID = _ObjCsPaper.ComId,

                };

                result = await Task.Run(() => { return _dalCasePaper.Create(_objDtoCasePaper); });


                IList<TestTable> counte = _ObjCsPaper.MatIs;
                if (counte != null)
                {
                    int intSrNo = 1;

                    foreach (TestTable _objTestTable in _ObjCsPaper.MatIs)
                    {
                        DTOTestTable _objTestTableDetails = new DTOTestTable()
                        {
                            TRN_NO = newPatientId,
                            TEST_CODE = _objTestTable.TestCode,
                            SR_NO = intSrNo,
                            PRICE = _objTestTable.Price,
                            LAB_PRICE = _objTestTable.LabPrice,
                            COM_ID = _ObjCsPaper.ComId

                        };
                        intSrNo++;
                        _dalTestTable.Create(_objTestTableDetails);
                       
                    }
                }


                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<Int64> Edit(CasePaper _ObjCsPaper)
        {
            try
            {
                Int64 result = 0;
                _dalCasePaper = new DALCasePaper();
                _dalTest = new DALTest();
                _dalTestTable = new DALTestTable();

                int intStatusCode = 0;
                _ObjCsPaper.StatusCode = intStatusCode;
                string strTranDate = DateUtility.GetFormatedDate(_ObjCsPaper.Date, 1);

                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    TRN_NO = _ObjCsPaper.TrnNo,
                    PATIENT_NAME = _ObjCsPaper.PatientName,
                    GENDER = _ObjCsPaper.Gender,
                    CON_NUMBER = _ObjCsPaper.ConNumber,
                    DOCTOR_CODE = _ObjCsPaper.DoctorCode,
                    DISCOUNT = _ObjCsPaper.Discount,
                    TOTAL_PROFIT = _ObjCsPaper.TotalProfit,
                    TOTAL_AMOUNT = _ObjCsPaper.TotalAmount,
                    STATUS_CODE = _ObjCsPaper.StatusCode,                   
                    DATE = strTranDate,                   
                    ADDRESS = _ObjCsPaper.Address,
                    PAYMENT_AMOUNT = _ObjCsPaper.PaymentAmount,
                    PAYMENT_METHOD = _ObjCsPaper.PaymentMethod,
                    COLLECTION_TYPE = _ObjCsPaper.CollectionType,
                    PAYMENT_STATUS = _ObjCsPaper.PaymentStatus,

                };

                result = await Task.Run(() => { return _dalCasePaper.Edit(_objDtoCasePaper); });

                _dalTestTable = new DALTestTable();
                _dalTestTable.DelPermenantData(_ObjCsPaper.TrnNo);

                if (_ObjCsPaper.MatIs != null)
                {
                    int intSrNo = 1;

                    foreach (TestTable _objTestTable in _ObjCsPaper.MatIs)
                    {
                        DTOTestTable _objTestTableDetails = new DTOTestTable()
                        {
                            TRN_NO = _ObjCsPaper.TrnNo,
                            TEST_CODE = _objTestTable.TestCode,
                            SR_NO = intSrNo,
                            PRICE = _objTestTable.Price,
                            LAB_PRICE = _objTestTable.LabPrice,
                            COM_ID = _ObjCsPaper.ComId,
                        };
                        intSrNo++;
                        _dalTestTable.Create(_objTestTableDetails);

                    }
                }
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<Int64> Delete(CasePaper _objDtoCasePaper)
        {
            try
            {
                int result = 0;
                DTOCasePaper _objDtoMstCasePaper = new DTOCasePaper()
                {
                    TRN_NO = _objDtoCasePaper.TrnNo,
                    DELETE_REASON = _objDtoCasePaper.DeleteReason
                };
                result = (int)await Task.Run(() => { return _dalCasePaper.Delete(_objDtoMstCasePaper); });

                _dalTestTable = new DALTestTable();
                _dalTestTable.DelPermenantData(_objDtoCasePaper.TrnNo);

                return _objDtoCasePaper.TrnNo;
            }
            catch
            {
                throw new Exception("Failed To Update");
            }
        }

        public static async Task<Int64> InvoiceSave(CasePaper _ObjCsPaper)
        {
            try
            {
                Int64 result = 0;
                _dalCasePaper = new DALCasePaper();
                                             

                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    TRN_NO = _ObjCsPaper.TrnNo,
                    INVOICE_NO = _ObjCsPaper.InvoiceNo,
                    
                };

                result = await Task.Run(() => { return _dalCasePaper.InvoiceSave(_objDtoCasePaper); });
                              
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<Int64> Approve(CasePaper _ObjCsPaper)
        {
            try
            {
                Int64 result = 0;
                _dalCasePaper = new DALCasePaper();
                _dalTest = new DALTest();
                _dalTestTable = new DALTestTable();

                int intStatusCode = 101;
                _ObjCsPaper.StatusCode = intStatusCode;
                string strTranDate = DateUtility.GetFormatedDate(_ObjCsPaper.Date, 1);

                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    TRN_NO = _ObjCsPaper.TrnNo,
                    PATIENT_NAME = _ObjCsPaper.PatientName,
                    GENDER = _ObjCsPaper.Gender,
                    CON_NUMBER = _ObjCsPaper.ConNumber,
                    DOCTOR_CODE = _ObjCsPaper.DoctorCode,
                    DISCOUNT = _ObjCsPaper.Discount,
                    TOTAL_PROFIT = _ObjCsPaper.TotalProfit,
                    TOTAL_AMOUNT = _ObjCsPaper.TotalAmount,
                    STATUS_CODE = _ObjCsPaper.StatusCode,
                    DATE = strTranDate,
                    ADDRESS = _ObjCsPaper.Address,
                    PAYMENT_AMOUNT = _ObjCsPaper.PaymentAmount,
                    PAYMENT_METHOD = _ObjCsPaper.PaymentMethod,
                    COLLECTION_TYPE = _ObjCsPaper.CollectionType,
                    PAYMENT_STATUS = _ObjCsPaper.PaymentStatus,

                };

                result = await Task.Run(() => { return _dalCasePaper.Approve(_objDtoCasePaper); });

                _dalTestTable = new DALTestTable();
                _dalTestTable.DelPermenantData(_ObjCsPaper.TrnNo);

                if (_ObjCsPaper.MatIs != null)
                {
                    int intSrNo = 1;

                    foreach (TestTable _objTestTable in _ObjCsPaper.MatIs)
                    {
                        DTOTestTable _objTestTableDetails = new DTOTestTable()
                        {
                            TRN_NO = _ObjCsPaper.TrnNo,
                            TEST_CODE = _objTestTable.TestCode,
                            SR_NO = intSrNo,
                            PRICE = _objTestTable.Price,
                            LAB_PRICE = _objTestTable.LabPrice,
                            COM_ID = _ObjCsPaper.ComId,
                        };
                        intSrNo++;
                        _dalTestTable.Create(_objTestTableDetails);

                    }
                }
                return result;
            }
            catch
            {
                return 0;
            }
        }
        public static async Task<int> Approve(List<Int64> TrnNos)
        {
            try
            {
                int result = 0;
                string strTrnList = string.Join(",", TrnNos.Select(n => n.ToString()).ToArray());
                return result = await Task.Run(() =>
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            _dalCasePaper = new DALCasePaper();

                            result = _dalCasePaper.Approve(strTrnList);
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            throw ex;
                        }
                    }
                    return result;
                });
            }
            catch
            {
                throw new Exception("Failed To Approve");
            }
        }
        public static async Task<List<CasePaper>> GetApprovalPendingListAsync(string comid)
        {
            try
            {
                _dalCasePaper = new DALCasePaper();
                List<CasePaper> lstCity = await Task.Run(() => { return fillCasePaperList(_dalCasePaper.GetApprovalPendingList(comid)); });
                return lstCity;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }
        private static async Task<long> GeneratePatientId(string datePart,string Comid)
        {
            _dalCasePaper = new DALCasePaper();
                        
            string fixedPart = Comid;

            string dateComboKey = datePart + Comid;

            string lastId = await _dalCasePaper.GetLastPatientIdForDate(dateComboKey);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith(dateComboKey))
            {
                int lastNumber = int.Parse(lastId.Substring(9)); 
                nextNumber = lastNumber + 1;
            }
                        
            long newPatientId = long.Parse(dateComboKey + nextNumber);

            return newPatientId;
        }

        private static async Task<string> GenerateInvoiceNoAsync(string comid)
        {
            _dalCasePaper = new DALCasePaper();

            string lastInvoice = await _dalCasePaper.GetLastInvoiceNoAsync(comid);

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastInvoice) && lastInvoice.StartsWith("INV"))
            {
                string numberPart = lastInvoice.Substring(3);
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            string newInvoice = "INV" + nextNumber.ToString("D3"); 
            return newInvoice;
        }

        public async Task<int> GetCountCurrentDate(string comid)
        {
            _dalCasePaper = new DALCasePaper();
            var strTranDate = DateUtility.GetCurrentDate();

            string currentDate = DateUtility.GetFormatedDate(strTranDate, 1);
            return await _dalCasePaper.GetCountByDate(currentDate, comid);
        }

        public async Task<int> GetCountApprovePending(string comid)
        {
            _dalCasePaper = new DALCasePaper();
            var strTranDate = DateUtility.GetCurrentDate();

            string currentDate = DateUtility.GetFormatedDate(strTranDate, 1);
            return await _dalCasePaper.GetCountApprovePending(comid);
        }

    }
}
