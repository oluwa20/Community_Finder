using Community_Finder2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Community_Finder2.Data
{
    public class Community_Finder2DbContext : DbContext
    {
        public Community_Finder2DbContext(DbContextOptions<Community_Finder2DbContext> options)
           : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Community> Communities { get; set; }
    }
}
