using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitbucket.Models
{
    public class BitbucketDbContext : DbContext
    {
        public BitbucketDbContext()
        {
        }

        public BitbucketDbContext(DbContextOptions<BitbucketDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=BitbucketDB");
            }
        }
    }
}
