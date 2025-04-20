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
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            string comid = Session["ComId"].ToString();
            var casePaper = new CasePaper(); 
            var count = await casePaper.GetCountCurrentDate(comid);
            var ApproveCount = await casePaper.GetCountApprovePending(comid);
            ViewData["Count"] = count;
            ViewData["AppPenCount"] = ApproveCount;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}