using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class ItemViewModel
    {
        public int Exemplaar { get; set; }
        [Required(ErrorMessage = "Vul een titel in aub...")]
        [MaxLength(45, ErrorMessage = "De titel is te lang (max. 45 tekens)")]
        public string Titel { get; set; }
        [MaxLength(1000, ErrorMessage = "De omschrijving is te lang (max. 1000 tekens)")]
        public string Omschrijving { get; set; }
        [Required(ErrorMessage = "Vul een jaartal tussen 0 & 99 in aub...")]
        [Range(0, 99)]
        public int Leeftijd { get; set; }
        [Required(ErrorMessage = "Kies een thema aub...")]
        public string Thema { get; set; }

        public bool Beschikbaar { get; set; }
        public bool Uitgeleend { get; set; }

        public ItemViewModel()
        {
            
        }
    }
}