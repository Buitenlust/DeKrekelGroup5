using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DeKrekelGroup5.Infrastructure;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.Models.Domain;
using DeKrekelGroup5.ViewModel;
using MySql.Data.Entity;

namespace DeKrekelGroup5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbConfiguration.SetConfiguration(new MySqlEFConfiguration()); //Configuratie voor MySQL
            Database.SetInitializer(new KrekelInitializer());

            ModelBinders.Binders.Add(typeof (Gebruiker), new GebruikerModelBinder());
            ModelBinders.Binders.Add(typeof(MainViewModel), new  MainViewModelBinder());
            ModelBinders.Binders.Add(typeof(Helper), new HelperModelBinder());
        }
    }
}
