using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class InstellingenMapper : EntityTypeConfiguration<Instellingen>
    {
        public InstellingenMapper()
        {
            //table
            ToTable("Instellingen");

            //Key
            HasKey(i => i.Id);
        }

    }
}