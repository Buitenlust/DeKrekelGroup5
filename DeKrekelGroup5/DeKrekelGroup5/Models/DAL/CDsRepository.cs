using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class CDsRepository : ICDsRepository
    {
        private KrekelContext context;

        public CDsRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<CD> FindAll()
        {
            return context.Cds;
        }

        public IQueryable<CD> Find(string search)
        {
            return context.Cds.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                          p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                          p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                          p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        public CD FindById(int id)
        {
            return context.Cds.Find(id);
        }

        public void Add(CD cd)
        {
            context.Cds.Add(cd);
        }

        public void SaveChanges(CD cd)
        {
            context.Entry(cd.Themaa).State = EntityState.Modified; //Zorgt dat het thema niet aangemaakt wordt.
            context.SaveChanges();
        }

        public void Remove(CD cd)
        {
            context.Cds.Remove(cd);
        }
    }
}