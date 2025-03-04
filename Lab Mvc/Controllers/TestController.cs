using Lab.Businesss.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc; // Ensure this namespace matches your Models

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

        //[HttpPost]
        //public async Task<ActionResult> Create(Test _ObjCsPaper)
        //{
        //    try
        //    {
        //        Int64 result = 0;
        //        result = await Test.Create(_ObjCsPaper);
        //        if (result != 0)
        //        {
        //            return Content("0");
        //        }
        //        else
        //        {
        //            return PartialView(_ObjCsPaper);
        //        }
        //    }
        //    catch
        //    {
        //        return PartialView(_ObjCsPaper);
        //    }
        //    //return View();
        //}

    } 
}
