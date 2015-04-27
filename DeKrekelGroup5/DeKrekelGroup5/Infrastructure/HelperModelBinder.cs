using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;

namespace DeKrekelGroup5.Infrastructure
{
    public class HelperModelBinder : IModelBinder
    {
        private const string helperSessionKey = "helper";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Helper helper = controllerContext.HttpContext.Session[helperSessionKey] as Helper;
            if (helper != null)
            {
                controllerContext.HttpContext.Session[helperSessionKey] = helper;
                return helper;
            }
            return new Helper();
        }
    }
}
