using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using jvPo.Models;

namespace jvPo.Infrastructure.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Suppliers>
    {
        public void Configure(EntityTypeBuilder<Suppliers> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.SupplierCode)
                .IsRequired();
            builder.Property(s => s.SupplierName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.SupplierAddress)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s=> s.ContactPerson)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.MobileNo)
                .IsRequired();

            builder.HasOne(s => s.Terms)
                .WithMany()
                .HasForeignKey(s => s.TermsId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}