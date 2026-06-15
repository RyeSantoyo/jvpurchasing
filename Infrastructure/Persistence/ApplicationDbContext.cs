using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; } = null!;
        public DbSet<Terms> Terms { get; set; } = null!;
        public DbSet<PO> POs { get; set; } = null!;
        public DbSet<PODetails> PODetails { get; set; } = null!;
        public DbSet<Suppliers> Suppliers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CompanyConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PODetailsConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PurchaseOrderConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SupplierConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersConfiguration).Assembly);
            
            // Configure relationships and constraints here if needed
        }
    }
}