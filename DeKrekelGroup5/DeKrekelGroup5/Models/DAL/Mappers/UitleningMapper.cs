using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class UitleningMapper : EntityTypeConfiguration<Uitlening>
    {
        public UitleningMapper()
        {
            //table
            ToTable("Uitlening");
            //Key
            HasKey(uit => uit.Id);

            //Props
            Property(uit => uit.StartDatum).IsRequired();
            Property(uit => uit.EindDatum).IsRequired();
        }

    }
}