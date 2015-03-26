using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class DVDsRepository : IDVDsRepository
    {
        private KrekelContext context;

        public DVDsRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<DVD> FindAll()
        {
            return context.Dvds;
        }

        public IQueryable<DVD> Find(string search)
        {
            return context.Dvds.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                             p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                             p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                             p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        public DVD FindById(int id)
        {
            return context.Dvds.Find(id);
        }

        public void Add(DVD dvd)
        {
            context.Dvds.Add(dvd);
        }

        public void SaveChanges(DVD dvd)
        {
            context.Entry(dvd.Themaa).State = EntityState.Modified;    //Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Remove(DVD dvd)
        {
            context.Dvds.Remove(dvd);
        }
    }
}