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
    public class Doctor
    {
        public static IMstDoctor _dalDoctor;
        public Int64 DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public string DoctorAddress { get; set; }
        public string DoctorNumber { get; set; }
        public string ShortTrnNo { get; set; }

        public static Doctor New()
        {
            try
            {
                return new Doctor();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Failed. " + ex.Message);
            }
        }


        public static List<Doctor> GetDoctorList()
        {
            _dalDoctor = new DALDoctor();
            return FillList(_dalDoctor.GetDoctorList());
        }
        private static List<Doctor> FillList(List<DTODoctor> lstdtoMstDoctor)
        {
            List<Doctor> _Doctorlist = new List<Doctor>();
            if (lstdtoMstDoctor.Count() > 0)
            {
                _Doctorlist = (from dtoMstDoctor in lstdtoMstDoctor
                                 select new Doctor()
                                 {
                                     DoctorCode= dtoMstDoctor.DOCTOR_CODE,
                                     DoctorName = dtoMstDoctor.DOCTOR_NAME,
                                     //EmployeeId = dtoMstEmployee.EMPLOYEE_ID,
                                     //EmployeeShortName = dtoMstEmployee.EMPLOYEE_SHORT_NAME,
                                     //EmployeeFirstName = dtoMstEmployee.EMPLOYEE_FIRST_NAME,
                                     //EmployeeMiddleName = dtoMstEmployee.EMPLOYEE_MIDDLE_NAME,
                                     //EmployeeLastName = dtoMstEmployee.EMPLOYEE_LAST_NAME,
                                     //StatusCode = dtoMstEmployee.STATUS_CODE,
                                     //EmployeeName = dtoMstEmployee.EMPLOYEE_FIRST_NAME + " " + dtoMstEmployee.EMPLOYEE_MIDDLE_NAME + " " + dtoMstEmployee.EMPLOYEE_LAST_NAME,
                                     //PayrollEmpId = dtoMstEmployee.PAYROLL_EMP_ID,
                                     //PayrollCompany = dtoMstEmployee.PAYROLL_COMPANY,
                                     //PayrollDept = dtoMstEmployee.PAYROLL_DEPT,
                                     //Location = dtoMstEmployee.LOCATION,
                                     //IsLeft = dtoMstEmployee.IS_LEFT,
                                     //DesignationCode = dtoMstEmployee.DESIGNATION_CODE,
                                     //IsVendor = dtoMstEmployee.IS_VENDOR,
                                     //CrtBy = dtoMstEmployee.CRT_BY,
                                     //EmployeeLoginId = dtoMstEmployee.EMPLOYEE_LOGIN_ID,
                                     //Email = dtoMstEmployee.EMAIL
                                 }).ToList();
            }
            return _Doctorlist;
        }
        public static async Task<List<Doctor>> GetAllAsync()
        {
            try
            {
                _dalDoctor = new DALDoctor();
                List<Doctor> lstDoctors = await Task.Run(() => { return fillDoctorsList(_dalDoctor.GetAll()); });
                return lstDoctors;
            }
            catch
            {
                throw new Exception("Request Failed");
            }
        }
        
        private static List<Doctor> fillDoctorsList(List<DTODoctor> dtoDoctor)
        {

            var _Doctorslist = from dtodoctor in dtoDoctor
                            select new Doctor()
                            {
                                DoctorCode = dtodoctor.DOCTOR_CODE,
                                DoctorName = dtodoctor.DOCTOR_NAME,                           
                                //ShortTrnNo = dtodoctor.DOCTOR_CODE.ToString().Substring(2, 6) + "-" + dtodoctor.DOCTOR_CODE.ToString().Substring(dtodoctor.DOCTOR_CODE.ToString().Length - 2),
                            };

            return _Doctorslist.AsEnumerable<Doctor>().ToList();
        }
        public static async Task<Int64> Create(Doctor _ObjDoctor)
        {
            try
            {
                Int64 result = 0;
                _dalDoctor = new DALDoctor();

                //string datePart = DateTime.Now.ToString("yyyyMMdd");
                Int64 newTestId = await GenerateDoctorId();

                DTODoctor _objDtoDoctor = new DTODoctor()
                {
                    DOCTOR_CODE = newTestId,
                    DOCTOR_NAME = _ObjDoctor.DoctorName,
                    DOCTOR_ADDRESS = _ObjDoctor.DoctorAddress,
                    DOCTOR_NUMBER = _ObjDoctor.DoctorNumber,
                   
                };

                result = await Task.Run(() => { return _dalDoctor.Create(_objDtoDoctor); });

                return result;
            }
            catch
            {
                return 0;
            }
        }

        private static async Task<long> GenerateDoctorId()
        {
            _dalDoctor = new DALDoctor();
            string fixedPart = "3";
            string fixedPartSec = "06";

            string lastId = await _dalDoctor.GetLastDoctorIdForFixedParts(fixedPart, fixedPartSec);

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
