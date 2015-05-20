using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;

namespace DeKrekelGroup5.Controllers
{
    public class InstellingenController : Controller
    {

        private IGebruikerRepository gebruikersRep;

        public InstellingenController(IGebruikerRepository lt)
        {
            gebruikersRep = lt; 
        }


        // GET: Instellingen
        public ActionResult Index(Gebruiker gebruiker)
        {
             MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            try
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                mvm.SetGebruikerToVm(gebruiker);
                mvm.InstellingenViewModel = new InstellingenViewModel(gebruiker.GetInstellingen()); 
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        // GET: Themas/Edit/5
        public ActionResult Edit(Gebruiker gebruiker)
        {
            MainViewModel mvm = new MainViewModel(gebruiker);
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return PartialView(new MainViewModel().SetNewInfo("U moet hiervoor inloggen!", true));
            try
            {
                gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);
                mvm.SetGebruikerToVm(gebruiker);
                mvm.InstellingenViewModel = new InstellingenViewModel(gebruiker.GetInstellingen()); 
                return View(mvm);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }




        // POST: Themas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "InstellingenViewModel")] InstellingenViewModel instellingenVm,
            Gebruiker gebruiker, MainViewModel mvm)
        {
            if (gebruiker == null || gebruiker.AdminRechten == false)
                return new HttpUnauthorizedResult();
            gebruiker = gebruikersRep.GetGebruikerByName(gebruiker.GebruikersNaam);

            if (ModelState.IsValid)
            {
                try
                {
                    gebruiker.UpdateInstellingen(instellingenVm);
                    if (!instellingenVm.BeheerderPaswoord.IsNullOrWhiteSpace())
                        gebruiker.VeranderPaswoord(instellingenVm.BeheerderPaswoord, gebruiker);
                    if (!instellingenVm.BibliothecarisPaswoord.IsNullOrWhiteSpace())
                    {
                        Gebruiker gebruikerTemp = gebruikersRep.GetGebruikerByName("Bibliothecaris");
                        gebruikerTemp.VeranderPaswoord(instellingenVm.BibliothecarisPaswoord, gebruiker);
                    }
                    gebruikersRep.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }

                return RedirectToAction("Index");
            }
            return View(mvm);
        }
    }
}