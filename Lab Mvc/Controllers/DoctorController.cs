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
    public class DoctorController : Controller
    {
        public async Task<ActionResult> Index(string sortdir, string sortOrder, string searchString, int? page)
        {
            var comid = Session["ComId"].ToString();
            List<Doctor> _lstDoctors = await Doctor.GetAllAsync(comid);

            string strSortDir = "";
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                _lstDoctors = _lstDoctors.Where(obj =>
                    (obj.DoctorName != null && obj.DoctorName.ToUpper().Contains(searchString.ToUpper()))
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
                        _lstDoctors = _lstDoctors.OrderByDescending(obj => obj.DoctorCode).ToList();
                    }
                    else
                    {
                        _lstDoctors = _lstDoctors.OrderBy(obj => obj.DoctorCode).ToList();
                    }
                    break;

                case "DoctorName":
                    if (strSortDir == "desc")
                    {
                        _lstDoctors = _lstDoctors.OrderByDescending(obj => obj.DoctorName).ToList();
                    }
                    else
                    {
                        _lstDoctors = _lstDoctors.OrderBy(obj => obj.DoctorName).ToList();
                    }
                    break;
                default:
                    _lstDoctors = _lstDoctors.OrderByDescending(p => p.DoctorCode).ToList();
                    ViewBag.SortDir = "asc";
                    break;
            }

            return View(_lstDoctors);
        }


        public ActionResult Create()
        {
            return PartialView(Doctor.New());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Doctor _ObjDoctor)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                _ObjDoctor.ComId = Session["ComId"].ToString();
                Int64 Doctor_Code = await Doctor.Create(_ObjDoctor);
                if (Doctor_Code != 0)
                {
                    string strDocNo = Doctor_Code.ToString().Substring(2) + "-" + Doctor_Code.ToString().Substring(Doctor_Code.ToString().Length - 2);
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