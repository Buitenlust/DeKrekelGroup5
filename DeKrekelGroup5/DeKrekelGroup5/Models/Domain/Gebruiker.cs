using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string GebruikersNaam { get; set; }
        public string PaswoordHashed { get; private set; }
        public bool AdminRechten { get; set; }

        public Gebruiker(int id, string gebruikersNaam, string paswoord, bool adminRechten)
        {
            Id = id;
            GebruikersNaam = gebruikersNaam;
            PaswoordHashed = HashPassword(paswoord);
            AdminRechten = adminRechten;
        }

        public Gebruiker()
        { 
        }

        /// <summary> Past het paswoord van de beheerder aan.</summary> 
        /// <param name="paswoord"> nieuw paswoord </param>
        public void VeranderPaswoord(string paswoord)
        {
            string hashed = HashPassword(paswoord);
            PaswoordHashed = hashed;
        }

        /// <summary> Past het paswoord van de beheerder aan.</summary> 
        /// <param name="paswoord"> nieuw paswoord </param>
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