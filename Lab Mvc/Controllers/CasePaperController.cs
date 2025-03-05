using System;
using System.Collections.Generic;
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
        public async Task<ActionResult> Index(string sortOrder, string searchString, int? page)
        {
            List<CasePaper> _lstCitys = await CasePaper.GetAllAsync();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;
                _lstCitys = _lstCitys.Where(obj => obj.PatientName != null && obj.PatientName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    _lstCitys = _lstCitys.OrderByDescending(obj => obj.PatientName).ToList();
                    break;
                default:
                    _lstCitys = _lstCitys.OrderBy(obj => obj.PatientName).ToList();
                    break;
            }

            
            return View(_lstCitys);
        }



        public ActionResult Create()
        {
            return PartialView(CasePaper.New());
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