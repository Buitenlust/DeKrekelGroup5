using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IThemasRepository
    {
        IQueryable<Thema> FindAll();
        Thema FindById(int id);
        void Add(Thema thema);
        void Remove(Thema thema);
        void SaveChanges();
    }
}