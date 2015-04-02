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

    }
}