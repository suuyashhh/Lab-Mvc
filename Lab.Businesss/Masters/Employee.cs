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
    public class Employee
    {
        public static IMstEmployee _dalEmployee;
        public Int64 EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpContact { get; set; }
        public string EmpPassword { get; set; }
        public string ShortTrnNo { get; set; }
        public string DeleteReason { get; set; }
        public string ComId { get; set; }

        public static Employee New()
        {
            try
            {
                return new Employee();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Failed. " + ex.Message);
            }
        }

        public static async Task<Employee> GetExistingAsync(Int64 code)
        {
            try
            {
                _dalEmployee = new DALEmployee();

                DTOEmployee dtoEmployee = await Task.Run(() => { return _dalEmployee.GetExisting(code); });

                if (dtoEmployee != null)
                    return new Employee()
                    {
                        //TrnNo = dtoDoctor.TRN_NO,
                        EmpId = dtoEmployee.EMP_ID,
                        EmpName = dtoEmployee.EMP_NAME,
                        EmpContact = dtoEmployee.EMP_CONTACT,
                        EmpPassword = dtoEmployee.EMP_PASSWORD,
                        //SrNo = dtoDoctor.SR_NO

                    };
                else
                    return null;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }


        public static List<Employee> GetEmployeeList(string comid)
        {
            _dalEmployee = new DALEmployee();
            return FillList(_dalEmployee.GetEmployeeList(comid));
        }
        private static List<Employee> FillList(List<DTOEmployee> lstdtoMstEmployee)
        {
            List<Employee> _Employeelist = new List<Employee>();
            if (lstdtoMstEmployee.Count() > 0)
            {
                _Employeelist = (from dtoMstEmployee in lstdtoMstEmployee
                               select new Employee()
                               {
                                   EmpId = dtoMstEmployee.EMP_ID,
                                   EmpName = dtoMstEmployee.EMP_NAME,
                                   EmpContact=dtoMstEmployee.EMP_CONTACT,
                                   EmpPassword=dtoMstEmployee.EMP_PASSWORD,
                               }).ToList();
            }
            return _Employeelist;
        }
        public static async Task<List<Employee>> GetAllAsync(string comid)
        {
            try
            {
                _dalEmployee = new DALEmployee();
                List<Employee> lstEmployees = await Task.Run(() => { return fillEmployeesList(_dalEmployee.GetAll(comid)); });
                return lstEmployees;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }

        private static List<Employee> fillEmployeesList(List<DTOEmployee> dtoEmployee)
        {

            var _Employeeslist = from dtoemployee in dtoEmployee
                                 select new Employee()
                               {
                                   EmpId = dtoemployee.EMP_ID,
                                   EmpName = dtoemployee.EMP_NAME,
                                   EmpContact=dtoemployee.EMP_CONTACT,
                                   EmpPassword=dtoemployee.EMP_PASSWORD,
                                   //ShortTrnNo = dtodoctor.DOCTOR_CODE.ToString().Substring(2, 6) + "-" + dtodoctor.DOCTOR_CODE.ToString().Substring(dtodoctor.DOCTOR_CODE.ToString().Length - 2),
                               };

            return _Employeeslist.AsEnumerable<Employee>().ToList();
        }
        public static async Task<Int64> Create(Employee _ObjEmployee)
        {
            try
            {
                Int64 result = 0;
                _dalEmployee = new DALEmployee();

                string ComId = _ObjEmployee.ComId;
                Int64 newTestId = await GenerateEmployeeId(ComId);

                DTOEmployee _objDtoEmployee = new DTOEmployee()
                {
                    EMP_ID = newTestId,
                    EMP_NAME = _ObjEmployee.EmpName,
                    EMP_CONTACT = _ObjEmployee.EmpContact,
                    EMP_PASSWORD = _ObjEmployee.EmpPassword,
                    COM_ID = _ObjEmployee.ComId,

                };

                result = await Task.Run(() => { return _dalEmployee.Create(_objDtoEmployee); });

                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<Int64> Edit(Employee _ObjEmployee)
        {
            try
            {
                Int64 result = 0;
                _dalEmployee = new DALEmployee();


                DTOEmployee _objDtoEmployee = new DTOEmployee()
                {
                    EMP_ID = _ObjEmployee.EmpId,
                    EMP_NAME = _ObjEmployee.EmpName,
                    EMP_CONTACT = _ObjEmployee.EmpContact,
                    EMP_PASSWORD= _ObjEmployee.EmpPassword,
                    //SR_NO = _ObjDoctor.SrNo
                };

                result = await Task.Run(() => { return _dalEmployee.Edit(_objDtoEmployee); });

                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<Int64> Delete(Employee _ObjEmployee)
        {
            try
            {
                Int64 result = 0;
                _dalEmployee = new DALEmployee();


                DTOEmployee _objDtoEmployee = new DTOEmployee()
                {
                    EMP_ID = _ObjEmployee.EmpId,
                    DELETE_REASON = _ObjEmployee.DeleteReason,
                };

                result = await Task.Run(() => { return _dalEmployee.Delete(_objDtoEmployee); });

                return result;
            }
            catch
            {
                return 0;
            }
        }

        private static async Task<long> GenerateEmployeeId(string ComId)
        {
           _dalEmployee = new DALEmployee();
           string fixedPart = "4";
           string fixedPartSec = ComId;

           string lastId = await _dalEmployee.GetLastEmployeeIdForFixedParts(fixedPart, fixedPartSec);

           int nextNumber = 1;
           if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith(fixedPart + fixedPartSec))
           {
               int lastNumber = int.Parse(lastId.Substring(fixedPart.Length + fixedPartSec.Length));
               nextNumber = lastNumber + 1;
           }

            long newTestId = long.Parse(fixedPart + fixedPartSec + nextNumber);

            return newTestId;
        }

    }
}
