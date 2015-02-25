using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class VerteltasRepository : IVerteltasRepository
    {
        private KrekelContext context;
        private DbSet<VertelTas> verteltassen; 

        public VerteltasRepository(KrekelContext context)
        {
            this.context = context;
            verteltassen = context.VertelTassen;
        }

        public IQueryable<VertelTas> FindAll()
        {
            return context.VertelTassen;
        }

        public VertelTas FindById(int id)
        {
            return context.VertelTassen.Find(id);
        }

        public void Add(VertelTas vertelTas)
        {
            verteltassen.Add(vertelTas);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}