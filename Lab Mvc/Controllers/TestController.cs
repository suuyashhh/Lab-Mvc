using Lab.Businesss.Masters;
using Lab_Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc; 

namespace Lab_Mvc.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public async Task<ActionResult> Index(string sortOrder, string searchString, int? page)
        {
            List<Test> _lstTests = await Test.GetAllAsync();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;
                _lstTests = _lstTests.Where(obj => obj.TestName != null && obj.TestName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    _lstTests = _lstTests.OrderByDescending(obj => obj.TestName).ToList();
                    break;
                default:
                    _lstTests = _lstTests.OrderBy(obj => obj.TestName).ToList();
                    break;
            }
                        
            return View(_lstTests);
        }

        public ActionResult Create()
        {
            return PartialView(Test.New());
        }

        [HttpPost]
        public async Task<ActionResult> Create(Test _ObjTest)
        {          
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 trn_no = await Test.Create(_ObjTest);
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

    } 
}
