using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Infrastructure
{
    public class GebruikerModelBinder : IModelBinder
    {
        private const string gebruikerSessionKey = "gebruiker";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Gebruiker gebruiker = controllerContext.HttpContext.Session[gebruikerSessionKey] as Gebruiker;
            if (gebruiker == null)
            {
                gebruiker = new Gebruiker();

                controllerContext.HttpContext.Session[gebruikerSessionKey] = gebruiker;
            }
            return gebruiker;
        }
    }
}
