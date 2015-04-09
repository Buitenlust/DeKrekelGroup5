using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class UitlenerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vul een naam in aub...")]
        [MaxLength(45, ErrorMessage = "naam is te lang (max. 45 tekens)")]
        public String Naam { get; set; }
        [Required(ErrorMessage = "Vul een voornaam in aub...")]
        [MaxLength(12, ErrorMessage = "Voornaam is te lang (max. 12 tekens)")]
        public String VoorNaam { get; set; }

        [MaxLength(8, ErrorMessage = "Klas is te lang (max. 8 tekens)")]
        public String Klas { get; set; }
        [MaxLength(20, ErrorMessage = "Adres is te lang (max. 20 tekens)")]
        public String Adres { get; set; }
        [EmailAddress]
        public String Email { get; set; }


        public UitlenerViewModel()
        {
        }

        public UitlenerViewModel(Uitlener uitlener)
        {
            Id = uitlener.Id;
            Naam = uitlener.Naam;
            VoorNaam = uitlener.VoorNaam;
            Klas = uitlener.Klas;
            Adres = uitlener.Adres;
            Email = uitlener.Email;
        }

        public Uitlener MapNaarUitlener(UitlenerViewModel vm)
        {
            return new Uitlener() { Adres = vm.Adres, Email = vm.Email, Klas = vm.Klas, Naam = vm.Naam, VoorNaam = vm.VoorNaam, Id = vm.Id };
        }

    }

    public class UitlenersLijstViewModel
    {
        public IEnumerable<UitlenerViewModel> Uitleners { get; set; }

        public UitlenersLijstViewModel(IEnumerable<Uitlener> uitleners)
        {
            Uitleners = uitleners.Select(b => new UitlenerViewModel(){
                Id = b.Id,
                Naam = b.Naam,
                VoorNaam = b.VoorNaam,
                Klas = b.Klas,
                Adres = b.Adres, 
                Email = b.Email
            });
        }
    }
}