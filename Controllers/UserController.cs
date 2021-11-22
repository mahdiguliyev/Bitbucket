using Bitbucket.Models;
using Bitbucket.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bitbucket.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly BitbucketDbContext _context;
        private readonly JWTSettings _jwtsettings;
        private readonly IUserRepository _userRepository;
        public UserController(BitbucketDbContext context,
                               IOptions<JWTSettings> jwtsettings, IUserRepository userRepository)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
            _userRepository = userRepository;
        }

        [HttpPost("Init")]
        public async Task<ActionResult<UserWithToken>> Init([FromBody] User user)
        {
            var useR = await _context.Users
                       .Include(u => u.ListOfLogins)
                       .Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefaultAsync();

            UserWithToken userWithToken = null;

            if (useR != null)
            {
                RefreshToken refreshToken = GenerateRefreshToken();
                useR.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                UserLoginAttempt userLoginAttempt = new UserLoginAttempt();
                userLoginAttempt.AttemptTime = DateTime.Now;
                userLoginAttempt.IsSuccess = true;
                useR.ListOfLogins.Add(userLoginAttempt);

                userWithToken = new UserWithToken(useR);
                userWithToken.RefreshToken = refreshToken.Token;
            }

            if (userWithToken == null)
            {
                UserLoginAttempt userLoginAttempt = new UserLoginAttempt();
                userLoginAttempt.AttemptTime = DateTime.Now;
                userLoginAttempt.IsSuccess = false;
                useR.ListOfLogins.Add(userLoginAttempt);

                return NotFound();
            }
            await _context.SaveChangesAsync();

            userWithToken.AccessToken = GenerateAccessToken(useR.Id);

            return userWithToken;
        }
        [HttpPost("GenerateUser")]
        public async Task<ActionResult<User>> GenerateUser()
        {
            User newUser = new User();
            newUser.Email = _userRepository.GenerateRandomEmail();
            newUser.Password = _userRepository.GenerateRandomPassword();
            newUser.Name = _userRepository.GenerateRandomNameAndSurname();
            newUser.Surname = _userRepository.GenerateRandomNameAndSurname();

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }
        private string GenerateAccessToken(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
                }),
                Expires = DateTime.UtcNow.AddSeconds(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
