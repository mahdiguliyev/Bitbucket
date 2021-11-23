using Bitbucket.Models;
using Bitbucket.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitbucket.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(BitbucketDbContext context) : base(context)
        {

        }
        private BitbucketDbContext BitbucketDbContext
        {
            get
            {
                return _context as BitbucketDbContext;
            }
        }
        public async Task<User> GetByEmail(string email)
        {
            var user = await BitbucketDbContext.Users.Include(u => u.ListOfLogins).FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<List<KeyValuePair<string, string>>> Statistic(string metric, bool? isSuccess, DateTime? startDate, DateTime? endDate)
        {
            //string username = GenerateRandomUsername();
            //string password = GenerateRandomPassword(12);
            List<KeyValuePair<string, string>> statistics = new List<KeyValuePair<string, string>>();

            var userLoginAttempts = await BitbucketDbContext.UserLoginAttempts.ToListAsync();

            if (startDate != null && endDate !=null)
            {
                userLoginAttempts = await BitbucketDbContext.UserLoginAttempts.Where(st => (!startDate.HasValue || st.AttemptTime >= startDate)
                && (!endDate.HasValue || st.AttemptTime <= endDate)).ToListAsync();
            }
            if (isSuccess != null)
            {
                if (isSuccess == true)
                {
                    userLoginAttempts = await BitbucketDbContext.UserLoginAttempts.Where(st => st.IsSuccess == true).ToListAsync();
                }
                userLoginAttempts = await BitbucketDbContext.UserLoginAttempts.Where(st => st.IsSuccess == false).ToListAsync();
            }
            if (metric == "hour")
            {
                foreach (var userLoginAttempt in userLoginAttempts)
                {
                    statistics.Add(new KeyValuePair<string, string>("Period", userLoginAttempt.AttemptTime.ToString("MM/dd/yyyy HH:mm")));
                    statistics.Add(new KeyValuePair<string, string>("Value", userLoginAttempts.Count().ToString()));
                }
            }
            else if (metric == "month")
            {
                foreach (var userLoginAttempt in userLoginAttempts)
                {
                    statistics.Add(new KeyValuePair<string, string>("Period", userLoginAttempt.AttemptTime.ToString("yyyy MMMM")));
                    statistics.Add(new KeyValuePair<string, string>("Value", userLoginAttempts.Count().ToString()));
                }
            }
            else if(metric == "year")
            {
                foreach (var userLoginAttempt in userLoginAttempts)
                {
                    statistics.Add(new KeyValuePair<string, string>("Period", userLoginAttempt.AttemptTime.ToString("yyyy")));
                    statistics.Add(new KeyValuePair<string, string>("Value", userLoginAttempts.Count().ToString()));
                }
            }

            return statistics;
        }
        public string GenerateRandomEmail()
        {
            string rv = "";

            char[] lowers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] uppers = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            int l = lowers.Length;
            int u = uppers.Length;
            int n = numbers.Length;

            Random random = new Random();

            rv += lowers[random.Next(0, l)].ToString();
            rv += lowers[random.Next(0, l)].ToString();
            rv += lowers[random.Next(0, l)].ToString();

            rv += uppers[random.Next(0, u)].ToString();
            rv += uppers[random.Next(0, u)].ToString();
            rv += uppers[random.Next(0, u)].ToString();

            rv += numbers[random.Next(0, n)].ToString();
            rv += numbers[random.Next(0, n)].ToString();
            rv += numbers[random.Next(0, n)].ToString();

            return rv + "@gmail.com";
        }
        public string GenerateRandomNameAndSurname()
        {
            string rv = "";

            char[] lowers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] uppers = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            int l = lowers.Length;
            int u = uppers.Length;
            int n = numbers.Length;

            Random random = new Random();

            rv += lowers[random.Next(0, l)].ToString();
            rv += lowers[random.Next(0, l)].ToString();
            rv += lowers[random.Next(0, l)].ToString();

            rv += uppers[random.Next(0, u)].ToString();
            rv += uppers[random.Next(0, u)].ToString();
            rv += uppers[random.Next(0, u)].ToString();

            rv += numbers[random.Next(0, n)].ToString();
            rv += numbers[random.Next(0, n)].ToString();
            rv += numbers[random.Next(0, n)].ToString();

            return rv;
        }
        public string GenerateRandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
