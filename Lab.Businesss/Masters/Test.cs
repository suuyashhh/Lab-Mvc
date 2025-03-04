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
                                StatusCode = dtotest.STATUS_CODE
                            };

            return _Testlist.AsEnumerable<Test>().ToList();
        }
        public static async Task<List<Test>> GetTestsAsync(string searchtext)
        {
            _dalTest = new DALTest();
            var dtoTests = await _dalTest.GetTestsAsync(searchtext);

            
            return dtoTests.Select(t => new Test
            {
                TestCode = (int)t.TEST_CODE,
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



                string datePart = DateTime.Now.ToString("yyyyMMdd");
                Int64 newTestId = await GenerateTestId(datePart);

                DTOTest _objDtoTest = new DTOTest()
                {
                    TRN_NO = newTestId,
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

        private static async Task<long> GenerateTestId(string datePart)
        {
            _dalTest = new DALTest();

            string fixedPart = "03";

            string lastId = await _dalTest.GetLastTestIdForDate(datePart);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith(datePart + fixedPart))
            {
                int lastNumber = int.Parse(lastId.Substring(10));
                nextNumber = lastNumber + 1;
            }

            long newTestId = long.Parse(datePart + fixedPart + nextNumber.ToString("D3"));

            return newTestId;
        }

    }
}
