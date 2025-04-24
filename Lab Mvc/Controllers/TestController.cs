using Lab.Businesss.Masters;
using Lab_Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc; 

namespace Lab_Mvc.Controllers
{
    [CustomAuthorize]
    public class TestController : Controller
    {
        // GET: Test
        public async Task<ActionResult> Index(string sortdir, string sortOrder, string searchString, int? page)
        {
            var comid = Session["ComId"].ToString();
            List<Test> _lstTests = await Test.GetAllAsync(comid);

            string strSortDir = "";
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (!string.IsNullOrEmpty(searchString))
            {
                ViewBag.CurrentFilter = searchString;

                _lstTests = _lstTests.Where(obj =>
                    (obj.ShortTrnNo != null && obj.ShortTrnNo.ToUpper().Contains(searchString.ToUpper())) ||
                    (obj.TestName != null && obj.TestName.ToUpper().Contains(searchString.ToUpper()))
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
                case "TestCode":
                    if (strSortDir == "desc")
                    {
                        _lstTests = _lstTests.OrderByDescending(obj => obj.TestCode).ToList();
                    }
                    else
                    {
                        _lstTests = _lstTests.OrderBy(obj => obj.TestCode).ToList();
                    }
                    break;
              
                case "TestName":
                    if (strSortDir == "desc")
                    {
                        _lstTests = _lstTests.OrderByDescending(obj => obj.TestName).ToList();
                    }
                    else
                    {
                        _lstTests = _lstTests.OrderBy(obj => obj.TestName).ToList();
                    }
                    break;
                default:
                    _lstTests = _lstTests.OrderByDescending(p => p.TestCode).ToList();
                    ViewBag.SortDir = "asc";
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
                _ObjTest.CrtBy = Session["UserName"].ToString();
                _ObjTest.ComId = Session["ComId"].ToString();
                Int64 Test_Code = await Test.Create(_ObjTest);
                if (Test_Code != 0)
                {
                    string strDocNo = Test_Code.ToString().Substring(2) + "-" + Test_Code.ToString().Substring(Test_Code.ToString().Length - 2);
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


        public async Task<ActionResult> Edit(Int64 TestCode)
        {
            var comid = Session["ComId"].ToString();
            List<Test> _lstTD = await Test.GetAllAsync(comid);

            return PartialView(await Test.GetExistingAsync(TestCode));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Test _ObjTest)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 Test_Code = await Test.Edit(_ObjTest);
                if (Test_Code != 0)
                {
                    string strDocNo = Test_Code.ToString().Substring(2) + "-" + Test_Code.ToString().Substring(Test_Code.ToString().Length - 2);
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

        public async Task<ActionResult> Delete(Int64 TestCode)
        {
            var comid = Session["ComId"].ToString();
            List<Test> _lstTD = await Test.GetAllAsync(comid);

            return PartialView(await Test.GetExistingAsync(TestCode));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Test _ObjTest)
        {
            var result = new SaveViewModel() { Status = true };

            try
            {
                Int64 Test_Code = await Test.Delete(_ObjTest);
                if (Test_Code != 0)
                {
                    string strDocNo = Test_Code.ToString().Substring(2) + "-" + Test_Code.ToString().Substring(Test_Code.ToString().Length - 2);
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
