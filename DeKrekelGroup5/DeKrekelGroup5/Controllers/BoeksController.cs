using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;

namespace DeKrekelGroup5.Controllers
{
    public class BoeksController : Controller
    {
        private IBoekenRepository br;
        private IThemasRepository tr;

        public BoeksController(IBoekenRepository boekenRepository, IThemasRepository themasRepository)
        {
            br = boekenRepository;
            tr = themasRepository;
        }

        // GET: Boeks
        public ActionResult Index()
        {
            return View(br.FindAll().ToList());
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
            return View();
        }

        // POST: Boeks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Exemplaar,Auteur,Uitgever,Jaar,isbn,Titel,Omschrijving")] Boek boek)
        {
            if (ModelState.IsValid)
            {
                Thema th = new Thema();
                th.IdThema = 1;
                th.Themaa = "Roman";
                boek.Themaa = th;
                br.Add(boek);
                br.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(new BoekThemaCreateViewModel(tr.FindAll().OrderBy(n => n.Themaa), new Boek().ConvertToBrouwerCreateViewModel()) );
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
            return View(boek);
        }

        // POST: Boeks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Exemplaar,Auteur,Uitgever,Jaar,isbn,Titel,Omschrijving")] Boek boek)
        {
            Boek bk = br.FindById(boek.Exemplaar);

            if (ModelState.IsValid)
            {
                try
                {
                    bk.Update(boek);
                    br.SaveChanges();
                    TempData["Info"] = "Het boek werd aangepast...";
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }

                return RedirectToAction("Index");
            }
            return View(boek);
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
            br.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
