using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeKrekelGroup5.Models.Domain
{
    public class Boek : Item
    {

        public string Auteur { get; set; }
        public string Uitgever { get; set; }
        [Required(ErrorMessage = "Please enter : Jaar")]
        [Display(Name = "Jaar")]
        [Range(1900, 2100)]
        public int Jaar { get; set; }
        [Required(ErrorMessage = "Please enter : Isbn")]
        [Display(Name = "Isbn")]
        public long isbn { get; set; }

        public void Update(Boek boek)
        {
            base.Omschrijving = boek.Omschrijving;
            base.Themaa = boek.Themaa;
            base.Titel = boek.Titel;
            Auteur = boek.Auteur;
            Uitgever = boek.Uitgever;
            Jaar = boek.Jaar;
            isbn = boek.isbn;

        }
    }
}
