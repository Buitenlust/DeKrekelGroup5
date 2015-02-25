using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class ThemasRepository : IThemasRepository
    {
        private KrekelContext context;

        public ThemasRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<Thema> FindAll()
        {
            return context.Themas;
        }

        public Thema FindById(int id)
        {
            return context.Themas.Find(id);
        }

        public void Add(Thema thema)
        {
            context.Themas.Add(thema);
        }

        public void Remove(Thema thema)
        {
            context.Themas.Remove(thema);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}