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
        Task<List<KeyValuePair<string, string>>> Statistic(string metric, bool? isSuccess, DateTime? startDate, DateTime? endDate);
        string GenerateRandomPassword(int size = 0);
        string GenerateRandomEmail();
        string GenerateRandomNameAndSurname();
    }
}
