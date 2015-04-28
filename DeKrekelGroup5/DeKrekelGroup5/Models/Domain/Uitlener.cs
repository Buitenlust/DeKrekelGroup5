using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using LINQtoCSV;

namespace DeKrekelGroup5.Models.Domain
{
    public class Uitlener
    { 
        public int Id { get; set; }
        [CsvColumn(FieldIndex = 1)]
        public String Naam { get; set; }
        [CsvColumn(Name = "Voornaam", FieldIndex = 2)]
        public String VoorNaam { get; set; }
        [CsvColumn(FieldIndex = 5)]
        public String Klas { get; set; }
        [CsvColumn(Name = "Domicilie-adres", FieldIndex = 3)]
        public String Adres { get; set; }
        [CsvColumn(Name = "E-mail adres (domicilie)", FieldIndex = 4)]
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
