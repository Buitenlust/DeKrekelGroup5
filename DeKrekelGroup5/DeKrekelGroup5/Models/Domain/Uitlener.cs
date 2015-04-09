using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Uitlener
    {
        public int Id { get; set; }
        public String Naam { get; set; }
        public String VoorNaam { get; set; }
        public String Klas { get; set; }
        public String Adres { get; set; }
        public String Email { get; set; }

        public void Update(Uitlener uitlener)
        {
            Naam = uitlener.Naam;
            VoorNaam = uitlener.VoorNaam;
            Klas = uitlener.Klas;
            Adres = uitlener.Adres;
            Email = uitlener.Email;
        }
         
    }
}
