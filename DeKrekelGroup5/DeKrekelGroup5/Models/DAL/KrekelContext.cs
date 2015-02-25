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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BoekMapper());
            modelBuilder.Configurations.Add(new SpelMapper());
            modelBuilder.Configurations.Add(new ThemaMapper());
            
            base.OnModelCreating(modelBuilder);
        }

        

        
    }
}