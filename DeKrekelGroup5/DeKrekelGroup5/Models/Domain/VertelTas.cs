using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Linq;

namespace DeKrekelGroup5.Models.Domain
{
    public class VertelTas
    {
        [Key]
        public int Exemplaar { get; set; }

        public String Titel { get; set; }
        public string Omschrijving { get; set; }
        public string ImageString { get; set; }
        public int Leeftijd { get; set; }

        public bool Beschikbaar { get; set; }

        public virtual List<Thema> Themas { get; set; }

        public virtual List<Uitlening> Uitleningen { get; set; }

        public virtual ICollection<Boek> Boeken { get; set; }
        public virtual ICollection<DVD> DVDs { get; set; }
        public virtual ICollection<CD> CDs { get; set; }
        public virtual ICollection<Spel> Spellen { get; set; } 

            public void Update(VertelTas vertelTas)
        {
            ImageString = vertelTas.ImageString;
            Leeftijd = vertelTas.Leeftijd;
            Omschrijving = vertelTas.Omschrijving;
            Titel = vertelTas.Titel;
            Beschikbaar = vertelTas.Beschikbaar; 
            Uitleningen = vertelTas.Uitleningen;
        }
    }
}
