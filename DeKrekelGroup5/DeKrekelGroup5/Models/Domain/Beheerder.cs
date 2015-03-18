using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.ViewModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Beheerder:LetterTuin
    {
        public Beheerder(IItemRepository iitemRepository, IThemasRepository themasRepository) : base(iitemRepository, themasRepository)
        {
        }

        public Item AddItem(ItemViewModel itemView, Item item)
        {
            item.Exemplaar = itemView.Exemplaar;
            
            item.Leeftijd = itemView.Leeftijd;
            item.Omschrijving = itemView.Omschrijving;
            item.Titel = itemView.Titel;
            item.Themaa = (String.IsNullOrEmpty(itemView.Thema) ? null : tr.FindBy(itemView.Thema));
            Boek newBoek = item as Boek;
            if (newBoek != null)
            {
                newBoek.Auteur = itemView.Auteur;
                newBoek.Uitgever = itemView.Uitgever;
                ii.Add(newBoek);
                ii.ModifiedThema(newBoek);
            }
            Spel newSpel = item as Spel;
            if (newSpel != null)
            {
                newSpel.Uitgever = itemView.Uitgever;
                ii.Add(newSpel);
                ii.ModifiedThema(newSpel);
            }
            ii.SaveChanges();

            return ii.FindLastItem();
        }
        
        public void AddBoek(ItemViewModel item)
        {
            Item newBoek = new Boek
            {
                Exemplaar = item.Exemplaar,
                Auteur = item.Auteur,
                Leeftijd = item.Leeftijd,
                Omschrijving = item.Omschrijving,
                Titel = item.Titel,
                Uitgever = item.Uitgever,
                Themaa = (String.IsNullOrEmpty(item.Thema) ? null : tr.FindBy(item.Thema))
            };

            ii.Add(newBoek);
            ii.ModifiedThema(newBoek);
        }
        public void AddSpel(ItemViewModel item)
        {
            Item newSpel = new Spel
            {
                Exemplaar = item.Exemplaar,
                Leeftijd = item.Leeftijd,
                Omschrijving = item.Omschrijving,
                Titel = item.Titel,
                Uitgever = item.Uitgever,
                Themaa = (String.IsNullOrEmpty(item.Thema) ? null : tr.FindBy(item.Thema))
            };

            ii.Add(newSpel);
            ii.ModifiedThema(newSpel);
        }

        public void SaveChanges()
        {
            ii.SaveChanges();
        }
    }
}