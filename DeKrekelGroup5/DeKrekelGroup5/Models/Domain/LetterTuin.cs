using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DeKrekelGroup5.Models.DAL;
using DeKrekelGroup5.ViewModel;
using Ninject.Activation;

namespace DeKrekelGroup5.Models.Domain
{
    public class LetterTuin
    {
        public virtual IEnumerable<Item> Items { get; set; }
        public virtual IEnumerable<Boek> Boeken { get; set; }
        public virtual IEnumerable<CD> CDs { get; set; }
        public virtual IEnumerable<DVD> DVDs { get; set; }
        public virtual IEnumerable<VertelTas> VertelTassen { get; set; }
        public virtual IEnumerable<Spel> Spellen { get; set; } 
        public virtual IEnumerable<Thema> Themas { get; set; }

        /// <summary> Get all boeken that contains a search keyword in Titel, Uitgever, Auteur, Thema or omschrijving </summary>
        /// <param name="search"> search keyword </param>
        public IEnumerable<Boek> GetBoeken(string search)
        {
            return Boeken.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                p.Auteur.ToLower().Contains(search.ToLower()) ||
                                p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        /// <summary> Get all spellen that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <param name="search"> search keyword </param>
        public IEnumerable<Spel> GetSpellen(string search)
        {
            return Spellen.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                 p.Uitgever.ToLower().Contains(search.ToLower()) ||             
                                 p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                 p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        /// <summary> Get all cd's that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <param name="search"> search keyword </param>
        public IEnumerable<CD> GetCDs(string search)
        {
            return CDs.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                             p.Uitgever.ToLower().Contains(search.ToLower()) ||
                             p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                             p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        /// <summary> Get all dvd's that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <param name="search"> search keyword </param>
        public IEnumerable<DVD> GetDVDs(string search)
        {
            return DVDs.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                              p.Uitgever.ToLower().Contains(search.ToLower()) ||
                              p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                              p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        /// <summary> Get all verteltassen that contains a search keyword in Titel, Uitgever, Thema or omschrijving </summary>
        /// <param name="search"> search keyword </param>
        public IEnumerable<VertelTas> GetVertelTassen(string search)
        {
            return VertelTassen.Where(p => p.Titel.ToLower().Contains(search.ToLower()) || 
                                           p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                           p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        /// <summary> Get all themas ordered by themas name </summary> 
        public IEnumerable<Thema> GetThemas()
        {
            return Themas.OrderBy(t => Themas);
        }

        /// <summary> Get the first thema that matches the thema search parameter </summary>
        /// <param name="thema"> search keyword </param>
        public Thema GetThemaByName(string thema)
        {
            return Themas.SingleOrDefault(t => t.Themaa == thema);
        }

        /// <summary> Get the first boek that matches the item id  parameter </summary>
        /// <param name="id"> search keyword </param>
        public Boek GetBoek(int id)
        {
            return Boeken.SingleOrDefault(i => i.Exemplaar == id);
        }

        /// <summary> Get the first spel that matches the item id  parameter </summary>
        /// <param name="id"> search keyword </param>
        public Spel GetSpel(int id)
        {
            return Spellen.SingleOrDefault(i => i.Exemplaar == id);
        }

        /// <summary> Get the first cd that matches the item id  parameter </summary>
        /// <param name="id"> search keyword </param>
        public CD GetCD(int id)
        {
            return CDs.SingleOrDefault(i => i.Exemplaar == id);
        }

        /// <summary> Get the first dvd that matches the item id  parameter </summary>
        /// <param name="id"> search keyword </param>
        public DVD GetDVD(int id)
        {
            return DVDs.SingleOrDefault(i => i.Exemplaar == id);
        }

        /// <summary> Get the first verteltas that matches the item id  parameter </summary>
        /// <param name="id"> search keyword </param>
        public VertelTas GetVertelTas(int id)
        {
            return VertelTassen.SingleOrDefault(i => i.Exemplaar == id);
        }

        /// <summary> Get the first boek that matches the item id  parameter </summary>
        /// <param name="id"> search keyword </param>
        public Boek AddBoek(Boek boek)
        {
            return null;
        }



    }
}