using EksiSozluk.Common.Models.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.SearchBySubject;
public class SearchEntryQueryValidation:AbstractValidator<SearchEntryQuery>
{
    public SearchEntryQueryValidation()
    {
        RuleFor(i => i.SearchText)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3).WithMessage("SearchText must be at least 3 characters long.");
    }
}