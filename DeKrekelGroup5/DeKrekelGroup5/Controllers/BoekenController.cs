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
        private IBoekenRepository br;
        private IThemasRepository tr;

        public BoekenController(IBoekenRepository boekenRepository, IThemasRepository themasRepository)
        {
            br = boekenRepository;
            tr = themasRepository;
        }

        // GET: Boeks
        public ActionResult Index(String search=null)
        {
            IEnumerable<Boek> boeken;
            if (!String.IsNullOrEmpty(search))
            {
                boeken = br.Find(search).ToList();
                ViewBag.Selection = "Alle boeken met " + search;
            }
            else
            {
                boeken = br.FindAll().OrderBy(p => p.Titel).ToList().Take(25);
                ViewBag.Selection = "Alle boeken";
            }
            if (Request.IsAjaxRequest())
                return PartialView("BoekenLijst", new BoeksIndexViewModel(boeken));
            return View(new BoeksIndexViewModel(boeken));


            //return View(new BoeksIndexViewModel(br.FindAll().OrderBy(p => p.Exemplaar).ToList()));
        }

        // GET: Boeks/Details/5
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Boek boek = br.FindById(id);
            
            if (boek == null)
            {
                return HttpNotFound();
            }
            else
            {
                boek.Themaa = tr.FindById(boek.Themaa.IdThema);
            }

            return View(boek);
        }

        // GET: Boeks/Create
        public ActionResult Create()
        {
            return View(new BoekThemaCreateViewModel(tr.FindAll().OrderBy(n => n.Themaa), new Boek().ConvertToBoekCreateViewModel()));
        }

        // POST: Boeks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "Boek")] BoekCreateViewModel boek)
        {
            if (ModelState.IsValid)
            {
                Boek newBoek = new Boek
                {
                    Exemplaar = boek.Exemplaar,
                    Auteur = boek.Auteur,
                    Leeftijd =  boek.Leeftijd,
                    Omschrijving = boek.Omschrijving,
                    Titel = boek.Titel,
                    Uitgever = boek.Uitgever, 
                    Themaa = (String.IsNullOrEmpty(boek.Thema) ? null : tr.FindBy(boek.Thema))
                };

                br.Add(newBoek);
                
                br.SaveChanges(newBoek);
                TempData["Info"] = "Het boek werd toegevoegd...";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

        // GET: Boeks/Edit/5
        public ActionResult Edit(int id=0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Boek boek = br.FindById(id);
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(new BoekThemaCreateViewModel(tr.FindAll().OrderBy(n => n.Themaa), boek.ConvertToBoekCreateViewModel()));
        }

        // POST: Boeks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "Boek")] BoekCreateViewModel boek)
        {
            Boek bk = null;
            if (ModelState.IsValid)
            {
                bk = br.FindById(boek.Exemplaar);

                if (bk == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    bk.Exemplaar = boek.Exemplaar;
                    bk.Auteur = boek.Auteur;
                    bk.Leeftijd = boek.Leeftijd;
                    bk.Omschrijving = boek.Omschrijving;
                    bk.Titel = boek.Titel;
                    bk.Uitgever = boek.Uitgever; 
                    bk.Themaa = (String.IsNullOrEmpty(boek.Thema) ? null : tr.FindBy(boek.Thema));

                    bk.Update(bk);
                    br.SaveChanges(bk);
                    TempData["Info"] = "Het boek werd aangepast...";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }

                return RedirectToAction("Index");
            }
            return View(new BoekThemaCreateViewModel(tr.FindAll().OrderBy(n => n.Themaa), boek));
        }

        // GET: Boeks/Delete/5
        public ActionResult Delete(int id=0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 

            Boek boek = br.FindById(id);
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        // POST: Boeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Boek boek = br.FindById(id);
            br.Remove(boek);
            br.SaveChanges(boek);
            return RedirectToAction("Index");
        }
    }
}
