using Bitbucket.Models;
using Bitbucket.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitbucket.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly BitbucketDbContext _context;

        public RepositoryBase(BitbucketDbContext context)
        {
            _context = context;
        }
    }
}
