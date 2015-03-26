using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface ICDsRepository
    {
        IQueryable<CD> FindAll();
        IQueryable<CD> Find(String search);
        CD FindById(int id);
        void Add(CD cd);
        void SaveChanges(CD cd);
        void Remove(CD cd);
    }
}