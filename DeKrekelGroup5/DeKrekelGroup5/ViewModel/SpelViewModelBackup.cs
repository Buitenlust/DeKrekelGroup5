using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class SpelViewModel:ItemViewModel
    { 
        [Required(ErrorMessage = "Geef een uitgever in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de uitgever is te lang (max. 45 tekens)")]
        public string Uitgever { get; set; }

        public SpelViewModel(Spel spel) : base(spel)
        {
            Uitgever = spel.Uitgever;
        }
    }
    public class SpellenViewModel
    {
        public IEnumerable<SpelViewModel> Spellen { get; set; }

        public SpellenViewModel(IEnumerable<Spel> spellen)
        {
            Spellen = spellen.Select(s => new SpelViewModel(s));
        }
    }
}