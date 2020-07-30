using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChecklistAPI.Helpers;
using EquipmentChecklistDataAccess;
using EquipmentChecklistDataAccess.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChecklistAPI.Services
{

    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings appSettings;
        private EquipmentChecklistDBContext context;

        public UserService(IOptions<AppSettings> appSettings, EquipmentChecklistDBContext context)
        {
            this.appSettings = appSettings.Value;
            this.context = context;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await context.Users.SingleOrDefaultAsync(x => x.ID == model.Username && x.Password == model.Password);
            
            if (user == null) return null;

            var token = generateJWTtoken(user);

            return new AuthenticateResponse(user, token);


        }

        private string generateJWTtoken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

}
