using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5;

namespace DeKrekelGroup5.Controllers
{
    public class itemsController : Controller
    {
        private dekrekelEntities db = new dekrekelEntities();

        // GET: items
        public ActionResult Index()
        {
            var item = db.item.Include(i => i.genre).Include(i => i.type);
            return View(item.ToList());
        }

        // GET: items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: items/Create
        public ActionResult Create()
        {
            ViewBag.genre_idgenre = new SelectList(db.genre, "idgenre", "beschrijving");
            ViewBag.type_idtype = new SelectList(db.type, "idtype", "beschrijving");
            return View();
        }

        // POST: items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "exemplaar,titel,uitgever,omschrijving,jaar,isbn,auteur,imdb,genre_idgenre,type_idtype")] item item)
        {
            if (ModelState.IsValid)
            {
                db.item.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.genre_idgenre = new SelectList(db.genre, "idgenre", "beschrijving", item.genre_idgenre);
            ViewBag.type_idtype = new SelectList(db.type, "idtype", "beschrijving", item.type_idtype);
            return View(item);
        }

        // GET: items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.genre_idgenre = new SelectList(db.genre, "idgenre", "beschrijving", item.genre_idgenre);
            ViewBag.type_idtype = new SelectList(db.type, "idtype", "beschrijving", item.type_idtype);
            return View(item);
        }

        // POST: items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "exemplaar,titel,uitgever,omschrijving,jaar,isbn,auteur,imdb,genre_idgenre,type_idtype")] item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.genre_idgenre = new SelectList(db.genre, "idgenre", "beschrijving", item.genre_idgenre);
            ViewBag.type_idtype = new SelectList(db.type, "idtype", "beschrijving", item.type_idtype);
            return View(item);
        }

        // GET: items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            item item = db.item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            item item = db.item.Find(id);
            db.item.Remove(item);
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
