using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;

namespace DeKrekelGroup5.Controllers
{
    public class HelperController : Controller
    {
        // GET: Helper
        public ActionResult ClearInfo(Gebruiker gebruiker, MainViewModel mvm)
        {
            mvm.InfoViewModel = null;
            HttpContext.Session["main"] = mvm;
            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
    }
}