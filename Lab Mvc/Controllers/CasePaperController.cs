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
    public class CasePaperController : Controller
    {
        // GET: CasePaper
        public async Task<ActionResult> Index(string sortdir, string sortOrder, string searchString, int? page)
        {
            List<CasePaper> _lstCitys = await CasePaper.GetAllAsync();

            string strSortDir = "";
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                  

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                _lstCitys = _lstCitys.Where(obj =>
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
                        _lstCitys = _lstCitys.OrderByDescending(obj => obj.TrnNo).ToList();
                    }
                    else
                    {
                        _lstCitys = _lstCitys.OrderBy(obj => obj.TrnNo).ToList();
                    }
                    break;
                case "Date":
                    if (strSortDir == "desc")
                    {
                        _lstCitys = _lstCitys.OrderByDescending(obj => obj.Date).ToList();
                    }
                    else
                    {
                        _lstCitys = _lstCitys.OrderBy(obj => obj.Date).ToList();
                    }
                    break;
                case "PatientName":
                    if (strSortDir == "desc")
                    {
                        _lstCitys = _lstCitys.OrderByDescending(obj => obj.PatientName).ToList();
                    }
                    else
                    {
                        _lstCitys = _lstCitys.OrderBy(obj => obj.PatientName).ToList();
                    }
                    break;
                default:
                    _lstCitys = _lstCitys.OrderByDescending(obj => obj.Date).ThenByDescending(p => p.TrnNo).ToList();
                    ViewBag.SortDir = "asc";
                    break;
            }
            
            return View(_lstCitys);
        }



        public ActionResult Create()
        {
            //ViewData["currentdate"] = DateUtility.GetCurrentDate();
            ViewData["currentdate"] = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
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
            List<CasePaper> _lstTD = await CasePaper.GetAllAsync();
            
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
            List<CasePaper> _lstTD = await CasePaper.GetAllAsync();

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

        public async Task<ActionResult> ApprovalPending()
        {
            List<CasePaper> _lstCasePaper = await CasePaper.GetApprovalPendingListAsync();

            return PartialView(_lstCasePaper);
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
               
    }
}