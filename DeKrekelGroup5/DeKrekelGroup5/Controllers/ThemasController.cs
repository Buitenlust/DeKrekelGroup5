using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Controllers
{
    public class ThemasController : Controller
    {
        private IThemasRepository tr;


        public ThemasController(IThemasRepository themasRepository)
        {
            tr = themasRepository;
        }

        // GET: Themas
        public ActionResult Index()
        {
            return View(tr.FindAll().ToList());
        }

        // GET: Themas/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = tr.FindById(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // GET: Themas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Themas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdThema,Themaa")] Thema thema)
        {
            if (ModelState.IsValid)
            {
                tr.Add(thema);
                tr.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thema);
        }

        // GET: Themas/Edit/5
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = tr.FindById(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // POST: Themas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdThema,Themaa")] Thema thema)
        {
            Thema th = tr.FindById(thema.IdThema);

            if (ModelState.IsValid)
            {
                try
                {
                    th.Update(thema.Themaa);
                    tr.SaveChanges();
                    TempData["Info"] = "Het thema werd aangepast...";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
                
                return RedirectToAction("Index");
            }
            return View(thema);
        }

        // GET: Themas/Delete/5
        public ActionResult Delete(int id=0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = tr.FindById(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // POST: Themas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Thema thema = tr.FindById(id);
            tr.Remove(thema);
            tr.Remove(thema);
            tr.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
