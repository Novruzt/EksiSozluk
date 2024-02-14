using EksiSozluk.Common.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.User
{
    public class LoginUserCommandValidator:AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(i => i.EmailAddress)
                .NotNull()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} not a valid email adress");

            RuleFor(i => i.Password)
                .NotNull()
                .MinimumLength(6)
                .WithMessage("{PropertyName} cannot be less than 6 characters.");
        }
    }
}
