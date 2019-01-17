using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GameBoard> GameBoards { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<BoardRow> BoardRows { get; set; }
        public DbSet<Cell> Cells { get; set; }

        public AppDbContext(){}
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseMySQL("server=alpha.akaver.com;" +
                          "database=student2018_iiounm_BS;" +
                          "user=student2018;" +
                          "password=student2018");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }
            modelBuilder.Entity<Game>()
                .HasKey(x => x.GameId);
                
            modelBuilder.Entity<Game>()
                .HasOne(x => x.Winner);
            
            modelBuilder.Entity<Game>()
                .HasOne(x => x.Turn);

            modelBuilder.Entity<Player>()
                .HasKey(x => x.PlayerId);

            modelBuilder.Entity<Game>()
                .HasOne(x => x.PlayerOne);

            modelBuilder.Entity<Game>()
                .HasOne(x => x.PlayerTwo);
            

            base.OnModelCreating(modelBuilder);
        }
        
        class BoolToIntConverter : ValueConverter<bool, int> {
            public BoolToIntConverter(ConverterMappingHints mappingHints = null)
                : base(
                    v => Convert.ToInt32(v),
                    v => Convert.ToBoolean(v),
                    mappingHints)
            {
            }

            public static ValueConverterInfo DefaultInfo { get; }
                = new ValueConverterInfo(typeof(bool), typeof(int), i => new BoolToIntConverter(i.MappingHints));
        }

    }
}