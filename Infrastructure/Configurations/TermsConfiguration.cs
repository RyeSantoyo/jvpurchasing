using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace jvPo.Infrastructure.Configurations
{
    public class TermsConfiguration : IEntityTypeConfiguration<Terms>
    {
        public void Configure(EntityTypeBuilder<Terms> builder)
        {
            builder.ToTable("Terms");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Term)
                .IsRequired();
            
            builder.Property(t=> t.Days)
                .IsRequired();
        }
    }
}