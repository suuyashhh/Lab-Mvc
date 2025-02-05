using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc; // Ensure this namespace matches your Models

namespace Lab_Mvc.Controllers
{
    public class TestController : Controller
    {
        private static List<Test> testList = new List<Test>
        {
            new Test { Id = 1, Name = "Sugar Test", Price = 240 },
            new Test { Id = 2, Name = "Thyroid Test", Price = 170 },
            new Test { Id = 3, Name = "Urine Test", Price = 500 }
        };

        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        // GET: Test/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Test model)
        {
            if (ModelState.IsValid)
            {
                model.Id = testList.Max(t => t.Id) + 1; // Generate a new ID
                testList.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Test/Edit/5
        public ActionResult Edit(int id)
        {
            var test = testList.FirstOrDefault(t => t.Id == id);
            if (test == null) return HttpNotFound();
            return View(test);
        }

        // POST: Test/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Test model)
        {
            var test = testList.FirstOrDefault(t => t.Id == model.Id);
            if (test != null)
            {
                test.Name = model.Name;
                test.Price = model.Price;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // POST: Test/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var test = testList.FirstOrDefault(t => t.Id == id);
            if (test != null)
            {
                testList.Remove(test);
            }
            return RedirectToAction("Index");
        }
    }

    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
