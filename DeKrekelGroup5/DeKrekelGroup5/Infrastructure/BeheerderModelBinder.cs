using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Infrastructure
{
    public class BeheerderModelBinder:IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            if (controllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                IBeheerderRepository repos = (IBeheerderRepository)DependencyResolver.Current.GetService(typeof(IBeheerderRepository));
                return repos.FindBeheerder(controllerContext.HttpContext.User.Identity.Name);
            }
            return null;
        }
    }
}