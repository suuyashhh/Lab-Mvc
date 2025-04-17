using Lab.DALDapper.Implimantation.Masters;
using Lab.DTO.Masters.Interfaces;
using Lab.DTO.Masters.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Businesss.Masters
{
    public class TestTable
    {
        public static IMstTestTable _dalTestTabe;

        public Int64 TestCode { get; set; }
        public String TestName { get; set; }
        public Int64 TrnNo { get; set; }
        public int SrNo { get; set; }
        public decimal Price { get; set; }
        public decimal LabPrice { get; set; }
        public string ComId { get; set; }

        public static List<TestTable> GetITableList(Int64 Trnno)
        {
            try
            {
                _dalTestTabe = new DALTestTable();
                return fillList(_dalTestTabe.GetITableList(Trnno));
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }

        private static List<TestTable> fillList(List<DTOTestTable> lstdtoMstEmployeeDepartment)
        {
            List<TestTable> _EmpDeptlist = new List<TestTable>();
            DALTestTable _dalMstMaterials = new DALTestTable(); 

            foreach (DTOTestTable dtoMstEmpDept in lstdtoMstEmployeeDepartment)
            {
                string strMatName = ""; 
                DTOTestTable objDtoMstMaterials = _dalMstMaterials.GetExisting(dtoMstEmpDept.TEST_CODE);

                if (objDtoMstMaterials != null)
                {
                    strMatName = objDtoMstMaterials.TEST_NAME; 
                }

                TestTable _objEmpDept = new TestTable()
                {
                    TrnNo = dtoMstEmpDept.TRN_NO,
                    SrNo = dtoMstEmpDept.SR_NO,
                    TestCode = dtoMstEmpDept.TEST_CODE,
                    Price = dtoMstEmpDept.PRICE,
                    LabPrice = dtoMstEmpDept.LAB_PRICE,
                    TestName = strMatName 
                };

                _EmpDeptlist.Add(_objEmpDept);
            }
            return _EmpDeptlist;
        }

    }
}
