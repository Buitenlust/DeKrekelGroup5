using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class ItemViewModel
    {
        public int Exemplaar { get; set; }
        [Required(ErrorMessage = "Geef een titel in aub...")]
        [MaxLength(45, ErrorMessage = "De titel is te lang (max. 55 tekens)")]
        public string Titel { get; set; }
        [MaxLength(1000, ErrorMessage = "De omschrijving is te lang (max. 1000 tekens)")]
        public string Omschrijving { get; set; }
        [Required(ErrorMessage = "Geef een jaartal tussen 0 & 99 in aub...")]
        [Range(0, 99)]
        public int Leeftijd { get; set; }
        [Required(ErrorMessage = "Kies een thema aub...")]
        public string Thema { get; set; }

        //Boek:
        [Required(ErrorMessage = "Geef een auteur in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de auteur is te lang (max. 45 tekens)")]
        public string Auteur { get; set; }

        //Boek of Spel:
        [Required(ErrorMessage = "Geef een uitgever in aub...")]
        [MaxLength(45, ErrorMessage = "De naam van de uitgever is te lang (max. 45 tekens)")]
        public string Uitgever { get; set; }

        public string ItemType { get; set; }

        public ItemViewModel(Item item)
        {
            Exemplaar = item.Exemplaar;
            Titel = item.Titel;
            Omschrijving = item.Omschrijving;
            Leeftijd = item.Leeftijd;
            Thema = item.Themaa.Themaa;
            
            //Boek
            Boek boek = item as Boek;
            if (boek != null)
            {
                Auteur = boek.Auteur;
                Uitgever = boek.Uitgever;
                ItemType = "boek";
            }

            Spel spel = item as Spel;
            if (spel != null)
            {
                Uitgever = spel.Uitgever;
                ItemType = "spel";
            }
        }

        public ItemViewModel()
        {
            
        }
    }
    public class CreateItem
    {
        public SelectList Themas { get; set; }
        public ItemViewModel ItemVm { get; set; }

        public CreateItem(IEnumerable<Thema> themas, Item item)
        {

            Themas = new SelectList(themas, "Themaa", "Themaa", "");
            ItemVm = new ItemViewModel(item);
        }
    }
}