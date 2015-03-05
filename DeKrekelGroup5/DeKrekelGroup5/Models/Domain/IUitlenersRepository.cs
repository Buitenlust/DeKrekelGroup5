using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IUitlenersRepository
    {
        IQueryable<Uitlener> FindAll();
        Uitlener FindById(int id);
        void Add(Uitlener uitlener);
        void SaveChanges();
    }
}