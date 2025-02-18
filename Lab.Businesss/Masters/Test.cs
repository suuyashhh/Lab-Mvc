using Lab.DALDapper.Implimantation.Masters;
using Lab.DTO.Masters.Interfaces;
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
        public Int64 TestCode { get; set; }
        public string TestName { get; set; }
        public decimal Price { get; set; }
        public decimal LabPrice { get; set; }

        public static async Task<List<Test>> GetTestsAsync(string searchtext)
        {
            _dalTest = new DALTest();
            var dtoTests = await _dalTest.GetTestsAsync(searchtext);

            // Map DTOTest to Test
            return dtoTests.Select(t => new Test
            {
                TestCode = (int)t.TEST_CODE,
                TestName = t.TEST_NAME,
                Price = t.PRICE,
                LabPrice = t.LAB_PRICE
            }).ToList();
        }
    }
}
