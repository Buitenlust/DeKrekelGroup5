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
    public class CDController : Controller
    {
        private IGebruikerRepository gebruikersRep;

        public CDController(IGebruikerRepository gebruikerRepository)
        {
            gebruikersRep = gebruikerRepository;
        }

        // GET: CDs
        public ActionResult Index(Gebruiker gebruiker, String search = null)
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
                    //ViewBag.Selection = "Alle cds met " + search;
                }
                else
                {
                    cds = gebruiker.LetterTuin.GetCDs(null).ToList();
                    //ViewBag.Selection = "Alle cds";
                }

                if (Request.IsAjaxRequest())
                    return PartialView("CDsLijst", new MainViewModel(gebruiker).SetNewCDsLijstVm(cds));

                return View(new MainViewModel(gebruiker).SetNewCDsLijstVm(cds));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // GET: CDs/Details/5
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
                CD cd = gebruiker.LetterTuin.GetCD(id);
                if (cd == null)
                    mvm.SetNewInfo("CD niet gevonden");
                return View("Details", mvm.SetCDViewModel(cd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        // GET: CDs/Create
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

        // POST: CDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "CDCreateViewModel")] CDCreateViewModel cd, Gebruiker gebruiker, MainViewModel mvm, string image = null)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (ModelState.IsValid && cd != null && cd.CD.Exemplaar <= 0)
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
                        cd.CD.image = cd.CD.Titel.Replace(" ", "_") + ".jpg";
                        string path = System.IO.Path.Combine(Server.MapPath("~/FTP/Images"), cd.CD.image);
                        Client.DownloadFile(image, path);
                    }
                    else
                    {
                        cd.CD.image = mvm.CDCreateViewModel.CD.image;
                    }

                    List<Thema> themas = gebruiker.GetThemaListFromSelectedList(cd.SubmittedThemas);
                    CD newCD = cd.CD.MapToCD(cd.CD, themas);

                    gebruiker.AddCD(newCD);
                    //gebruikersRep.DoNotDuplicateThema(newCD);
                    gebruikersRep.SaveChanges();
                    mvm.SetNewInfo("CD" + cd.CD.Titel + " werd toegevoegd...");
                    return RedirectToAction("Details", new { gebruiker = gebruiker, mvm = mvm, id = gebruiker.LetterTuin.Cds.Max(b => b.Exemplaar) });
                }
                catch (NullReferenceException)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

            }
            return RedirectToAction("Create");
        }

        // GET: CDs/Edit/5
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
                CD cd = gebruiker.LetterTuin.GetCD(id);
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

        // POST: CDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                        //gebruikersRep.DoNotDuplicateThema(newCD);
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

        // GET: CDs/Delete/5
        public ActionResult Delete(Gebruiker gebruiker, int id = 0)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
            if (id <= 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                CD cd = gebruiker.LetterTuin.GetCD(id);
                if (cd == null)
                    return HttpNotFound();
                return View(new MainViewModel(gebruiker).SetCDViewModel(cd));
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        // POST: CDs/Delete/5
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
                CD cd = gebruiker.LetterTuin.GetCD(id);
                if (cd == null)
                    return HttpNotFound();
                gebruiker.RemoveCD(cd);
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
