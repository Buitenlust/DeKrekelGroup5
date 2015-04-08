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
    public class UitlenersController : Controller
    {
        private IGebruikerRepository gebruikerRepository;
        private Gebruiker Gebruiker;

        public GebruikersController(IGebruikerRepository gebruikerRepository)
        {
            this.gebruikerRepository = gebruikerRepository;

            Gebruiker = HttpContext.
        }




        // GET: Uitleners
        public ActionResult Index()
        {
            return View(db.Uitleners.ToList());
        }

        // GET: Uitleners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uitlener uitlener = db.Uitleners.Find(id);
            if (uitlener == null)
            {
                return HttpNotFound();
            }
            return View(uitlener);
        }

        // GET: Uitleners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Uitleners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Naam,VoorNaam")] Uitlener uitlener)
        {
            if (ModelState.IsValid)
            {
                db.Uitleners.Add(uitlener);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uitlener);
        }

        // GET: Uitleners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uitlener uitlener = db.Uitleners.Find(id);
            if (uitlener == null)
            {
                return HttpNotFound();
            }
            return View(uitlener);
        }

        // POST: Uitleners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Naam,VoorNaam")] Uitlener uitlener)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uitlener).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uitlener);
        }

        // GET: Uitleners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uitlener uitlener = db.Uitleners.Find(id);
            if (uitlener == null)
            {
                return HttpNotFound();
            }
            return View(uitlener);
        }

        // POST: Uitleners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uitlener uitlener = db.Uitleners.Find(id);
            db.Uitleners.Remove(uitlener);
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
