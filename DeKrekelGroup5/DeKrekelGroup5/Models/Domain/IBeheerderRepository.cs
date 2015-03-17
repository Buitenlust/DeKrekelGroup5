using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.Domain
{
    public interface IBeheerderRepository
    {
        Beheerder FindBeheerder(String naam);
        void SaveChanges(Boek boek);
    }
}