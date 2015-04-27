using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;

namespace DeKrekelGroup5.Models.Domain
{
   
    public class Item
    {
        [Key]
        public int Exemplaar { get; set; }

        public string Titel { get; set; }
        public string ImageString { get; set; }
        public string Omschrijving { get; set; }

        public int Leeftijd { get; set; }

        public bool Beschikbaar { get; set; }

        public virtual Thema Themaa { get; set; }

        public virtual List<Uitlening> Uitleningen { get; set; }


        public Item()
        { 
        }

        public void Update(Item item)
        {
            ImageString = item.ImageString;
            Leeftijd = item.Leeftijd;
            Omschrijving = item.Omschrijving;
            Titel = item.Titel;
            Beschikbaar = item.Beschikbaar;
            Themaa = item.Themaa;
            Uitleningen = item.Uitleningen;
        }
    }
}
