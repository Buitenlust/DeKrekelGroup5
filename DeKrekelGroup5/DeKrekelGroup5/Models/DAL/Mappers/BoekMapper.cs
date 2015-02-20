using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL.Mappers
{
    public class BoekMapper : EntityTypeConfiguration<Boek>
    {
        public BoekMapper()
        {
            ToTable("Boek");
            HasKey(boek => boek.Exemplaar);
        }
    }
}