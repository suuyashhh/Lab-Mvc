using Lab.Businesss.Masters;
using Lab_Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lab_Mvc.Controllers
{
    [CustomAuthorize]
    public class EmployeeController : Controller
    {
        public async Task<ActionResult> Index(string sortdir, string sortOrder, string searchString, int? page)
        {
            var comid = Session["ComId"].ToString();
            List<Employee> _lstEmployees = await Employee.GetAllAsync(comid);

            string strSortDir = "";
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                _lstEmployees = _lstEmployees.Where(obj =>
                    (obj.EmpName != null && obj.EmpName.ToUpper().Contains(searchString.ToUpper()))
                //(obj.CreatedBy != null && obj.CreatedBy.ToUpper().Contains(searchString.ToUpper())) ||
                //(obj.AppBy != null && obj.AppBy.ToUpper().Contains(searchString.ToUpper()))
                ).ToList();
            }

            if (!string.IsNullOrEmpty(sortdir))
            {
                strSortDir = sortdir;
                if (sortdir == "desc")
                {
                    ViewBag.SortDir = "asc";
                }
                else
                {
                    ViewBag.SortDir = "desc";
                }
            }

            switch (sortOrder)
            {
                case "DoctorCode":
                    if (strSortDir == "desc")
                    {
                        _lstEmployees = _lstEmployees.OrderByDescending(obj => obj.EmpId).ToList();
                    }
                    else
                    {
                        _lstEmployees = _lstEmployees.OrderBy(obj => obj.EmpId).ToList();
                    }
                    break;

                case "DoctorName":
                    if (strSortDir == "desc")
                    {
                        _lstEmployees = _lstEmployees.OrderByDescending(obj => obj.EmpName).ToList();
                    }
                    else
                    {
                        _lstEmployees = _lstEmployees.OrderBy(obj => obj.EmpName).ToList();
                    }
                    break;
                default:
                    _lstEmployees = _lstEmployees.OrderByDescending(p => p.EmpId).ToList();
                    ViewBag.SortDir = "asc";
                    break;
            }

            return View(_lstEmployees);
        }


        public ActionResult Create()
        {
            return PartialView(Doctor.New());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Employee _ObjEmployee)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                _ObjEmployee.ComId = Session["ComId"].ToString();
                Int64 Emp_Id = await Employee.Create(_ObjEmployee);
                if (Emp_Id != 0)
                {
                    //string strDocNo = Doctor_Code.ToString().Substring(2) + "-" + Doctor_Code.ToString().Substring(Doctor_Code.ToString().Length - 2);
                    string strDocNo = Emp_Id.ToString().Substring(2) + "-" + Emp_Id.ToString().Substring(Emp_Id.ToString().Length - 2);
                    result.DocNo = strDocNo;
                    result = new SaveViewModel()
                    {
                        Status = true,
                        Message = "",
                        DocNo = strDocNo
                    };
                }
                else
                {
                    result.Status = false;
                    result.Message = "Something went wrong.";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                result.Status = false;
                result.Message = "Something went wrong.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> Edit(Int64 EmpId)
        {
            var comid = Session["ComId"].ToString();
            List<Employee> _lstTD = await Employee.GetAllAsync(comid);

            return PartialView(await Employee.GetExistingAsync(EmpId));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Employee _ObjEmployee)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 Emp_Id = await Employee.Edit(_ObjEmployee);
                if (Emp_Id != 0)
                {
                    string strDocNo = Emp_Id.ToString().Substring(2) + "-" + Emp_Id.ToString().Substring(Emp_Id.ToString().Length - 2);
                    result.DocNo = strDocNo;
                    result = new SaveViewModel()
                    {
                        Status = true,
                        Message = "",
                        DocNo = strDocNo
                    };
                }
                else
                {
                    result.Status = false;
                    result.Message = "Something went wrong.";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                result.Status = false;
                result.Message = "Something went wrong.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Delete(Int64 EmpId)
        {
            var comid = Session["ComId"].ToString();
            List<Employee> _lstTD = await Employee.GetAllAsync(comid);

            return PartialView(await Employee.GetExistingAsync(EmpId));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Employee _ObjEmployee)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 Emp_Id = await Employee.Delete(_ObjEmployee);
                if (Emp_Id != 0)
                {
                    string strDocNo = Emp_Id.ToString().Substring(2) + "-" + Emp_Id.ToString().Substring(Emp_Id.ToString().Length - 2);
                    result.DocNo = strDocNo;
                    result = new SaveViewModel()
                    {
                        Status = true,
                        Message = "",
                        DocNo = strDocNo
                    };
                }
                else
                {
                    result.Status = false;
                    result.Message = "Something went wrong.";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                result.Status = false;
                result.Message = "Something went wrong.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}