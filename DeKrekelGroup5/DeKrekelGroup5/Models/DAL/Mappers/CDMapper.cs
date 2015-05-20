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
            Property(c => c.Omschrijving).HasMaxLength(5000);
            Property(c => c.Titel).HasMaxLength(250).IsRequired();
            Property(c => c.Uitgever).HasMaxLength(250);
            Property(b => b.ImageString).HasMaxLength(254);
            //Primary Key
            HasKey(c => c.Exemplaar);
        }
    }
}