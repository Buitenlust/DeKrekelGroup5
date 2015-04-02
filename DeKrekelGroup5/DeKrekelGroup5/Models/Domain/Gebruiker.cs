﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.WebPages;

namespace DeKrekelGroup5.Models.Domain
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string GebruikersNaam { get; set; }
        public string PaswoordHashed { get; private set; }
        public bool AdminRechten { get; set; }
        public bool BibliotheekRechten { get; set; }
        public LetterTuin LetterTuin { get; set; }
        
        public Gebruiker(int id, string gebruikersNaam, string paswoord, bool adminRechten, bool bibRechten, LetterTuin letterTuin)
        {
            Id = id;
            GebruikersNaam = gebruikersNaam;
            PaswoordHashed = HashPassword(paswoord);
            AdminRechten = adminRechten;
            BibliotheekRechten = bibRechten;
            LetterTuin = letterTuin;
        }

        public Gebruiker()
        { 
            if(LetterTuin == null)
                LetterTuin = new LetterTuin();
        }

 

        /// <summary> Past het paswoord van de beheerder aan.</summary> 
        /// <param name="paswoord"> nieuw paswoord </param>
        public void VeranderPaswoord(string paswoord)
        {
            CheckAdminRechten();
            string hashed = HashPassword(paswoord);
            PaswoordHashed = hashed;
        }

        /// <summary> Past het paswoord van de beheerder aan.</summary> 
        /// <param name="paswoord"> nieuw paswoord </param>
        public string HashPassword(string paswoord)
        {
            string sHashWithSalt = paswoord + "";
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            System.Security.Cryptography.HashAlgorithm algorithm = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
        }

        #region ADMIN

        /// <summary> Adds an item to the collection </summary>
        /// <returns>returns true if item is succesfully added to the collection. Exemplaar must be = 0 </returns>
        /// <param name="item"> toe te voegen Item </param>
        public bool AddItem(Item item)
        {
            CheckAdminRechten();
                if (item != null && item.Exemplaar == 0)
                {
                    LetterTuin.Items.Add(item);
                    return true;
                }
                return false;
        }
 
        /// <summary> Update de parameters van een boek</summary>
        /// <returns> Geeft true weer als het boek is aangepast. Exemplaar van het boek moet > 0 </returns>
        /// <param name="boek"> te updaten boek </param>
        public bool UpdateBoek(Boek boek)
        {
            CheckAdminRechten();
            if (boek != null && boek.Exemplaar > 0)
            {
                var newItem = LetterTuin.GetItem(boek.Exemplaar) as Boek;
                if (newItem != null)
                {
                    newItem.Update(boek);
                    return true;
                }
            }
            return false;
        }

        /// <summary> Removes an item from the collection </summary>
        /// <returns>returns true if item is succesfully removed from the collection. Exemplaar must be > 0 </returns>
        /// <param name="id"> id van te verwijderen item </param>
        public bool RemoveItem(int id)
        {
            CheckAdminRechten();
            Item item = LetterTuin.GetItem(id);
            if (item != null && item.Exemplaar > 0)
            {
                LetterTuin.Items.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary> Past de boete per dag aan met nieuwe instelling</summary> 
        /// <param name="bedrag"> nieuw bedrag in euro </param>
        public void PasBoeteAan(int bedrag)
        {
            CheckAdminRechten();
            if (bedrag >= 0)
                LetterTuin.Instellingen.BedragBoetePerDag = bedrag;
        }

        /// <summary> Past de maximum aantal toegestane Verlengingen aan</summary> 
        /// <param name="aantal"> nieuwe aantal verlengingen </param>
        public void PasMaxVerlengingen(int aantal)
        {
            CheckAdminRechten();
            if (aantal <= 3 && aantal >= 0)
                LetterTuin.Instellingen.MaxVerlengingen = aantal;
        }

        /// <summary> Past de max aantal toegestane uitleendagen aan</summary> 
        /// <param name="dagen"> nieuwe aantal dagen </param>
        public void PasMaxuitleenDagenAan(int dagen)
        {
            CheckAdminRechten();
            if (dagen > 7)
                LetterTuin.Instellingen.UitleenDagen = dagen;
        }


        /// <summary> voegt een Thema toe aan de lijst </summary>
        /// <returns>Geeft true weer als het thema werd aangemaakt. </returns>
        /// <param name="thema"> toe te voegen Thema</param>
        public bool AddThema(Thema thema)
        {
            CheckAdminRechten();
            if (thema != null)
            {
                LetterTuin.Themas.Add(thema);
                return true;
            }
            return false;
        }


        /// <summary> Update de parameters van een boek</summary>
        /// <returns> Geeft true weer als het boek is aangepast. Exemplaar van het boek moet > 0 </returns>
        /// <param name="thema"> te updaten boek </param>
        public bool UpdateThema(Thema thema)
        {
            CheckAdminRechten();
            if (thema != null && thema.IdThema > 0)
            {
                var th = LetterTuin.GetThemaById(thema.IdThema);
                if (th != null)
                {
                    th.Update(thema.Themaa);
                    return true;
                }
            }
            return false;
        }

        /// <summary> verwijdert een thema van de lijst </summary>
        /// <returns>Geeft true weer als het thema werd verwijderd. </returns>
        /// <param name="thema"> toe te voegen Thema</param>
        public bool RemoveThema(Thema thema)
        {
            CheckAdminRechten();
            Thema th = LetterTuin.GetThemaById(thema.IdThema);
            if (th != null && thema.IdThema > 0)
            {
                LetterTuin.Themas.Remove(th);
                return true;
            }
            return false;
        }
        #endregion

        #region BIBLIOTHECARIS
        /// <summary> Gets all uitleningen that contains a search keyword in Titel, naam or voornaam uitlener or omschrijving </summary>
        /// <returns>Returns a IEnumerable of Spellen or null if not found</returns>
        /// <param name="search"> search keyword </param>
        public IEnumerable<Uitlening> GetUitleningen(string search)
        {
            CheckBibliotheekRechten();
            if (search != null && !search.Trim().IsEmpty())
                return LetterTuin.Uitleningen.Where(p => p.Itemm.Titel.ToLower().Contains(search.ToLower()) ||
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
            CheckBibliotheekRechten();
            if (id > 0)
                return LetterTuin.Uitleningen.SingleOrDefault(p => p.Id == id);
            return null;
        }

        /// <summary> Add a new Uitlening to the Collection </summary>
        /// <returns>Returns a boolean true if the uitlening is succesfully created</returns>
        /// <param name="uitlener"> Object Uitlener </param>
        /// <param name="item"> Object item </param>
        public bool VoegUitleningToe(Gebruiker gebruiker, Uitlener uitlener, Item item)
        {
            CheckBibliotheekRechten();
            if (uitlener != null && item != null && uitlener.Id > 0 && item.Exemplaar > 0)
            {
                LetterTuin.Uitleningen.Add(new Uitlening()
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
        public bool EindeUitlening(Gebruiker gebruiker, int id)
        {
            CheckBibliotheekRechten();
            Uitlening oldUitlening = GetUitlening(id);
            if (oldUitlening != null)
            {
                LetterTuin.Uitleningen.Remove(oldUitlening);
                oldUitlening.EindDatum = DateTime.Today;
                LetterTuin.Uitleningen.Add(oldUitlening);
                return true;
            }
            return false;
        }

        /// <summary> Calculates the fine after an item is returned </summary>
        /// <returns>Returns the amount to pay.</returns>
        /// <param name="id"> Id of uitlening </param>
        public decimal GetBoete(Gebruiker gebruiker, int id)
        {
            CheckBibliotheekRechten();
            Uitlening uitlening = GetUitlening(id);
            int dagen = uitlening.EindDatum.Subtract(uitlening.StartDatum).Days;
            if (dagen > LetterTuin.Instellingen.UitleenDagen)
                return dagen - LetterTuin.Instellingen.UitleenDagen * LetterTuin.Instellingen.BedragBoetePerDag;
            return 0;
        }

        /// <summary> Verlengt de uitlening van een uitgeleende item </summary>
        /// <returns>Retourneerd true als dit mogelijk en gewijzigd is</returns>
        /// <param name="id"> Id van uitlening </param>
        public bool VerlengUitlening(Gebruiker gebruiker, int id)
        {
            CheckBibliotheekRechten();
            Uitlening uitlening = GetUitlening(id);
            if (uitlening.Verlenging < LetterTuin.Instellingen.MaxVerlengingen)
            {
                LetterTuin.Uitleningen.Remove(uitlening);
                uitlening.Verlenging += 1;
                LetterTuin.Uitleningen.Add(uitlening);
                return true;
            }
            return false;
        }
        #endregion

        /// <summary> Controleert de gebruiker zijn adminrechten </summary>
        /// <exception>Throws Accessviolationexception indien gebruiker geen rechten heeft. </exception>
        public void CheckAdminRechten()
        {
            if (!AdminRechten)
                throw new AccessViolationException();
        }

        /// <summary> Controleert de gebruiker zijn bibliotheekrechten </summary>
        /// <exception>Throws Accessviolationexception indien gebruiker geen rechten heeft. </exception>
        public void CheckBibliotheekRechten()
        {
            if (!BibliotheekRechten)
                throw new AccessViolationException();
        }

    }
}