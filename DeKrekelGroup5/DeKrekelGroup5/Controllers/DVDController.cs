using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class DVDController : Controller
    {
        private IGebruikerRepository gebruikersRep;

        public DVDController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: DVDs
        public ActionResult Index(Gebruiker gebruiker, String search = null)
        {
            if (gebruiker == null)
                gebruiker = gebruikersRep.GetGebruikerByName("Anonymous");
            else
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {
                IEnumerable<DVD> dvds;
                if (!String.IsNullOrEmpty(search))
                {
                    dvds = gebruiker.LetterTuin.GetDVDs(search).ToList();
                    //ViewBag.Selection = "Alle dvds met " + search;
                }
                else
                {
                    dvds = gebruiker.LetterTuin.GetDVDs(null).ToList();
                    //ViewBag.Selection = "Alle dvds";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("DVDsLijst", new MainViewModel(gebruiker).SetNewDVDsLijstVm(dvds));

                return View(new MainViewModel(gebruiker).SetNewDVDsLijstVm(dvds));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: DVDs/Details/5
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
                DVD dvd = gebruiker.LetterTuin.GetDVD(id);
                if (dvd == null)
                    mvm.SetNewInfo("DVD niet gevonden");
                return View("Details", mvm.SetDVDViewModel(dvd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        // GET: DVDs/Create
        public ActionResult Create(Gebruiker gebruiker)
        {
            MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            try
            {

                mvm.SetDVDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), new DVD());
                HttpContext.Session["main"] = mvm;
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: DVDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "DVDCreateViewModel")] DVDCreateViewModel dvd, Gebruiker gebruiker, MainViewModel mvm, string image = null)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && dvd != null && dvd.DVD.Exemplaar <= 0)
            {
                try
                {
                    if (!image.IsNullOrWhiteSpace())
                    {
                        WebClient Client = new WebClient();
                        //WebRequest request = FtpWebRequest.Create(image);

                        //using (WebResponse response = request.GetResponse())
                        //{
                        //    Stream responseStream = response.GetResponseStream();
                        //    responseStream.CopyTo();
                        //    fb.InputStream.Read() = responseStream;
                        //}

                        //HttpPostedFileBase fb = new HttpPostedFileWrapper();
                        //fb.InputStream.BeginRead(); 
                        dvd.DVD.image = dvd.DVD.Titel.Replace(" ", "_") + ".jpg";
                        string path = System.IO.Path.Combine(Server.MapPath("~/FTP/Images"), dvd.DVD.image);
                        Client.DownloadFile(image, path);
                    }
                    else
                    {
                        dvd.DVD.image = mvm.DVDCreateViewModel.DVD.image;
                    }

                    List<Thema> themas = gebruiker.GetThemaListFromSelectedList(dvd.SubmittedThemas);
                    DVD newDVD = dvd.DVD.MapToDVD(dvd.DVD, themas);

                    gebruiker.AddDVD(newDVD);
                    //gebruikersRep.DoNotDuplicateThema(newDVD);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("DVD" + dvd.DVD.Titel + " werd toegevoegd...");
                    return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = gebruiker.LetterTuin.Dvds.Max(b => b.Exemplaar) });
                }
                catch (NullReferenceException)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

            }
            return RedirectToAction("Create");
        }

        // GET: DVDs/Edit/5
        public ActionResult Edit(Gebruiker gebruiker, int id = 0)
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
                DVD dvd = gebruiker.LetterTuin.GetDVD(id);
                if (dvd == null)
                    return HttpNotFound();
                mvm.SetDVDCreateViewModel(gebruiker.LetterTuin.Themas.ToList(), dvd);
                HttpContext.Session["main"] = mvm;

                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        // POST: DVDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "DVDCreateViewModel")] DVDCreateViewModel dvd, Gebruiker gebruiker, MainViewModel mvm, HttpPostedFileBase newimage = null)
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
                if (ModelState.IsValid && dvd != null && dvd.DVD.Exemplaar > 0)
                {
                    try
                    {
                        dvd.DVD.image = mvm.DVDCreateViewModel.DVD.image;

                        List<Thema> themas = gebruiker.GetThemaListFromSelectedList(dvd.SubmittedThemas);
                        DVD newDVD = dvd.DVD.MapToDVD(dvd.DVD, themas);
                        gebruiker.UpdateDVD(newDVD);
                        //gebruikersRep.DoNotDuplicateThema(newDVD);
                        mvm.SetNewInfo("DVD " + dvd.DVD.Titel + " werd aangepast...");
                        gebruikersRep.SaveChanges();
                        return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = newDVD.Exemplaar });
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                }
                mvm.DVDCreateViewModel.Themas = new SelectList(gebruiker.LetterTuin.Themas.ToList());
                return View(new MainViewModel(gebruiker) { DVDCreateViewModel = dvd });
            }



        }

        // GET: DVDs/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                DVD dvd = gebruiker.LetterTuin.GetDVD(id);
                if (dvd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetDVDViewModel(dvd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: DVDs/Delete/5
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
                DVD dvd = gebruiker.LetterTuin.GetDVD(id);
                if (dvd == null)
                    return HttpNotFound();
                gebruiker.RemoveDVD(dvd);
                gebruikersRep.SaveChanges();
                mvm.SetNewInfo("DVD" + dvd.Titel + " werd verwijderd...");
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
                mvm.DVDCreateViewModel.DVD.image = rnd + pic;
                return Json(new { imagePath = rnd + pic });
            }

            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }
    }
}
