using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IItemRepository
    {
        IQueryable<Item> FindAll();
        IQueryable<Boek> FindBoek(string search);
        IQueryable<Spel> FindSpel(string search);
        Item FindById(int id);
        void Add(Item item);
        void ModifiedThema(Item item);
        void SaveChanges();
        void Remove(Item item);
        Item FindLastItem();

    }
}