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
    public class CDController : Controller
    {
        private IGebruikerRepository gebruikersRep; 

        public CDController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: CDs
        public ActionResult Index( Gebruiker gebruiker, String search=null)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            else
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<CD> cds;
                if (!String.IsNullOrEmpty(search))
                {
                    cds = gebruiker.LetterTuin.GetCDs(search).ToList();
                    //ViewBag.Selection = "Alle CDs met " + search;
                }
                else
                {
                    cds = gebruiker.LetterTuin.GetCDs(null).ToList();
                    //ViewBag.Selection = "Alle CDs";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("CDLijst", new MainViewModel(gebruiker).SetNewCDLijstVm(cds));

                return View(new MainViewModel(gebruiker).SetNewCDLijstVm(cds));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
            
        }

        // GET: CD/Details/5
        public ActionResult Details(Gebruiker gebruiker, MainViewModel mvm, int id = 0)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            else
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.SetGebruikerToVm(gebruiker);
            mvm.InfoViewModel.Info = null;
            try
            {
                if (id == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    mvm.SetNewInfo("CD niet gevonden");
                return View("Details", mvm.SetCDViewModel(cd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // GET: CD/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {

                mvm.SetCDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new CD());
                HttpContext.Session["main"] = mvm;
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: CD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "CDCreateViewModel")] CDCreateViewModel cd, Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && cd != null && cd.CD.Exemplaar <= 0)
            {
                try
                {
                    cd.CD.image = mvm.CDCreateViewModel.CD.image;
                    List<Thema> themas = gebruiker.GetThemaListFromSelectedList(cd.SubmittedThemas);
                    CD newCD = cd.CD.MapToCD(cd.CD, themas);

                    gebruiker.AddItem(newCD);
                    //gebruikersRep.DoNotDuplicateThema(newBoek);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("CD" + cd.CD.Titel + " werd toegevoegd...");
                    return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = gebruiker.LetterTuin.Items.Max(b => b.Exemplaar) });
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

            }
            return RedirectToAction("Create");
        }

        // GET: CD/Edit/5
        public ActionResult Edit(Gebruiker gebruiker,int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            MainViewModel mvm = new MainViewModel(gebruiker);
            mvm.InfoViewModel.Info = null;
            mvm.SetGebruikerToVm(gebruiker);
            try
            {
                if (id <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    return HttpNotFound();
                mvm.SetCDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), cd);
                HttpContext.Session["main"] = mvm;

                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            
        }

        // POST: CD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "CDCreateViewModel")] CDCreateViewModel cd, Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase newimage = null)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            mvm.InfoViewModel.Info = null;
            mvm.SetGebruikerToVm(gebruiker);
            if (newimage != null)
            {
                return UploadImage(gebruiker, mvm, newimage);
            }
            else
            {
                if (ModelState.IsValid && cd != null && cd.CD.Exemplaar > 0)
                {
                    try
                    {
                        cd.CD.image = mvm.CDCreateViewModel.CD.image;

                        List<Thema> themas = gebruiker.GetThemaListFromSelectedList(cd.SubmittedThemas);
                        CD newCD = cd.CD.MapToCD(cd.CD, themas);
                        gebruiker.UpdateCD(newCD);
                        //gebruikersRep.DoNotDuplicateThema(newBoek);
                        mvm.SetNewInfo("CD " + cd.CD.Titel + " werd aangepast...");
                        gebruikersRep.SaveChanges();
                        return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = newCD.Exemplaar });
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
                mvm.CDCreateViewModel.Themas = new SelectList(gebruiker.LetterTuin.Themas.ToList());
                return View(new MainViewModel(gebruiker) { CDCreateViewModel = cd });
            }
        }

        // GET: CD/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id=0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetCDViewModel(cd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: CD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Gebruiker gebruiker, int id)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                MainViewModel mvm = new MainViewModel(gebruiker);
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                CD cd = gebruiker.LetterTuin.GetItem(id) as CD;
                if (cd == null)
                    return HttpNotFound();
                gebruiker.RemoveItem(cd);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("CD" + cd.Titel + " werd verwijderd...");
                return View("Index", mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            
        }

        [HttpPost]
        public ActionResult UploadImage(Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase newimage)
        {
            if (newimage != null)
            {
                int rnd = new Random().Next(99999);
                string pic = System.IO.Path.GetFileName(newimage.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/FTP/Images"), rnd + pic);
                ;

                // file is uploaded
                newimage.SaveAs(path);
                mvm.CDCreateViewModel.CD.image = rnd + pic;
                return Json(new { imagePath = rnd + pic });
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}