using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class SpellenRepository : ISpellenRepository
    {
        private KrekelContext context;

        public SpellenRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<Spel> FindAll()
        {
            return context.Spellen;
        }

        public Spel FindById(int id)
        {
            return context.Spellen.Find(id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}