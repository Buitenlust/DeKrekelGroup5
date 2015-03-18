using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class ItemsViewModel
    {
        public IEnumerable<ItemViewModel> Items { get; set; } 

        public ItemsViewModel(IEnumerable<Item> items)
        {
                Items = items.Select(p => new ItemViewModel(p));
        }
    }
}