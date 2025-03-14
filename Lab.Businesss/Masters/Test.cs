using Lab.DALDapper.Implimantation.Masters;
using Lab.DTO.Masters.Interfaces;
using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Businesss.Masters
{
    public class Test
    {
        public static IMSTTest _dalTest;
        public Int64 TrnNo { get; set; }
        public Int64 TestCode { get; set; }
        public string TestName { get; set; }
        public decimal Price { get; set; }
        public decimal LabPrice { get; set; }
        public int SrNo { get; set; }
        public int StatusCode { get; set; }
        public string ShortTrnNo { get; set; }
        public string DeleteReason { get; set; }


        public static Test New()
        {
            try
            {
                return new Test();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Failed. " + ex.Message);
            }
        }

        public static async Task<Test> GetExistingAsync(Int64 code)
        {
            try
            {
                _dalTest = new DALTest();

                DTOTest dtoTest = await Task.Run(() => { return _dalTest.GetExisting(code); });

                if (dtoTest != null)
                    return new Test()
                    {
                        TrnNo = dtoTest.TRN_NO,
                        TestCode = dtoTest.TEST_CODE,
                        TestName = dtoTest.TEST_NAME,
                        Price = dtoTest.PRICE,
                        LabPrice = dtoTest.LAB_PRICE,
                        SrNo = dtoTest.SR_NO

                    };
                else
                    return null;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }
        public static async Task<List<Test>> GetAllAsync()
        {
            try
            {
                _dalTest = new DALTest();
                List<Test> lstTests = await Task.Run(() => { return fillTestList(_dalTest.GetAll()); });
                return lstTests;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }

        private static List<Test> fillTestList(List<DTOTest> dtoTest)
        {

            var _Testlist = from dtotest in dtoTest
                            select new Test()
                            {
                                TrnNo = dtotest.TRN_NO,
                                TestCode= dtotest.TEST_CODE,
                                TestName = dtotest.TEST_NAME,
                                Price = dtotest.PRICE,
                                LabPrice = dtotest.LAB_PRICE,
                                SrNo = dtotest.SR_NO,
                                StatusCode = dtotest.STATUS_CODE,
                                ShortTrnNo = dtotest.TEST_CODE.ToString().Substring(2) + "-" + dtotest.TEST_CODE.ToString().Substring(dtotest.TEST_CODE.ToString().Length - 2)
        };

            return _Testlist.AsEnumerable<Test>().ToList();
        }
        public static async Task<List<Test>> GetTestsAsync(string searchtext)
        {
            _dalTest = new DALTest();
            var dtoTests = await _dalTest.GetTestsAsync(searchtext);

            
            return dtoTests.Select(t => new Test
            {
                TestCode = (Int64)t.TEST_CODE,
                TestName = t.TEST_NAME,
                Price = t.PRICE,
                LabPrice = t.LAB_PRICE
            }).ToList();
        }

        public static async Task<Int64> Create(Test _ObjTest)
        {
            try
            {
                Int64 result = 0;
                _dalTest = new DALTest();

                Int64 newTestId = await GeneratTestId();

                DTOTest _objDtoTest = new DTOTest()
                {
                    TEST_CODE = newTestId,
                    TEST_NAME = _ObjTest.TestName,
                    PRICE = _ObjTest.Price,
                    LAB_PRICE    = _ObjTest.LabPrice,
                    SR_NO = _ObjTest.SrNo
                };

                result = await Task.Run(() => { return _dalTest.Create(_objDtoTest); });

                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<Int64> Edit(Test _ObjTest)
        {
            try
            {
                Int64 result = 0;
                _dalTest = new DALTest();


                DTOTest _objDtoTest = new DTOTest()
                {
                    TEST_CODE = _ObjTest.TestCode,
                    TEST_NAME = _ObjTest.TestName,
                    PRICE = _ObjTest.Price,
                    LAB_PRICE = _ObjTest.LabPrice,
                    SR_NO = _ObjTest.SrNo
                };

                result = await Task.Run(() => { return _dalTest.Edit(_objDtoTest); });

                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<Int64> Delete(Test _ObjTest)
        {
            try
            {
                Int64 result = 0;
                _dalTest = new DALTest();


                DTOTest _objDtoTest = new DTOTest()
                {
                    TEST_CODE = _ObjTest.TestCode,
                    DELETE_REASON = _ObjTest.DeleteReason,
                };

                result = await Task.Run(() => { return _dalTest.Delete(_objDtoTest); });

                return result;
            }
            catch
            {
                return 0;
            }
        }
        private static async Task<long> GeneratTestId()
        {
            _dalTest = new DALTest();
            string fixedPart = "2";
            string fixedPartSec = "06";

            string lastId = await _dalTest.GetLastTestIdForFixedParts(fixedPart, fixedPartSec);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith(fixedPart + fixedPartSec))
            {
                int lastNumber = int.Parse(lastId.Substring(fixedPart.Length + fixedPartSec.Length));
                nextNumber = lastNumber + 1;
            }

            long newTestId = long.Parse(fixedPart + fixedPartSec + nextNumber.ToString("D3"));

            return newTestId;
        }

    }
}
