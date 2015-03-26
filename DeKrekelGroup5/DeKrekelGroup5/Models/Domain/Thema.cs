using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeKrekelGroup5.Models.Domain
{
    public class Thema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdThema { get; set; }
        [Required(ErrorMessage = "Vul een thema in aub...")]
        [Display(Name = "Thema")]
        public String Themaa { get; set; }


        public void Update(string thema)
        {
            Themaa = thema;
        }
    }
}
