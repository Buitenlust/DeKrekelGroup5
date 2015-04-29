using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class ThemaViewModel
    {
        public int IdThema { get; set; }
        [Required(ErrorMessage = "Vul een thema in aub...")]
        [Display(Name = "Thema")]
        public String Themaa { get; set; }


        public ThemaViewModel()
        {
        }

        public ThemaViewModel(string thema)
        {
            Themaa = thema;
        }

        public ThemaViewModel(Thema thema)
        {
            IdThema = thema.IdThema;
            Themaa = thema.Themaa;
        }
    }
 
}