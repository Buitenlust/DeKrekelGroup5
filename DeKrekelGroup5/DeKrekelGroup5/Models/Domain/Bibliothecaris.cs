using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.WebPages;

namespace DeKrekelGroup5.Models.Domain
{
    public class Bibliothecaris:LetterTuin
    {
        public ICollection<Uitlening> Uitleningen { get; set; }
        public int User { get; set; }
        private string password;

        public string Password
        {
            set { password = HashPassword(value); }
            get { return password; }
        }


        /// <summary> Gets all uitleningen that contains a search keyword in Titel, naam or voornaam uitlener or omschrijving </summary>
        /// <returns>Returns a IEnumerable of Spellen or null if not found</returns>
        /// <param name="search"> search keyword </param>
        public IEnumerable<Uitlening> GetUitleningen(string search)
        {
            if (search != null && !search.Trim().IsEmpty())
            return Uitleningen.Where(p => p.Itemm.Titel.ToLower().Contains(search.ToLower()) ||
                                          p.Uitlenerr.Naam.ToLower().Contains(search.ToLower()) ||
                                          p.Uitlenerr.VoorNaam.ToLower().Contains(search.ToLower()) || 
                                          p.Itemm.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.EindDatum);
            return null;
        }

        /// <summary> return an uitlening matching the parameter id.</summary>
        /// <returns>Returns a IEnumerable of Spellen or null if not found</returns>
        /// <param name="id"> search keyword </param>
        public Uitlening GetUitlening(int id)
        {
            if(id > 0)
                return Uitleningen.SingleOrDefault(p => p.Id == id);
            return null;
        }

        /// <summary> Add a new Uitlening to the Collection </summary>
        /// <returns>Returns a boolean true if the uitlening is succesfully created</returns>
        /// <param name="uitlener"> Object Uitlener </param>
        /// <param name="item"> Object item </param>
        public bool VoegUitleningToe(Uitlener uitlener, Item item)
        {
            if (uitlener != null && item != null && uitlener.Id > 0 && item.Exemplaar > 0)
            {
                Uitleningen.Add( new  Uitlening()
                {
                    Itemm = item,
                    StartDatum = DateTime.Today,
                    Verlenging = 0,
                    Uitlenerr = uitlener
                    
                });
                return true;
            }
            return false;
        }

        /// <summary> Modifies the uitlening. adds the enddate to today </summary>
        /// <returns>Returns a boolean true if the uitlening is correctly modified</returns>
        /// <param name="id"> Id of uitlening </param>
        public bool EindeUitlening(int id)
        {
            Uitlening oldUitlening = GetUitlening(id);
            if (oldUitlening != null)
            {
                Uitleningen.Remove(oldUitlening);
                oldUitlening.EindDatum = DateTime.Today;
                Uitleningen.Add(oldUitlening);
                return true;
            }
            return false;
        }

        /// <summary> Calculates the fine after an item is returned </summary>
        /// <returns>Returns the amount to pay.</returns>
        /// <param name="id"> Id of uitlening </param>
        public decimal GetBoete(int id)
        {
            Uitlening uitlening = GetUitlening(id);
            int dagen = uitlening.EindDatum.Subtract(uitlening.StartDatum).Days;
            if ( dagen > Instellingen.UitleenDagen)
                return dagen - Instellingen.UitleenDagen*Instellingen.BedragBoetePerDag;
            return 0;
        }

        /// <summary> Verlengt de uitlening van een uitgeleende item </summary>
        /// <returns>Retourneerd true als dit mogelijk en gewijzigd is</returns>
        /// <param name="id"> Id van uitlening </param>
        public bool VerlengUitlening(int id )
        {
            Uitlening uitlening = GetUitlening(id);
            if (uitlening.Verlenging < Instellingen.MaxVerlengingen)
            {
                Uitleningen.Remove(uitlening);
                uitlening.Verlenging += 1;
                Uitleningen.Add(uitlening);
                return true;
            }
            return false;
        }

        /// <summary> Computes a salted hash of the password and salt provided and returns as a base64 encoded string. </summary>
        /// <param name="pass">The password to hash.</param>
        public string HashPassword(string pass)
        {
            string sHashWithSalt = pass + "";
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            System.Security.Cryptography.HashAlgorithm algorithm = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
        }
    }
}