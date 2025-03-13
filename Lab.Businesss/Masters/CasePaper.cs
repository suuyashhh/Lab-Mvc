using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab.DTO.Masters.Objects;
using Lab.DTO.Masters.Interfaces;
using System.Threading.Tasks;
using Lab.DALDapper.Implimantation.Masters;

namespace Lab.Businesss.Masters
{
    public class CasePaper
    {
        public static IMstCasePaper _dalCasePaper;
        public static IMSTTest _dalTest;
        public static IMstTestTable _dalTestTable;
        public Int64 TrnNo { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string ConNumber { get; set; }
        public string Address { get; set; }
        public string DoctorRef { get; set; }
        public string Date { get; set; }
        public int StatusCode { get; set; }
        public IList<TestTable> MatIs { get; set; }
        public string ShortTrnNo { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal Discount { get; set; }
        public string DeleteReason { get; set; }

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
                        DoctorRef = dtoCasePaper.DOCTOR_REF,
                        StatusCode = dtoCasePaper.STATUS_CODE,
                        Discount= dtoCasePaper.DISCOUNT,
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
        public static async Task<List<CasePaper>> GetAllAsync()
        {
            try
            {
                _dalCasePaper = new DALCasePaper();
                List<CasePaper> lstCity = await Task.Run(() => { return fillCityList(_dalCasePaper.GetAll()); });
                return lstCity;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }

        private static List<CasePaper> fillCityList(List<DTOCasePaper> dtoCasePaper)
        {

            var _citylist = from dtocasepaper in dtoCasePaper
                            select new CasePaper()
                            {
                                TrnNo = dtocasepaper.TRN_NO,
                                PatientName = dtocasepaper.PATIENT_NAME,
                                Gender = dtocasepaper.GENDER,
                                ConNumber = dtocasepaper.CON_NUMBER,
                                Address = dtocasepaper.ADDRESS,
                                DoctorRef = dtocasepaper.DOCTOR_REF,
                                Date = dtocasepaper.DATE,
                                StatusCode = dtocasepaper.STATUS_CODE,
                                Discount = dtocasepaper.DISCOUNT,
                                ShortTrnNo = dtocasepaper.TRN_NO.ToString().Substring(2, 6) + "-" + dtocasepaper.TRN_NO.ToString().Substring(dtocasepaper.TRN_NO.ToString().Length - 2),
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


                string strTranDate = DateUtility.GetFormatedDate(_ObjCsPaper.Date, 1);
                string datePart = DateTime.Now.ToString("yyyyMMdd");
                Int64 newPatientId = await GeneratePatientId(datePart);

                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    TRN_NO = newPatientId, 
                    PATIENT_NAME = _ObjCsPaper.PatientName,
                    DATE = strTranDate,
                    GENDER = _ObjCsPaper.Gender,
                    CON_NUMBER = _ObjCsPaper.ConNumber,
                    DOCTOR_REF = _ObjCsPaper.DoctorRef,
                    DISCOUNT = _ObjCsPaper.Discount,
                    TOTAL_PROFIT = _ObjCsPaper.TotalProfit,
                    TOTAL_AMOUNT = _ObjCsPaper.TotalAmount,

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


                DTOCasePaper _objDtoCasePaper = new DTOCasePaper()
                {
                    TRN_NO = _ObjCsPaper.TrnNo,
                    PATIENT_NAME = _ObjCsPaper.PatientName,
                    GENDER = _ObjCsPaper.Gender,
                    CON_NUMBER = _ObjCsPaper.ConNumber,
                    DOCTOR_REF = _ObjCsPaper.DoctorRef,
                    DISCOUNT = _ObjCsPaper.Discount,
                    TOTAL_PROFIT = _ObjCsPaper.TotalProfit,
                    TOTAL_AMOUNT = _ObjCsPaper.TotalAmount,

                };

                result = await Task.Run(() => { return _dalCasePaper.Edit(_objDtoCasePaper); });

                _dalTestTable = new DALTestTable();
                _dalTestTable.DelPermenantData(_ObjCsPaper.TrnNo);

                //IList<TestTable> counte = _ObjCsPaper.MatIs;
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

                return result;
            }
            catch
            {
                throw new Exception("Failed To Update");
            }
        }

        private static async Task<long> GeneratePatientId(string datePart)
        {
            _dalCasePaper = new DALCasePaper();
                        
            string fixedPart = "02";
                       
            string lastId = await _dalCasePaper.GetLastPatientIdForDate(datePart);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith(datePart + fixedPart))
            {
                int lastNumber = int.Parse(lastId.Substring(10)); 
                nextNumber = lastNumber + 1;
            }
                        
            long newPatientId = long.Parse(datePart + fixedPart + nextNumber.ToString("D3"));

            return newPatientId;
        }

    }
}
