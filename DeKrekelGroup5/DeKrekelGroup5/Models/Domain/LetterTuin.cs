using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.ViewModel;
using Microsoft.Ajax.Utilities;
using Ninject.Activation;

namespace DeKrekelGroup5.Models.Domain
{
    public class LetterTuin
    {
        public int Id { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Thema> Themas { get; set; }
        public virtual ICollection<Gebruiker> Gebruikers { get; set; }
        public virtual ICollection<Uitlening> Uitleningen { get; set; }
        public virtual Instellingen Instellingen { get; set; }

        public LetterTuin()
        {
            if(Items  == null)
                Items = new List<Item>();
            if (Themas == null)
                Themas = new List<Thema>();
            if (Instellingen == null)
                Instellingen = new Instellingen();
            if (Uitleningen == null)
                Uitleningen = new List<Uitlening>();
            if (Gebruikers == null)
                Gebruikers = new List<Gebruiker>();
        }

        #region ANONYMOUS

        /// <summary> Get all boeken that contains a search keyword in Titel, Uitgever, Auteur, Thema or omschrijving </summary>
        /// <returns>Returns a IEnumerable of Boeken</returns>
        /// <param name="search"> search keyword </param>
        public IEnumerable<Boek> GetBoeken(string search)
        {
            if (search != null && !search.Trim().IsEmpty())
                return Items.OfType<Boek>().Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                                       p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                                       p.Auteur.ToLower().Contains(search.ToLower()) ||
                                                       p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                                       p.Omschrijving.ToLower().Contains(search.ToLower()))
                    .OrderBy(p => p.Titel);
            return Items.OfType<Boek>().Take(25);
        }

        /// <summary> Get all spellen that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <returns>Returns a IEnumerable of Spellen</returns>
        /// <param name="search"> search keyword </param>
        public IEnumerable<Spel> GetSpellen(string search)
        {
            if (search != null && !search.Trim().IsEmpty())
                return Items.OfType<Spel>().Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                                       p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                                       p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                                       p.Omschrijving.ToLower().Contains(search.ToLower()))
                    .OrderBy(p => p.Titel);
            return Items.OfType<Spel>().Take(25);
        }

        /// <summary> Get all cd's that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <returns>Returns a IEnumerable of Cd's</returns>
        /// <param name="search"> search keyword </param>
        public IEnumerable<CD> GetCDs(string search)
        {
            if (search != null && !search.Trim().IsEmpty())
                return Items.OfType<CD>().Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                                     p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                                     p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                                     p.Omschrijving.ToLower().Contains(search.ToLower()))
                    .OrderBy(p => p.Titel);
            return Items.OfType<CD>().Take(25);
        }

        /// <summary> Get all dvd's that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <returns>Returns a IEnumerable of DVD's</returns>
        /// <param name="search"> search keyword </param>
        public IEnumerable<DVD> GetDVDs(string search)
        {
            if (search != null && !search.Trim().IsEmpty())
                return Items.OfType<DVD>().Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                                      p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                                      p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                                      p.Omschrijving.ToLower().Contains(search.ToLower()))
                    .OrderBy(p => p.Titel);
            return Items.OfType<DVD>().Take(25);
        }

        /// <summary> Get all verteltassen that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <returns>Returns a IEnumerable of Verteltassen</returns>
        /// <param name="search"> search keyword </param>
        public IEnumerable<VertelTas> GetVertelTassen(string search)
        {
            if (search != null && !search.Trim().IsEmpty())
                return Items.OfType<VertelTas>().Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                                            p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                                            p.Omschrijving.ToLower().Contains(search.ToLower()))
                    .OrderBy(p => p.Titel);
            return Items.OfType<VertelTas>().Take(25);
        }


        /// <summary> Get the first thema that matches the thema search parameter </summary>
        /// <param name="thema"> search keyword </param>
        public Thema GetThemaByName(string thema)
        {
            if (thema != null && !thema.Trim().IsEmpty())
                return Themas.SingleOrDefault(t => t.Themaa == thema);
            return null;
        }

        /// <summary>Geeft de eerste thema met betreffende ID terug </summary>
        /// <param name="id"> id van thema </param>
        public Thema GetThemaById(int id)
        {
            if (id > 0)
                return Themas.SingleOrDefault(t => t.IdThema == id);
            return null;
        }

        /// <summary> Get the first item that matches the item id  parameter </summary>
        /// <param name="id"> search keyword </param>
        public Item GetItem(int id)
        {
            if (id > 0)
                return Items.SingleOrDefault(i => i.Exemplaar == id);
            return null;
        }

        #endregion

        #region BIBLIOTHECARIS
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
            if (id > 0)
                return Uitleningen.SingleOrDefault(p => p.Id == id);
            return null;
        }

        /// <summary> Add a new Uitlening to the Collection </summary>
        /// <returns>Returns a boolean true if the uitlening is succesfully created</returns>
        /// <param name="uitlener"> Object Uitlener </param>
        /// <param name="item"> Object item </param>
        public bool VoegUitleningToe(Gebruiker gebruiker, Uitlener uitlener, Item item)
        {
            if (uitlener != null && item != null && uitlener.Id > 0 && item.Exemplaar > 0)
            {
                Uitleningen.Add(new Uitlening()
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
        public decimal GetBoete(Gebruiker gebruiker, int id)
        {
            Uitlening uitlening = GetUitlening(id);
            int dagen = uitlening.EindDatum.Subtract(uitlening.StartDatum).Days;
            if (dagen > Instellingen.UitleenDagen)
                return dagen - Instellingen.UitleenDagen * Instellingen.BedragBoetePerDag;
            return 0;
        }

        /// <summary> Verlengt de uitlening van een uitgeleende item </summary>
        /// <returns>Retourneerd true als dit mogelijk en gewijzigd is</returns>
        /// <param name="id"> Id van uitlening </param>
        public bool VerlengUitlening(Gebruiker gebruiker, int id)
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
        #endregion
        
        #region ADMIN 

        /// <summary> Adds an item to the collection </summary>
        /// <returns>returns true if item is succesfully added to the collection. Exemplaar must be = 0 </returns>
        /// <param name="item"> toe te voegen Item </param>
        public bool AddItem(Gebruiker gebruiker, Item item)
        {
            if (item != null && item.Exemplaar == 0)
            {
                Items.Add(item);
                return true;
            }
            return false;
        }

        /// <summary> Update de parameters van een boek</summary>
        /// <returns> Geeft true weer als het boek is aangepast. Exemplaar van het boek moet > 0 </returns>
        /// <param name="boek"> te updaten boek </param>
        public bool UpdateBoek(Gebruiker gebruiker, Boek boek)
        {
            if (boek != null && boek.Exemplaar > 0)
            {
                var newItem = GetItem(boek.Exemplaar) as Boek;
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
        public bool RemoveItem(Gebruiker gebruiker, int id)
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
        public void PasBoeteAan(Gebruiker gebruiker, int bedrag)
        {
            if (bedrag >= 0)
                Instellingen.BedragBoetePerDag = bedrag;
        }

        /// <summary> Past de maximum aantal toegestane Verlengingen aan</summary> 
        /// <param name="aantal"> nieuwe aantal verlengingen </param>
        public void PasMaxVerlengingen(Gebruiker gebruiker, int aantal)
        {
            if (aantal <= 3 && aantal >= 0)
                Instellingen.MaxVerlengingen = aantal;
        }

        /// <summary> Past de max aantal toegestane uitleendagen aan</summary> 
        /// <param name="dagen"> nieuwe aantal dagen </param>
        public void PasMaxuitleenDagenAan(Gebruiker gebruiker, int dagen)
        {
            if (dagen > 7)
                Instellingen.UitleenDagen = dagen;
        }


        /// <summary> voegt een Thema toe aan de lijst </summary>
        /// <returns>Geeft true weer als het thema werd aangemaakt. </returns>
        /// <param name="thema"> toe te voegen Thema</param>
        public bool AddThema(Gebruiker gebruiker, Thema thema)
        {
            if (thema != null)
            {
                Themas.Add(thema);
                return true;
            }
            return false;
        }


        /// <summary> Update de parameters van een boek</summary>
        /// <returns> Geeft true weer als het boek is aangepast. Exemplaar van het boek moet > 0 </returns>
        /// <param name="thema"> te updaten boek </param>
        public bool UpdateThema(Gebruiker gebruiker, Thema thema)
        {
            if (thema != null && thema.IdThema > 0)
            {
                var th = GetThemaById(thema.IdThema);
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
        public bool RemoveThema(Gebruiker gebruiker, Thema thema)
        {
            Thema th = GetThemaById(thema.IdThema);
            if (th != null && thema.IdThema > 0)
            {
                Themas.Remove(th);
                return true;
            }
            return false;
        }
        #endregion

        
    }
}