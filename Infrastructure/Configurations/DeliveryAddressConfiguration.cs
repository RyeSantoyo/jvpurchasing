using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace jvPo.Infrastructure.Configurations
{
    public class DeliveryAddressConfiguration : IEntityTypeConfiguration<DeliveryAddress>
    {
        public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
        {
            builder.ToTable("DeliveryAddress"); 
            
            builder.HasKey(da => da.Id);
            builder.Property(da => da.Address)
                .IsRequired()
                .HasMaxLength(200);

        }
    }
}