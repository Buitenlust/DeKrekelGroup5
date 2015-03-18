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

        public Spel FindById(int id)
        {
            return context.Spellen.Find(id);
        }

        public void Add(Spel spel)
        {
            spellen.Add(spel);
        }

        public void Remove(Spel spel)
        {
            spellen.Remove(spel);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}