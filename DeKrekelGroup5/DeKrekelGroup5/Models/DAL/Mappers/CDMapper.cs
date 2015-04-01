using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class CDMapper : EntityTypeConfiguration<CD>
    {
        public CDMapper()
        {

            //table
            ToTable("CD");

            //properties
            Property(c => c.Omschrijving).HasMaxLength(1023);
            Property(c => c.Titel).HasMaxLength(45).IsRequired();
            Property(c => c.Uitgever).HasMaxLength(45);
            Property(c => c.Leeftijd).IsRequired();
            Property(c => c.Beschikbaar).IsRequired();
            Property(c => c.Uitgeleend).IsRequired();

            //Primary Key
            HasKey(c => c.Exemplaar);
        }
    }
}