﻿using System;
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

        public int Leeftijd { get; set; }

        public virtual Thema Themaa { get; set; }

        //public enum Beschikbaar { get; set; }

        //public string Cover { get; set; } <-- wordt naar verwezen als images/Cover.imageFormat OF gwn als url
 
        //public boolean Uitgeleend { get; set; }
    }
}
