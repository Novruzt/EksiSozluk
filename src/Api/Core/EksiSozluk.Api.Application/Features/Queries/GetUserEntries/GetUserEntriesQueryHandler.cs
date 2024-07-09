using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.Page;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EksiSozluk.Common.Infrastructure.Extensions;

namespace EksiSozluk.Api.Application.Features.Queries.GetUserEntries;
public class GetUserEntriesQueryHandler : IRequestHandler<GetUserEntriesQuery, PagedViewModel<GetUserEntriesDetailsViewModel>>
{
    private readonly IEntryRepository entryRepository;

    public GetUserEntriesQueryHandler(IEntryRepository entryRepository)
    {
        this.entryRepository = entryRepository;
    }

    public async Task<PagedViewModel<GetUserEntriesDetailsViewModel>> Handle(GetUserEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = entryRepository.AsQueryable();

        if (request.UserId != null && request.UserId.HasValue && request.UserId != Guid.Empty)
            query = query.Where(i => i.CreatedById == request.UserId);
        else if (!string.IsNullOrEmpty(request.UserName))
            query = query.Where(i => i.CreatedBy.UserName == request.UserName);
        else
            return null;

        query = query.Include(i => i.EntryFavorites)
                     .Include(i => i.CreatedBy);

        var list = query.Select(i => new GetUserEntriesDetailsViewModel()
        {
            Id = i.Id,
            Subject = i.Subject,
            Content = i.Content,
            IsFavorited = false,
            FavoritedCount = i.EntryFavorites.Count,
            CreatedDate = i.CreateDate,
            CreatedByUserName = i.CreatedBy.UserName
        });

        PagedViewModel<GetUserEntriesDetailsViewModel> entries = await list.GetPagedAsync(request.Page, request.PageSize);

        return entries;
    }
}
