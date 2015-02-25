using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeKrekelGroup5.Models.Domain
{
   
    public class Item
    {
        [Key]
        public int Exemplaar { get; set; }

        [Required(ErrorMessage = "Vul een titel in aub...")]
        public string Titel { get; set; }

        [Display(Name = "Omschrijving")]
        [MaxLength(1023)]
        public string Omschrijving { get; set; }

        [Display(Name = "Thema")]
        [Required(ErrorMessage = "Kies een Thema aub...")]
        [MaxLength(45)]
        public Thema Themaa { get; set; }

    }
}
