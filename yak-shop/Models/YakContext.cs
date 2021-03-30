using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yak_shop.DetailsAndUtilities;

namespace yak_shop.Models
{
    public class YakContext : DbContext
    {
        public YakContext(DbContextOptions<YakContext> options)
            : base(options)
        {
        }

        public DbSet<YakDetails> YakItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data
            modelBuilder.Entity<YakDetails>().HasData(
                new YakDetails()
                {
                    Id = 1,
                    Name = "Billy",
                    Age = 3,
                    Sex = 'f',
                    ageLastShaved = 3
                }
            );
            base.OnModelCreating(modelBuilder);
        }

    }
}
