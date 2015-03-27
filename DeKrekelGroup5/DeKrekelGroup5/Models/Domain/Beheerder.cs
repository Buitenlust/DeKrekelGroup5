using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.ViewModel;

namespace DeKrekelGroup5.Models.Domain
{
    public class Beheerder : Bibliothecaris
    {
        /// <summary> Adds an item to the collection </summary>
        /// <returns>returns true if item is succesfully added to the collection. Exemplaar must be = 0 </returns>
        /// <param name="item"> toe te voegen Item </param>
        public bool AddItem(Item item)
        {
            if (item != null && item.Exemplaar == 0)
            {
                Items.Add(item);
                return true;
            }
            return false;
        }

        /// <summary> Get the first boek that matches the item id  parameter </summary>
        /// <returns>returns true if item is succesfully added to the list. Exemplaar must be > 0 </returns>
        /// <param name="item"> te updaten Item </param>
        public bool UpdateItem(Item item)
        {
            if (item != null && item.Exemplaar > 0)
            {
                Items.Remove(GetItem(item.Exemplaar));
                Items.Add(item);
                return true;
            }
            return false;
        }

        /// <summary> Removes an item from the collection </summary>
        /// <returns>returns true if item is succesfully removed from the collection. Exemplaar must be > 0 </returns>
        /// <param name="id"> id van te verwijderen item </param>
        public bool RemoveItem(int id)
        {
            Item item = GetItem(id);
            if (item != null && item.Exemplaar > 0)
            {
                Items.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary> Past de boete per dag aan met nieuwe instelling</summary> 
        /// <param name="bedrag"> nieuw bedrag in euro </param>
        public void PasBoeteAan(int bedrag)
        {
            if (bedrag >= 0) 
            Instellingen.BedragBoetePerDag = bedrag;
        }

        /// <summary> Past de maximum aantal toegestane Verlengingen aan</summary> 
        /// <param name="aantal"> nieuwe aantal verlengingen </param>
        public void PasMaxVerlengingen(int aantal)
        {
            if (aantal <= 3 && aantal >= 0)
            Instellingen.MaxVerlengingen = aantal;
        }

        /// <summary> Past de max aantal toegestane uitleendagen aan</summary> 
        /// <param name="dagen"> nieuwe aantal dagen </param>
        public void PasMaxuitleenDagenAan(int dagen)
        {
            if (dagen > 7)
            Instellingen.UitleenDagen = dagen;
        }


        /// <summary> Past het paswoord van de beheerder aan.</summary> 
        /// <param name="paswoord"> nieuw paswoord </param>
        public void VeranderPaswoorBeheerder(string paswoord)
        {
            string hashed = HashPassword(paswoord);
            this.Password = hashed;
        }

    }
}