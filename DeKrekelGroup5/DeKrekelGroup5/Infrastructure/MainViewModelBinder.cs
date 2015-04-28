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
    public class MainViewModelBinder : IModelBinder
    {
        private const string gebruikerSessionKey = "main";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            MainViewModel mvm = controllerContext.HttpContext.Session[gebruikerSessionKey] as MainViewModel;
            if (mvm != null)
            { 
                controllerContext.HttpContext.Session[gebruikerSessionKey] = mvm;
                return mvm;
            }
            else
            {
                return new MainViewModel();
            }
            
        }
    }
}
 