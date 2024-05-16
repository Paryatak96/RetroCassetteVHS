using RetroCassetteVHS.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection.Emit;

namespace RetroCassetteVHS.Infrastructure
{
    public class Context : IdentityDbContext
    {
        public DbSet<Cassette> Cassettes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            });
        }
    }
}
