using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Infrastructure
{
    public class LetterTuinModelBinder : IModelBinder
    {
        private const string letterTuinSessionKey = "letterTuin";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            LetterTuin letterTuin = controllerContext.HttpContext.Session[letterTuinSessionKey] as LetterTuin;
            if (letterTuin == null)
            {
                letterTuin = new LetterTuin();
                controllerContext.HttpContext.Session[letterTuinSessionKey] = letterTuin;
            }
            return letterTuin;
        }
    }
}
