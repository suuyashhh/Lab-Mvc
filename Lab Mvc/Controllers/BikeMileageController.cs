using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_Mvc.Controllers
{
    public class BikeMileageController : Controller
    {
        // GET: BikeMileage
        public ActionResult Index()
        {
            return View();
        }
    }
}