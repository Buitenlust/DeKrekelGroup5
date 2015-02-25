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
            Property(s => s.Omschrijving).HasMaxLength(1023);
            Property(s => s.Titel).HasMaxLength(45).IsRequired();
            Property(s => s.Uitgever).HasMaxLength(45);

            //Primary Key
            HasKey(spel => spel.Exemplaar);
        }

    }
}