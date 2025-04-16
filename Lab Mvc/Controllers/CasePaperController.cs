using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lab.Businesss.Masters;
using Lab_Mvc.Models;

namespace Lab_Mvc.Controllers
{
    [CustomAuthorize]
    public class CasePaperController : Controller
    {
        // GET: CasePaper

        public async Task<ActionResult> Index(string sortdir, string sortOrder, string searchString, int? page, string FromDate, string ToDate)
        {
           
            string strStartDate = "";
            string strToDate = "";
            string strSortDir = "";
            if (FromDate == null)
            {
                strStartDate = DateUtility.GetFormatedDate(DateUtility.GetCurrentMonthStartDate(), 1);
            }

            else
            {
                strStartDate = FromDate;
            }
            if (ToDate == null)
            {
                strToDate = DateUtility.GetFormatedDate(DateUtility.GetCurrentDate(), 1);
            }
            else
            {
                strToDate = ToDate;
            }

            List<CasePaper> _lstTD = await CasePaper.GetDateWiseAll(strStartDate, strToDate);
            TranGridSettings _objTranGridSettings = new TranGridSettings() { TranFromDate = DateUtility.GetFormatedDate(strStartDate, 0), TranToDate = DateUtility.GetFormatedDate(strToDate, 0) };
            ViewData["trangridsettings"] = _objTranGridSettings;

            ViewBag.FromDate = strStartDate;
            ViewBag.ToDate = strToDate;
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                  

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                _lstTD = _lstTD.Where(obj =>
                    (obj.Date != null && obj.Date.ToString().ToUpper().Contains(searchString.ToUpper())) ||
                    (obj.ShortTrnNo != null && obj.ShortTrnNo.ToUpper().Contains(searchString.ToUpper())) ||
                    (obj.PatientName != null && obj.PatientName.ToUpper().Contains(searchString.ToUpper())) 
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
                case "TrnNo":
                    if (strSortDir == "desc")
                    {
                        _lstTD = _lstTD.OrderByDescending(obj => obj.TrnNo).ToList();
                    }
                    else
                    {
                        _lstTD = _lstTD.OrderBy(obj => obj.TrnNo).ToList();
                    }
                    break;
                case "Date":
                    if (strSortDir == "desc")
                    {
                        _lstTD = _lstTD.OrderByDescending(obj => obj.Date).ToList();
                    }
                    else
                    {
                        _lstTD = _lstTD.OrderBy(obj => obj.Date).ToList();
                    }
                    break;
                case "PatientName":
                    if (strSortDir == "desc")
                    {
                        _lstTD = _lstTD.OrderByDescending(obj => obj.PatientName).ToList();
                    }
                    else
                    {
                        _lstTD = _lstTD.OrderBy(obj => obj.PatientName).ToList();
                    }
                    break;
                default:
                    _lstTD = _lstTD.OrderByDescending(obj => obj.Date).ThenByDescending(p => p.TrnNo).ToList();
                    ViewBag.SortDir = "asc";
                    break;
            }
            int pageSize = 20;
            int pageNumber = 1;
            if (page != null)
            {
                pageNumber = Convert.ToInt16(page);
            }

            //return View(_lstTD);
            return PartialView(_lstTD);
        }



        public ActionResult Create()
        {
            //ViewData["currentdate"] = DateUtility.GetCurrentDate();
            ViewData["currentdate"] = DateUtility.GetCurrentDate();
            List<Doctor> _objDoctor = Doctor.GetDoctorList();
            ViewData["doctor"] = _objDoctor;
            return PartialView("Create", CasePaper.New());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CasePaper _ObjCsPaper)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                _ObjCsPaper.CrtBy = Session["UserName"].ToString();
                _ObjCsPaper.ComId = Session["ComId"].ToString();
                Int64 trn_no = await CasePaper.Create(_ObjCsPaper);
                if (trn_no != 0)
                {
                    string strDocNo = trn_no.ToString().Substring(2, 6) + "-" + trn_no.ToString().Substring(trn_no.ToString().Length - 2);
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

        public async Task<ActionResult> Edit(Int64 TrnNo)
        {            
            
            List<Doctor> _objDoctor = Doctor.GetDoctorList();
            ViewData["doctor"] = _objDoctor;
            return PartialView(await CasePaper.GetExistingAsync(TrnNo));
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(CasePaper _ObjCsPaper)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {               
                Int64 trn_no = await CasePaper.Edit(_ObjCsPaper);
                if (trn_no != 0)
                {
                    string strDocNo = trn_no.ToString().Substring(2, 6) + "-" + trn_no.ToString().Substring(trn_no.ToString().Length - 2);
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
        
        public async Task<ActionResult> Delete(Int64 TrnNo)
        {
            List<Doctor> _objDoctor = Doctor.GetDoctorList();
            ViewData["doctor"] = _objDoctor;
            return PartialView(await CasePaper.GetExistingAsync(TrnNo));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(CasePaper _ObjCsPaper)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 trn_no = await CasePaper.Delete(_ObjCsPaper);
                if (trn_no != 0)
                {
                    string strDocNo = trn_no.ToString().Substring(2, 6) + "-" + trn_no.ToString().Substring(trn_no.ToString().Length - 2);
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

        public async Task<ActionResult> Details(Int64 TrnNo)
        {
            List<Doctor> _objDoctor = Doctor.GetDoctorList();
            ViewData["doctor"] = _objDoctor;
            return PartialView(await CasePaper.GetExistingAsync(TrnNo));
        }

        public async Task<ActionResult> Invoice(Int64 TrnNo)
        {
            
            List<Doctor> _objDoctor = Doctor.GetDoctorList();
            ViewData["doctor"] = _objDoctor;
            return PartialView(await CasePaper.GetExistingAsyncInvoice(TrnNo));
        }
        public async Task<ActionResult> InvoiceSave(Int64 TrnNo)
        {
            
            List<Doctor> _objDoctor = Doctor.GetDoctorList();
            ViewData["doctor"] = _objDoctor;
            return PartialView(await CasePaper.GetExistingAsyncInvoice(TrnNo));
        }

        [HttpPost]
        public async Task<ActionResult> InvoiceSave(CasePaper _ObjCsPaper)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 trn_no = await CasePaper.InvoiceSave(_ObjCsPaper);
                if (trn_no != 0)
                {
                    string strDocNo = trn_no.ToString().Substring(2, 6) + "-" + trn_no.ToString().Substring(trn_no.ToString().Length - 2);
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
        public async Task<ActionResult> Approve(Int64 TrnNo)
        {
            
            List<Doctor> _objDoctor = Doctor.GetDoctorList();
            ViewData["doctor"] = _objDoctor;
            return PartialView(await CasePaper.GetExistingAsync(TrnNo));
        }

        [HttpPost]
        public async Task<ActionResult> Approve(CasePaper _ObjCsPaper)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 trn_no = await CasePaper.Approve(_ObjCsPaper);
                if (trn_no != 0)
                {
                    string strDocNo = trn_no.ToString().Substring(2, 6) + "-" + trn_no.ToString().Substring(trn_no.ToString().Length - 2);
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
        public async Task<ActionResult> ApprovalPending()
        {
            List<CasePaper> _lstCasePaper = await CasePaper.GetApprovalPendingListAsync();

            return PartialView(_lstCasePaper);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveMultiple(List<Int64> TrnNos)
        {
            int result = 0;
            result = await CasePaper.Approve(TrnNos);
            if (result != 0)
            {
                return Content("0");
            }
            else
            {
                return Content("1");
            }
        }

        [HttpPost]
        public async Task<JsonResult> TestAutoFill(string searchtext)
        {
            if (string.IsNullOrEmpty(searchtext))
            {
                return Json(new List<Test>(), JsonRequestBehavior.AllowGet);
            }

            List<Test> testList = await Test.GetTestsAsync(searchtext);

            return Json(testList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            // Abandon session
            Session.Clear();
            Session.Abandon();

            // Clear cookie
            if (Request.Cookies["UserAuth"] != null)
            {
                var cookie = new HttpCookie("UserAuth")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Value = ""
                };
                Response.Cookies.Add(cookie);
            }

            return Redirect("~/Login.aspx?loggedout=true");

        }
    }
}