using Microsoft.EntityFrameworkCore;
using ShopBridgeData.DBModels;
using System;

namespace ShopBridgeData
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ErrorLog> Logs { get; set; }
    }
}
