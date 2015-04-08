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
        private IGebruikerRepository GebruikerRepository;
        private Gebruiker Gebruiker;

        public ThemasController(IGebruikerRepository lt)
        {
            GebruikerRepository = lt;
            Gebruiker = GebruikerRepository.GetGebruiker(1);
            Gebruiker.LetterTuin = GebruikerRepository.GetGebruiker(1).LetterTuin;

            if (HttpContext.Session["gebruiker"] == null)
            {
                HttpContext.Session["gebruiker"] = GebruikerRepository.GetGebruiker(1);
            }
        }

        // GET: Themas
        public ActionResult Index()
        {
            return View(Gebruiker.LetterTuin.Themas.ToList());
        }

        // GET: Themas/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = Gebruiker.LetterTuin.GetThemaById(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // GET: Themas/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            return View();
        }

        // POST: Themas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gebruiker gebruiker, [Bind(Include = "IdThema,Themaa")] Thema thema)
        {
            if (ModelState.IsValid)
            {
                Gebruiker.AddThema(thema); 
                GebruikerRepository.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thema);
        }

        // GET: Themas/Edit/5
        public ActionResult Edit(Gebruiker gebruiker, int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = Gebruiker.LetterTuin.GetThemaById(id);
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
        public ActionResult Edit(Gebruiker gebruiker, [Bind(Include = "IdThema,Themaa")] Thema thema)
        {
            Thema th = Gebruiker.LetterTuin.GetThemaById(thema.IdThema);

            if (ModelState.IsValid)
            {
                try
                {
                    th.Update(thema.Themaa);
                    GebruikerRepository.SaveChanges();
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
        public ActionResult Delete(Gebruiker gebruiker, int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thema thema = Gebruiker.LetterTuin.GetThemaById(id);
            if (thema == null)
            {
                return HttpNotFound();
            }
            return View(thema);
        }

        // POST: Themas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker, int id)
        {
            Thema thema = Gebruiker.LetterTuin.GetThemaById(id);
            Gebruiker.RemoveThema(thema); 
            GebruikerRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
