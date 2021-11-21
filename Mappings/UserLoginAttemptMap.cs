using Bitbucket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitbucket.Mappings
{
    public class UserLoginAttemptMap : IEntityTypeConfiguration<UserLoginAttempt>
    {
        public void Configure(EntityTypeBuilder<UserLoginAttempt> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne<User>(a => a.User).WithMany(c => c.ListOfLogins).HasForeignKey(a => a.UserId);

            builder.ToTable("UserLoginAttempts");
        }
    }
}
