using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class ItemViewModel
    {
        public int Exemplaar { get; set; }
        [Required(ErrorMessage = "Vul een titel in aub...")]
        [MaxLength(200, ErrorMessage = "De titel is te lang (max. 200 tekens)")]
        public string Titel { get; set; }
        [MaxLength(4000, ErrorMessage = "De omschrijving is te lang (max. 4000 tekens)")]
        public string Omschrijving { get; set; }
        [Required(ErrorMessage = "Vul een jaartal tussen 0 & 99 in aub...")]
        [Range(0, 99, ErrorMessage = "Vul een jaartal tussen 0 & 99 in aub...")]
        public int Leeftijd { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        //Required(ErrorMessage = "Kies een thema aub...")]
        public List<Thema> Themas { get; set; }

        public bool Beschikbaar { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]

        public DateTime EindDatumUitlening { get; set; }
        public bool Uitgeleend { get; set; }
        public string image { get; set; }
        public ItemViewModel()
        {
            
        }

        public String GetStringDate()
        {
            return EindDatumUitlening.Day + " / " + EindDatumUitlening.Month + " / " + EindDatumUitlening.Year;
        }
    }
}