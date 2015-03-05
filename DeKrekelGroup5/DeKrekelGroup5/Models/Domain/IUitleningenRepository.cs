using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IUitleningenRepository
    {
        IQueryable<Uitlening> FindAll();
        Uitlening FindById(int id);
        void Add(Uitlening uitlening);
        void SaveChanges();
    }
}