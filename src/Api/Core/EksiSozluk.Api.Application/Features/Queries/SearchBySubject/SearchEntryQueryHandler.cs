using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EksiSozluk.Api.Application.Features.Queries.SearchBySubject;
public class SearchEntryQueryHandler : IRequestHandler<SearchEntryQuery, List<SearchEntryViewModel>>
{
    private readonly IEntryRepository entryRepository;

    public SearchEntryQueryHandler(IEntryRepository entryRepository)
    {
        this.entryRepository = entryRepository;
    }
    public async Task<List<SearchEntryViewModel>> Handle(SearchEntryQuery request, CancellationToken cancellationToken)
    {
        var result = entryRepository
                    .Get(e => EF.Functions.Like(e.Subject, $"%{request.SearchText}%"))
                    .Select(e=>new SearchEntryViewModel()
                    {
                        Id=e.Id,
                        Subject=e.Subject
                    });

        return await result.ToListAsync(cancellationToken);
    }
}
