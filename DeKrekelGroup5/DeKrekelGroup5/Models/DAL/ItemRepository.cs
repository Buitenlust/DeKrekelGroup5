using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class ItemRepository : IItemRepository
    {
        private KrekelContext context;

        public ItemRepository(KrekelContext context)
        {
            this.context = context;
        }

        public IQueryable<Item> FindAll()
        {
            return context.Items;
        }

        public IQueryable<Boek> FindBoek(string search)
        {
            return context.Items.OfType<Boek>().Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                             p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                             p.Auteur.ToLower().Contains(search.ToLower()) ||
                                             p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                             p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        public IQueryable<Spel> FindSpel(string search)
        {
            return context.Items.OfType<Spel>().Where(p => p.Titel.ToLower().Contains(search.ToLower()) ||
                                             p.Uitgever.ToLower().Contains(search.ToLower()) ||
                                             p.Themaa.Themaa.ToLower().Contains(search.ToLower()) ||
                                             p.Omschrijving.ToLower().Contains(search.ToLower())).OrderBy(p => p.Titel);
        }

        public Item FindById(int id)
        {
            return context.Items.Find(id);
        }

        public void Add(Item item)
        {

            context.Items.Add(item);
        }

        public void ModifiedThema(Item item)
        {
            context.Entry(item.Themaa).State = EntityState.Modified;    //Zorgt dat het thema niet aangemaakt wordt.
        }

        public void SaveChanges()
        {
            
            context.SaveChanges();
        }

        public void Remove(Item item)
        {
            context.Items.Remove(item);
        }

        public Item FindLastItem()
        {

            return context.Items.OrderByDescending(id => id.Exemplaar).FirstOrDefault();
        }
    }
}