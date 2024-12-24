using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pizza_api.Models;

namespace pizza_api.Data
{
    public class pizza_apiContext : DbContext
    {
        public pizza_apiContext (DbContextOptions<pizza_apiContext> options)
            : base(options)
        {
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Campaign>()
        //        .Property(c => c.Id)
        //        .ValueGeneratedOnAdd(); // Ensure EF knows this is an identity column

        //    modelBuilder.Entity<CampaignRecipient>()
        //        .Property(cr => cr.Id)
        //        .ValueGeneratedOnAdd();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CRecipients>()
                .Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");
        }

        public DbSet<Pizza> Pizza { get; set; } = default!;
        public DbSet<Campaign> Campaign { get; set; } = default!;
        public DbSet<CampaignRecipient> CampaignRecipient { get; set; } = default!;



    }
}
