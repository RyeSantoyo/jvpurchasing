using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using jvPo.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace jvPo.Infrastructure.Configurations
{
    public class PODetailsConfiguration : IEntityTypeConfiguration<PODetails>
    {
        public void Configure(EntityTypeBuilder<PODetails> builder)
        {
            builder.ToTable("PODetails");

            builder.HasKey(pd => pd.Id);

            builder.Property(pd => pd.Quantity)
                .IsRequired();
            builder.Property(pd => pd.Unit)
                .IsRequired();
            builder.Property(pd => pd.Description)
                .HasMaxLength(200);
            builder.Property(pd => pd.Price)
                .IsRequired();
            builder.Property(pd=> pd.Total)
                .IsRequired();
            builder.HasOne(pd => pd.Company)
                .WithMany()
                .HasForeignKey(pd => pd.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(pd => pd.PurchaseOrder)
            .WithMany(pd=> pd.PODetails)
            .HasForeignKey(pd => pd.POId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}