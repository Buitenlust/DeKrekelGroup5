using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class DVDMapper : EntityTypeConfiguration<DVD>
    {
        //table
            ToTable("DVD");

        //properties
        Property(d => d.Omschrijving).HasMaxLength(1023);
        Property(d => d.Titel).HasMaxLength(45).IsRequired();
        Property(d => d.Uitgever).HasMaxLength(45)

        //Primary Key
        HasKey(d => d.Exemplaar);
    }
}