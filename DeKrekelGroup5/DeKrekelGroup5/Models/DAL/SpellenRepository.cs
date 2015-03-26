using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class SpellenRepository : ISpellenRepository
    {
        private KrekelContext context;
        private DbSet<Spel> spellen;

        public SpellenRepository(KrekelContext context)
        {
            this.context = context;
            spellen = context.Spellen;
        }

        public IQueryable<Spel> FindAll()
        {
            return context.Spellen;
        }

        public IQueryable<Spel> Find(string search)
        {
            return context.Spellen.Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                             p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                             p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                             p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        public Spel FindById(int id)
        {
            return context.Spellen.Find(id);
        }

        public void DoNotDuplicateThema(Spel spel)
        {
            context.Entry(spel.Themaa).State = EntityState.Modified; //Zorgt dat het thema niet aangemaakt wordt.
        }

        public void Add(Spel spel)
        {
            spellen.Add(spel);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Remove(Spel spel)
        {
            context.Spellen.Remove(spel);
        }
    }
}