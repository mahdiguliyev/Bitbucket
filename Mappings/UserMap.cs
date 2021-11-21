using Bitbucket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitbucket.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(u => u.Email).HasMaxLength(25);
            builder.Property(u => u.Name).HasMaxLength(30);
            builder.Property(u => u.Name).HasMaxLength(30);

            builder.ToTable("Users");
        }
    }
}
