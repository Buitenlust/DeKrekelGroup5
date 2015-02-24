using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class ThemaMapper : EntityTypeConfiguration<Thema>
    {
        public ThemaMapper()
        {
            //table
            ToTable("Thema");

            //PK
            HasKey(t => t.IdThema);

            //Props
            Property(t => t.Themaa).HasMaxLength(45);
        }
    }
}