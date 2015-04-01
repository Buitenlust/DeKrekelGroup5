using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class BibliothecarisMapper : EntityTypeConfiguration<Bibliothecaris>
    {
        public BibliothecarisMapper()
        {
            //Table
            ToTable("Bibliothecaris");

            //key
            HasKey(bi => bi.Id);

            //Props
            Property(bi => bi.Password).IsRequired();
        }
    }
}