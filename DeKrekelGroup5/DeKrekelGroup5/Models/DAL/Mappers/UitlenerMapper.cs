using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class UitlenerMapper : EntityTypeConfiguration<Uitlener>
    {
        
        public UitlenerMapper()
        {
            //table
            ToTable("Uitlener");

            //Key
            HasKey(u => u.Id);

            //Props
            Property(u => u.Naam).HasMaxLength(125);
            Property(u => u.VoorNaam).HasMaxLength(125);
            Property(u => u.Adres).HasMaxLength(255);
            Property(u => u.Email).HasMaxLength(125);
            Property(u => u.Klas).HasMaxLength(45);
        }
    }
}