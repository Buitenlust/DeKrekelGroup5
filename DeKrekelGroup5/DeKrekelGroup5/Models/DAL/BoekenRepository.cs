using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class BoekenRepository : IBoekenRepository
    {
        private KrekelContext context;

        public BoekenRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<Boek> FindAll()
        {
            return context.Boeken;
        }

        public Boek FindById(int id)
        {
            return context.Boeken.Find(id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}