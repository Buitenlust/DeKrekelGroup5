using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IVerteltasRepository
    {
        IQueryable<VertelTas> FindAll();
        VertelTas FindById(int id);
        void Add(VertelTas vertelTas);
        void SaveChanges();
    }
}