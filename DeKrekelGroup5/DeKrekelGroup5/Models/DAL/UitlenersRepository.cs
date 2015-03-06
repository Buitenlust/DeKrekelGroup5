using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class UitlenersRepository : IUitlenersRepository
    {
        private KrekelContext context;

        public UitlenersRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<Uitlener> FindAll()
        {
            return context.Uitleners;
        }

        public Uitlener FindById(int id)
        {
            return context.Uitleners.Find(id);
        }

        public void Add(Uitlener uitlener)
        {
            context.Uitleners.Add(uitlener);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}