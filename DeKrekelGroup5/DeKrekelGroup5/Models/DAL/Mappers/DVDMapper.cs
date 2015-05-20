using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class DVDMapper : EntityTypeConfiguration<DVD>
    {
        public DVDMapper()
        {


            //table
            ToTable("DVD");

            //properties
            Property(d => d.Omschrijving).HasMaxLength(5000);
            Property(d => d.Titel).HasMaxLength(250).IsRequired();
            Property(d => d.Uitgever).HasMaxLength(250);            
            Property(b => b.ImageString).HasMaxLength(254);
            //Primary Key
            HasKey(d => d.Exemplaar);

        }
    }
}