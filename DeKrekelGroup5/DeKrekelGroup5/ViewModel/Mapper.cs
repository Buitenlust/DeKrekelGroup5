using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public static class Mapper
    {
        public static BoekCreateViewModel ConvertToBoekCreateViewModel(this Boek boek)
        {
            return new BoekCreateViewModel()
            {
                Exemplaar = boek.Exemplaar,
                Omschrijving = boek.Omschrijving,
                Titel = boek.Titel,
                Auteur = boek.Auteur,
                Uitgever = boek.Uitgever,
                Leeftijd = boek.Leeftijd,
                Thema  =
                    (boek.Themaa == null
                         ? ""
                         : boek.Themaa.Themaa)

            };
        }
    }
}