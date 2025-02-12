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
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Create()
        {
            return PartialView(CasePaper.New());
        }

        [HttpPost]
        public async Task<ActionResult>Create(CasePaper _ObjCsPaper)
        {
            try
            {
                int result = 0;
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