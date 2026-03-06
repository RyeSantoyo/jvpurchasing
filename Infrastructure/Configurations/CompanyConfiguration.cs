using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace jvPo.Infrastructure.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CompanyCode)
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}