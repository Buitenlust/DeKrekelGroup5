using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class BibliothecarisRepository : IBibliothecarisRepository
    {
        private KrekelContext context;

        //uitleningen
        public BibliothecarisRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<Uitlening> FindAllUitleningen()
        {
            return context.Uitleningen;
        }

        public Uitlening FindByIdUitlening(int id)
        {
            return context.Uitleningen.Find(id);
        }

        public void Add(Uitlening uitlening)
        {
            context.Uitleningen.Add(uitlening);
        }

        public void SaveChanges(Uitlening uitlening)
        {
            context.SaveChanges();
        }

        //uitleners
        public IQueryable<Uitlener> FindAllUitleners()
        {
            return context.Uitleners;
        }

        public Uitlener FindByIdUitlener(int id)
        {
            return context.Uitleners.Find(id);
        }
    }
}