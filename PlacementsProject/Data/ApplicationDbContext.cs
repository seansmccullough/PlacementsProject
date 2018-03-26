using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlacementsProject.Models;

namespace PlacementsProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Adjustment> Adjustments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<LineItem>().ToTable("LineItems");
            builder.Entity<Campaign>().ToTable("Campaigns");
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Adjustment>().ToTable("Adjustments");
        }
    }
}
