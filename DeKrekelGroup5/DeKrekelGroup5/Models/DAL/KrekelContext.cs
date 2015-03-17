using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.DAL.Mappers;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.Models.DAL
{
    public class KrekelContext : DbContext
    {
        public KrekelContext(): base("name=dekrekelDB"){ }
        public KrekelContext(string connStringName) : base(connStringName) { }

        public DbSet<Boek> Boeken { get; set; }
        public DbSet<Spel> Spellen { get; set; }
        public DbSet<Thema> Themas { get; set; }
        public DbSet<VertelTas> VertelTassen { get; set; }

        public DbSet<Uitlening> Uitleningen { get; set; }
        public DbSet<Uitlener> Uitleners { get; set; }
        public DbSet<Beheerder> Beheerders { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BoekMapper());
            modelBuilder.Configurations.Add(new SpelMapper());
            modelBuilder.Configurations.Add(new ThemaMapper());
            modelBuilder.Configurations.Add(new VerteltasMapper());
            modelBuilder.Configurations.Add(new UitleningMapper());
            modelBuilder.Configurations.Add(new UitlenerMapper());
            modelBuilder.Configurations.Add(new BeheerderMapper());
            
            base.OnModelCreating(modelBuilder);
        }

        

        
    }
}