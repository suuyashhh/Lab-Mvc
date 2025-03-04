using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lab.Businesss.Masters;

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
            try
            {
                Int64 result = 0;
                result = await CasePaper.Create(_ObjCsPaper);
                if (result != 0)
                {
                    return Json(new { Status = true, Message = "" });
                }
                else
                {
                    return Json(new { Status = false, Message = "" });
                }
            }
            catch (Exception ex)
            {                
                return Json(new { Status = false, Message = "An error occurred: " + ex.Message });
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