using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeKrekelGroup5.Models.Domain
{
    public class Thema
    {
        [Key]
        public int IdThema { get; set; }
        [Required(ErrorMessage = "Vul een thema in aub...")]
        [Display(Name = "Thema")]
        public String Themaa { get; set; } 
    }
}
