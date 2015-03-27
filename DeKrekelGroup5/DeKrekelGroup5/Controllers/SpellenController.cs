using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class SpellenController : Controller
    {
        private LetterTuin letterTuin;
        private Beheerder beheerder;

        public SpellenController(ISpellenRepository spellenRepository, IThemasRepository themasRepository)
        {
            letterTuin = new LetterTuin();
            beheerder = new Beheerder();
        }

        // GET: Spellen
        public ActionResult Index(String search=null)
        {
            IEnumerable<Spel> spellen;
            if (!String.IsNullOrEmpty(search))
            {
                spellen = letterTuin.GetSpellen(search);
                ViewBag.Selection = "Alle spellen met " + search;
            }
            else
            {
                spellen = letterTuin.GetSpellen(search).Take(25);
                ViewBag.Selection = "Alle spellen";
            }
            if (Request.IsAjaxRequest())
                return PartialView("SpellenLijst", new SpellenLijstViewModel(spellen));

            return View(new SpellenLijstViewModel(spellen));
        }

        // GET: Spellen/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Spel spel = letterTuin.GetItem(id) as Spel;
            if (spel == null)
                return HttpNotFound();
            return View(spel);
        }

        // GET: Spellen/Create
        public ActionResult Create()
        {
            return View(new SpelCreateViewModel(beheerder.GetThemas(), new Spel()));
        }

        // POST: Spellen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "Spel")] SpelViewModel spel)
        {
            if (ModelState.IsValid)
            {
                Spel newSpel = new Spel()
                {
                    Titel = spel.Titel, 
                    Leeftijd = spel.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = beheerder.GetThemaByName(spel.Thema),
                    Omschrijving = spel.Omschrijving,
                    Exemplaar = 0,
                    Uitgever = spel.Uitgever

                };
                beheerder.AddItem(newSpel);
                TempData["Info"] = "Het spel werd toegevoegd...";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: Spellen/Edit/5
        public ActionResult Edit(int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Spel spel = beheerder.GetItem(id) as Spel;
            if (spel == null)
                return HttpNotFound();
            return View(new SpelCreateViewModel(beheerder.GetThemas(), spel));
        }

        // POST: Spellen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "Spel")] SpelViewModel spel)
        {
            if (ModelState.IsValid)
            {
                Spel newSpel = new Spel()
                {
                    Titel = spel.Titel,
                    Leeftijd = spel.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = beheerder.GetThemaByName(spel.Thema),
                    Omschrijving = spel.Omschrijving,
                    Exemplaar = spel.Exemplaar,
                    Uitgever = spel.Uitgever

                };

                beheerder.AddItem(newSpel);

                return RedirectToAction("Index");
            }
            return View(spel);
        }

        // GET: Spellen/Delete/5
        public ActionResult Delete(int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Spel spel = beheerder.GetItem(id) as Spel;
            if (spel == null)
            {
                return HttpNotFound();
            }
            return View(spel);
        }

        // POST: Spellen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        { 
            beheerder.RemoveItem(id);
            return RedirectToAction("Index");
        }
    }
}
