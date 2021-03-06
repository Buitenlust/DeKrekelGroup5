﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.WebPages;
using DeKrekelGroup5.ViewModel;

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
        
        public Gebruiker(string gebruikersNaam, string paswoord, bool adminRechten, bool bibRechten, LetterTuin letterTuin)
        { 
            GebruikersNaam = gebruikersNaam;
            PaswoordHashed = HashPassword(paswoord);
            AdminRechten = adminRechten;
            BibliotheekRechten = bibRechten;
            LetterTuin = letterTuin;
        }
        
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
        /// <param name="beheerder"> beheerder moet meegegeven worden om rechten te controleren </param>
        public void VeranderPaswoord(string paswoord, Gebruiker beheerder=null)
        {
            if(beheerder!=null)
                beheerder.CheckAdminRechten();
            else
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
            HashAlgorithm algorithm = new SHA256Managed();
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

        /// <summary> Adds a spel to the collection </summary>
        /// <returns>returns true if item is succesfully added to the collection. Exemplaar must be = 0 </returns>
        /// <param name="item"> toe te voegen Item </param>
        public bool AddBoek(Boek boek)
        {
            CheckAdminRechten();
            if (boek != null && boek.Exemplaar == 0)
            {
                LetterTuin.Boeken.Add(boek);
                return true;
            }
            return false;
        }

        /// <summary> Adds a cd to the collection </summary>
        /// <returns>returns true if item is succesfully added to the collection. Exemplaar must be = 0 </returns>
        /// <param name="item"> toe te voegen Item </param>
        public bool AddCD(CD cd)
        {
            CheckAdminRechten();
            if (cd != null && cd.Exemplaar == 0)
            {
                LetterTuin.Cds.Add(cd);
                return true;
            }
            return false;
        }

        /// <summary> Adds a dvd to the collection </summary>
        /// <returns>returns true if item is succesfully added to the collection. Exemplaar must be = 0 </returns>
        /// <param name="item"> toe te voegen Item </param>
        public bool AddDVD(DVD dvd)
        {
            CheckAdminRechten();
            if (dvd != null && dvd.Exemplaar == 0)
            {
                LetterTuin.Dvds.Add(dvd);
                return true;
            }
            return false;
        }

        /// <summary> Adds a spel to the collection </summary>
        /// <returns>returns true if item is succesfully added to the collection. Exemplaar must be = 0 </returns>
        /// <param name="item"> toe te voegen Item </param>
        public bool AddSpel(Spel spel)
        {
            CheckAdminRechten();
            if (spel != null && spel.Exemplaar == 0)
            {
                LetterTuin.Spellen.Add(spel);
                return true;
            }
            return false;
        }
 
 
        public bool AddVertelTas(VertelTas vertelTas)
        {
            CheckAdminRechten();
            if (vertelTas != null && vertelTas.Exemplaar == 0)
            {
                LetterTuin.VertelTassen.Add(vertelTas);
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
                var newItem = LetterTuin.GetBoek(boek.Exemplaar);
                if (newItem != null)
                {
                    newItem.Update(boek);
                    newItem.Themas.Clear();
                    foreach (Thema thema in boek.Themas)
                    {
                        newItem.Themas.Add(thema);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary> Update de parameters van een boek</summary>
        /// <returns> Geeft true weer als het boek is aangepast. Exemplaar van het boek moet > 0 </returns>
        /// <param name="cd"> te updaten boek </param>
        public bool UpdateCD(CD cd)
        {
            CheckAdminRechten();
            if (cd != null && cd.Exemplaar > 0)
            {
                var newItem = LetterTuin.GetCD(cd.Exemplaar);
                if (newItem != null)
                {
                    newItem.Update(cd);
                    newItem.Themas.Clear();
                    foreach (Thema thema in cd.Themas)
                    {
                        newItem.Themas.Add(thema);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary> Update de parameters van een dvd</summary>
        /// <returns> Geeft true weer als de dvd is aangepast. Exemplaar van de dvd moet > 0 </returns>
        /// <param name="dvd"> te updaten dvd </param>
        public bool UpdateDVD(DVD dvd)
        {
            CheckAdminRechten();
            if (dvd != null && dvd.Exemplaar > 0)
            {
                var newItem = LetterTuin.GetDVD(dvd.Exemplaar);
                if (newItem != null)
                {
                    newItem.Update(dvd);
                    newItem.Themas.Clear();
                    foreach (Thema thema in dvd.Themas)
                    {
                        newItem.Themas.Add(thema);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool UpdateVertelTas(VertelTas vertelTas)
        {
            CheckAdminRechten();
            if (vertelTas != null && vertelTas.Exemplaar > 0)
            {
                var newVertelTas = LetterTuin.GetVertelTas(vertelTas.Exemplaar);
                if (newVertelTas != null)
                {
                    newVertelTas.Update(vertelTas);
                    newVertelTas.Themas.Clear();
                    newVertelTas.Boeken.Clear();
                    newVertelTas.DVDs.Clear();
                    newVertelTas.CDs.Clear();
                    newVertelTas.Spellen.Clear();
                    foreach (Thema thema in vertelTas.Themas)
                    {
                        newVertelTas.Themas.Add(thema);
                    }
                    foreach (Boek boek in vertelTas.Boeken)
                    {
                        newVertelTas.Boeken.Add(boek);
                    }
                    foreach (DVD dvd in vertelTas.DVDs)
                    {
                        newVertelTas.DVDs.Add(dvd);
                    }
                    foreach (CD cd in vertelTas.CDs)
                    {
                        newVertelTas.CDs.Add(cd);
                    }
                    foreach (Spel spel in vertelTas.Spellen)
                    {
                        newVertelTas.Spellen.Add(spel);
                    }



                    return true;
                }
            }
            return false;
        }


        /// <summary> Update de parameters van een spel</summary>
        /// <returns> Geeft true weer als het spel is aangepast. Exemplaar van het spel moet > 0 </returns>
        /// <param name="spel"> te updaten spel </param>
        public bool UpdateSpel(Spel spel)
        {
            CheckAdminRechten();
            if (spel != null && spel.Exemplaar > 0)
            {
                var newItem = LetterTuin.GetSpel(spel.Exemplaar);
                if (newItem != null)
                {
                    newItem.Update(spel);
                    return true;
                }
            }
            return false;
        }

        /// <summary> Removes a spel from the collection </summary>
        /// <returns>returns true if spel is succesfully removed from the collection. Exemplaar must be > 0 </returns>
        /// <param name="id"> id van te verwijderen item </param>
        public bool RemoveSpel(Spel spel)
        {
            CheckAdminRechten();
            if (spel != null && spel.Exemplaar > 0)
            {
                LetterTuin.Spellen.Remove(spel);
                return true;
            }
            return false;
        }
        /// <summary> Removes a boek from the collection </summary>
        /// <returns>returns true if boek is succesfully removed from the collection. Exemplaar must be > 0 </returns>
        /// <param name="id"> id van te verwijderen item </param>
        public bool RemoveBoek(Boek boek)
        {
            CheckAdminRechten();
            if (boek != null && boek.Exemplaar > 0)
            {
                LetterTuin.Boeken.Remove(boek);
                return true;
            }
            return false;
        }

        /// <summary> Removes a cd from the collection </summary>
        /// <returns>returns true if cd is succesfully removed from the collection. Exemplaar must be > 0 </returns>
        /// <param name="id"> id van te verwijderen item </param>
        public bool RemoveCD(CD cd)
        {
            CheckAdminRechten();
            if (cd != null && cd.Exemplaar > 0)
            {
                LetterTuin.Cds.Remove(cd);
                return true;
            }
            return false;
        }

        /// <summary> Removes an dvd from the collection </summary>
        /// <returns>returns true if dvd is succesfully removed from the collection. Exemplaar must be > 0 </returns>
        /// <param name="id"> id van te verwijderen item </param>
        public bool RemoveDVD(DVD dvd)
        {
            CheckAdminRechten();
            if (dvd != null && dvd.Exemplaar > 0)
            {
                LetterTuin.Dvds.Remove(dvd);
                return true;
            }
            return false;
        }

        public bool RemoveVertelTas(VertelTas vertelTas)
        {
            CheckAdminRechten();
            if (vertelTas != null && vertelTas.Exemplaar > 0)
            {
                LetterTuin.VertelTassen.Remove(vertelTas);
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
                Thema dbthema = LetterTuin.GetThemas(thema.Themaa).FirstOrDefault();
                if (dbthema == null) { 
                    LetterTuin.Themas.Add(thema);
                return true;
                }
            }
            return false;
        }


        /// <summary> Update de parameters van een boek</summary>
        /// <returns> Geeft true weer als het boek is aangepast. Exemplaar van het boek moet > 0 </returns>
        /// <param name="thema"> te updaten boek </param>
        public bool UpdateThema(ThemaViewModel thema)
        {
            CheckAdminRechten();
            if (thema != null && thema.IdThema > 0)
            {
                var th = LetterTuin.GetThemaById(thema.IdThema);
                if (th != null)
                {
                    th.Themaa = thema.Themaa;
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

        /// <summary> return an uitlening matching the parameter id.</summary>
        /// <returns>Returns a IEnumerable of Spellen or null if not found</returns>
        /// <param name="id"> id of an item </param> 
        public Uitlening GetOpenUitleningByItem(int id)
        {
            CheckBibliotheekRechten();
            if (id > 0)
                return LetterTuin.Uitleningen.Where(p => p.Itemm.Exemplaar == id).SingleOrDefault(p => p.BinnenGebracht.Year == 1);
            return null;
        }

        /// <summary> Add a new Uitlening to the Collection </summary>
        /// <returns>Returns a boolean true if the uitlening is succesfully created</returns>
        /// <param name="uitlener"> Object Uitlener </param>
        /// <param name="item"> Object item </param>
        public bool VoegUitleningToe(Uitlener uitlener, Item item)
        {
            CheckBibliotheekRechten();
            if (uitlener != null && item != null && uitlener.Id > 0 && item.Exemplaar > 0)
            {
                LetterTuin.Uitleningen.Add(new Uitlening()
                {
                    Itemm = item,
                    StartDatum = DateTime.Today,
                    EindDatum = DateTime.Today.AddDays(LetterTuin.Instellingen.UitleenDagen),
                    Verlenging = 0,
                    Uitlenerr = uitlener

                });
                return true;
            }
            return false;
        }

        /// <summary> Modifies the uitlening. adds the enddate to today </summary>
        /// <returns>Returns a boolean true if the uitlening is correctly modified</returns>
        /// <param name="uitlening">uitlening </param>
        public void EindeUitlening(Uitlening uitlening)
        {
            CheckBibliotheekRechten();
            if (uitlening != null)
            {
                uitlening.BinnenGebracht = DateTime.Today;
                uitlening.update(uitlening);
            }
        }

        /// <summary> Calculates the fine after an item is returned </summary>
        /// <returns>Returns the amount to pay.</returns>
        /// <param name="uitlening"> de uitlening </param>
        public decimal GetBoete(Uitlening uitlening)
        {
            CheckBibliotheekRechten();
            int dagen = DateTime.Today.Subtract(uitlening.EindDatum).Days;
            if (dagen > 0)
                return (dagen * (decimal)LetterTuin.Instellingen.BedragBoetePerDag)/100;
            return 0;
        }

        /// <summary> Verlengt de uitlening van een uitgeleende item </summary>
        /// <returns>Retourneerd true als dit mogelijk en gewijzigd is</returns>
        /// <param name="id"> Id van uitlening </param>
        public bool VerlengUitlening(int id)
        {
            CheckBibliotheekRechten();
            Uitlening uitlening = GetUitlening(id);
            if (uitlening.Verlenging < LetterTuin.Instellingen.MaxVerlengingen)
            {
                uitlening.update(uitlening);
                uitlening.EindDatum = uitlening.EindDatum.AddDays(LetterTuin.Instellingen.UitleenDagen);
                uitlening.Verlenging += 1;
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

        /// <summary> Geeft een lijst van uitleners </summary>
        /// <returns>Returns IEnumerable van Uitleners</returns>
        /// <exception>Throws Accessviolationexception indien gebruiker geen rechten heeft. </exception>
        public IEnumerable<Uitlener> GetUitleners(String search)
        {
            CheckBibliotheekRechten();

            if (search != null && !search.Trim().IsEmpty())
                return LetterTuin.Uitleners.Where(p => p.Naam.ToLower().Contains(search.ToLower().Trim()) ||
                                                       p.Klas.ToLower().Contains(search.ToLower().Trim()) ||
                                                       p.VoorNaam.ToLower().Contains(search.ToLower().Trim())).OrderBy(p=>p.Naam); 
            return LetterTuin.Uitleners;
        }

        /// <summary> Geeft een lijst van uitleners </summary>
        /// <exception>Throws Accessviolationexception indien gebruiker geen rechten heeft. </exception>
        public void AddUitlener(Uitlener uitlener)
        {
            CheckAdminRechten();
            LetterTuin.Uitleners.Add(uitlener);
        }

        /// <summary> Geeft uitlener terug op basis van id </summary>
        /// <exception>Throws Accessviolationexception indien gebruiker geen rechten heeft. </exception>
        public Uitlener GetUitlenerById(int id)
        {
            CheckBibliotheekRechten();
            if(LetterTuin.Uitleners.Count > 0 && id > 0)
                return LetterTuin.Uitleners.FirstOrDefault(u => u.Id == id);
            return null;
        }

        /// <summary> Update de parameters van een uitlener</summary>
        /// <returns> Geeft true weer als het boek is aangepast. Exemplaar van het boek moet > 0 </returns>
        /// <param name="uitlener"> te updaten uitlener </param>
        public bool UpdateUitlener(Uitlener uitlener)
        {
            CheckAdminRechten();
            if (uitlener != null && uitlener.Id > 0)
            {
                var newUitlener = GetUitlenerById(uitlener.Id);
                if (newUitlener != null)
                {
                    newUitlener.Update(uitlener);
                    return true;
                }
            }
            return false;
        }

        /// <summary> Verwijdert uitlener op basis van id </summary>
        /// <exception>Throws Accessviolationexception indien gebruiker geen rechten heeft. </exception>
        /// <param name="uitlener"> te verwijderen uitlener </param>
        public bool RemoveUitlener(Uitlener uitlener)
        {
            CheckAdminRechten();
            if (uitlener != null && uitlener.Id > 0)
            {
                LetterTuin.Uitleners.Remove(uitlener);
                return true;
            }
            return false;
        }

        /// <summary> controleert indien een verlenging mogelijk is </summary>
        /// <exception>Throws Accessviolationexception indien gebruiker geen rechten heeft. </exception>
        /// <returns>true indien verlenging mogelijk is</returns>
        /// <param name="item"> item te verlengen uitlening </param>
        public bool CheckVerlenging(Item item)
        {
            CheckBibliotheekRechten();
            Uitlening uitlening = GetOpenUitleningByItem(item.Exemplaar);
            if (uitlening.Verlenging < LetterTuin.Instellingen.MaxVerlengingen)
                return true;
            return false;
         }


        public List<Thema> GetThemaListFromSelectedList(List<int> submittedThemas)
        {
            return  submittedThemas.Select(id => LetterTuin.GetThemaById(id)).ToList();
        }

        public List<Boek> GetBoekListFromSelectedList(List<int> submittedBoeken)
        {
            return submittedBoeken.Select(id => LetterTuin.GetBoeken().SingleOrDefault(e => e.Exemplaar == id)).ToList();
        }

        public List<CD> GetCDListFromSelectedList(List<int> submittedCDs)
        {
            return submittedCDs.Select(id => LetterTuin.GetCDs().SingleOrDefault(e => e.Exemplaar == id)).ToList();
        }

        public List<DVD> GetDVDListFromSelectedList(List<int> submittedDvds)
        {
            return submittedDvds.Select(id => LetterTuin.GetDVDs().SingleOrDefault(e => e.Exemplaar == id)).ToList();
        }

        public List<Spel> GetSpelListFromSelectedList(List<int> submittedSpellen)
        {
            return submittedSpellen.Select(id => LetterTuin.GetSpellen().SingleOrDefault(e => e.Exemplaar == id)).ToList();
        }


        /// <summary> returns a list of themas from an item</summary>
        /// <returns>Returns a list of themas from an item</returns>
        /// <param name="item"> an item </param> 
        public List<Thema> GetThemasFromItem(Item item)
        { 
            if (item !=null)
                return LetterTuin.Themas.Where(p => p.Items.Contains(item)).ToList();
            return new List<Thema>();
        }




        

        /// <summary> Geeft de instellingen terug</summary>
        /// <returns>Geeft de instellingen terug</returns> 
        public Instellingen GetInstellingen()
        {
            return LetterTuin.Instellingen;
        }

        /// <summary> Past de basisinstellingen van de lettertuin aan</summary> 
        /// <param name="instellingenVm"> Het viewmodel van instellingen </param> 
        public void UpdateInstellingen(InstellingenViewModel instellingenVm)
        {
            Instellingen instellingen = LetterTuin.Instellingen;
            instellingen.UitleenDagen = instellingenVm.UitleenDagen;
            instellingen.MaxVerlengingen = instellingenVm.MaxVerlengingen;
        }
    }
}