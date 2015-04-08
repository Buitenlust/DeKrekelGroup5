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
    public class BoekenController : Controller
    {
        private IGebruikerRepository gebruikersRep;
        private Gebruiker Gebruiker;
 
        public BoekenController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
           
        }

        // GET: Boeken
        public ActionResult Index( Gebruiker gebruiker, String search=null)
        {
            Gebruiker = gebruiker;
            if (gebruiker == null)
                Gebruiker = gebruikersRep.GetGebruiker(1);
           
            IEnumerable<Boek> boeken;
            if (!String.IsNullOrEmpty(search))
            {
                boeken = Gebruiker.LetterTuin.GetBoeken(search);
                ViewBag.Selection = "Alle boeken met " + search;
            }
            else
            {
                boeken = Gebruiker.LetterTuin.GetBoeken(null);
                ViewBag.Selection = "Alle boeken";
            }
            
            if (Request.IsAjaxRequest())
                return PartialView("BoekenLijst", new BoekenLijstViewModel(boeken){IsAdmin = Gebruiker.AdminRechten, IsBibliothecaris = Gebruiker.BibliotheekRechten});
            
            return View(new BoekenLijstViewModel(boeken){ IsAdmin = Gebruiker.AdminRechten, IsBibliothecaris = Gebruiker.BibliotheekRechten });
        }

        // GET: Boeken/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Boek boek = Gebruiker.LetterTuin.GetItem(id) as Boek;
            if (boek == null)
                return HttpNotFound();
            return View(boek);
        }

        // GET: Boeken/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            if(gebruiker != null && gebruiker.AdminRechten)
                return View(new BoekCreateViewModel(Gebruiker.LetterTuin.Themas.ToList(), new Boek()));
            return RedirectToAction("Login", "AdminLogin");
            
        }

        // POST: Boeken/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "Boek")] BoekViewModel boek, Gebruiker gebruiker = null)
        {
            if (ModelState.IsValid)
            {
                Boek newBoek = new Boek()
                {
                    Titel = boek.Titel,
                    Auteur = boek.Auteur,
                    Leeftijd = boek.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = Gebruiker.LetterTuin.GetThemaByName(boek.Thema),
                    Omschrijving = boek.Omschrijving,
                    Exemplaar = 0,
                    Uitgever = boek.Uitgever
                };

                Gebruiker.AddItem(newBoek);
                gebruikersRep.DoNotDuplicateThema(newBoek);
                gebruikersRep.SaveChanges();
                TempData["Info"] = "Het boek werd toegevoegd...";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: Boeken/Edit/5
        public ActionResult Edit(Gebruiker gebruiker= null,int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Boek boek = Gebruiker.LetterTuin.GetItem(id) as Boek;
            if (boek == null)
                return HttpNotFound();
            return View(new BoekCreateViewModel(Gebruiker.LetterTuin.Themas, boek));
        }

        // POST: Boeken/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "Boek")] BoekViewModel boek, Gebruiker gebruiker=null)
        {
            if (ModelState.IsValid)
            {
                Boek newBoek = new Boek()
                {
                    Titel = boek.Titel,
                    Auteur = boek.Auteur,
                    Leeftijd = boek.Leeftijd,
                    Beschikbaar = true,
                    Uitgeleend = false,
                    Themaa = Gebruiker.LetterTuin.GetThemaByName(boek.Thema),
                    Omschrijving = boek.Omschrijving,
                    Exemplaar = boek.Exemplaar,
                    Uitgever = boek.Uitgever

                };
                Gebruiker.UpdateBoek(newBoek);
                gebruikersRep.DoNotDuplicateThema(newBoek);
                gebruikersRep.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boek);
        }

        // GET: Boeken/Delete/5
        public ActionResult Delete(Gebruiker gebruiker=null, int id=0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Boek boek = Gebruiker.LetterTuin.GetItem(id) as Boek;
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        // POST: Boeken/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker, int id)
        {
            Gebruiker.RemoveItem(id);
            gebruikersRep.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
