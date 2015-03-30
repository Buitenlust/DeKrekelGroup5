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

            //Props
            Property(i => i.MaxVerlengingen).Equals(4);
            Property(i => i.UitleenDagen).IsRequired().Equals(14);
            Property(i => i.BedragBoetePerDag).Equals(0.2);
        }

    }
}