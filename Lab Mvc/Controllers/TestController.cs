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
        public ActionResult Index()
        {
            return View();
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
                    return Content("0");
                }
                else
                {
                    return PartialView(_ObjCsPaper);
                }
            }
            catch
            {
                return PartialView(_ObjCsPaper);
            }
            //return View();
        }

    } 
}
