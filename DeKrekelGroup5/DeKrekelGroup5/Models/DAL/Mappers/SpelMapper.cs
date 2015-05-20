using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class SpelMapper : EntityTypeConfiguration<Spel>
    {
        public SpelMapper()
        {
            //table
            ToTable("Spel");

            //properties
            Property(s => s.Omschrijving).HasMaxLength(5000);
            Property(s => s.Titel).HasMaxLength(250).IsRequired();
            Property(s => s.Uitgever).HasMaxLength(250);
            Property(b => b.ImageString).HasMaxLength(254);
            //Primary Key
            HasKey(spel => spel.Exemplaar);
        }

    }
}