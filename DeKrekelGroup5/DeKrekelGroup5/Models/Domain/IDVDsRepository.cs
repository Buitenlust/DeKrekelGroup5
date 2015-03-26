using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IDVDsRepository
    {
        IQueryable<DVD> FindAll();
        IQueryable<DVD> Find(String search);
        DVD FindById(int id);
        void Add(DVD dvd);
        void SaveChanges(DVD dvd);
        void Remove(DVD dvd);
    }
}