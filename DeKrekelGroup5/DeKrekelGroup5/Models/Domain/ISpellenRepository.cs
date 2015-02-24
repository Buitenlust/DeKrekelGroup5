﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface ISpellenRepository
    {
        IQueryable<Spel> FindAll();
        Spel FindById(int id);
        void SaveChanges();
    }
}