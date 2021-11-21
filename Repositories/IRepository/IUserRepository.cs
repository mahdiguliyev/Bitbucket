using Bitbucket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitbucket.Repositories.IRepository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetByEmail(string email);
        Task<UserLoginAttempt> Statistic(DateTime metric, bool? isSuccess, string startDate = null, string endDate = null);
    }
}
