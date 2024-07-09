using AutoMapper;
using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common.Infrastructure.Exceptions;
using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.RequestModels.UserCommands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EksiSozluk.Api.Application.Features.Commands.User
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {

        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetSingleAsync(u => u.EmailAddress == request.EmailAddress);

            if (dbUser == null) 
                throw new DatabaseValidationException("User not found.");

            string password = PasswordEncryptor.Encrypt(request.Password);
            if (PasswordEncryptor.Encrypt(dbUser.Password) != password)
                throw new DatabaseValidationException("Password is wrong!");

            if (!dbUser.EmailConfirmed)
                throw new DatabaseValidationException("Email adress is not confirmed!");

            LoginUserViewModel result = mapper.Map<LoginUserViewModel>(dbUser);

            Claim[] claims = 
            {
              new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
              new Claim(ClaimTypes.Email, dbUser.EmailAddress),
              new Claim(ClaimTypes.Name, dbUser.UserName),
              new Claim(ClaimTypes.GivenName, dbUser.FirstName),
              new Claim(ClaimTypes.Surname, dbUser.LastName)
            };

            result.Token = GenerateToken(claims);

            return result;
        }


        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims,
                                             expires: expiry,
                                             signingCredentials: creds,
                                             notBefore: DateTime.Now);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
