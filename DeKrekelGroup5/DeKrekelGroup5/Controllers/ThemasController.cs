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
        private KrekelContext db = new KrekelContext();

        // GET: Themas
        public ActionResult Index()
        {
            return View(db.Themas.ToList());
        }

        // GET: Themas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = db.Themas.Find(id);
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
                db.Themas.Add(thema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thema);
        }

        // GET: Themas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = db.Themas.Find(id);
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
            if (ModelState.IsValid)
            {
                db.Entry(thema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thema);
        }

        // GET: Themas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = db.Themas.Find(id);
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
            Thema thema = db.Themas.Find(id);
            db.Themas.Remove(thema);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
