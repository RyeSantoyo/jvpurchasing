using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using jvPo.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace jvPo.Infrastructure.Configurations
{
    public class PurchaseOrderConfigutation : IEntityTypeConfiguration<PO>
    {
        public void Configure(EntityTypeBuilder<PO> builder)
        {
            builder.ToTable("PO");

            builder.HasKey(po => po.Id);
            builder.Property(po => po.PONumber)
                .IsRequired();
            builder.HasIndex(po => po.PONumber)
                .IsUnique();
            builder.Property(po => po.PODate)
                .IsRequired();
            builder.Property(po => po.RequestedBy)
                .IsRequired();
            builder.Property(po => po.OrderBy)
                .IsRequired();
            builder.Property(po => po.RONumber)
                .IsRequired();
            builder.Property(po => po.RODate)
                .IsRequired();
            builder.Property(po => po.TotalAmount)
                .IsRequired();
            
            builder.HasOne(po=> po.Company)
                .WithMany()
                .HasForeignKey(po => po.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(po => po.Supplier)
                .WithMany()
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(po => po.User)
                .WithMany()
                .HasForeignKey(po => po.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(po=> po.Terms)
                .WithMany()
                .HasForeignKey(po => po.TermsId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(po=> po.Address)
                .WithMany()
                .HasForeignKey(po => po.DeliveryAddressID)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}