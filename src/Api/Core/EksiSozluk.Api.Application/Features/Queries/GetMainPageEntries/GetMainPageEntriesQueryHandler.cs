using AutoMapper;
using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Infastructure.Extensions;
using EksiSozluk.Common.Models.Page;
using EksiSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetMainPageEntries;
public class GetMainPageEntriesQueryHandler : IRequestHandler<GetMainPageEntriesQuery, PagedViewModel<GetEntryDetailViewModel>>
{
    private readonly IEntryRepository entryRepository;

    public GetMainPageEntriesQueryHandler(IEntryRepository entryRepository)
    {
        this.entryRepository = entryRepository;
    }
    public async Task<PagedViewModel<GetEntryDetailViewModel>> Handle(GetMainPageEntriesQuery request, CancellationToken cancellationToken)
    {
        var query = entryRepository.AsQueryable();
        query = query.Include(q => q.EntryFavorites)
                     .Include(q => q.CreatedBy)
                     .Include(q => q.EntryVotes);

        var list = query.Select(i => new GetEntryDetailViewModel()
        {
            Id = i.Id,
            Subject= i.Subject,
            Content= i.Content,
            IsFavorited=request.UserId.HasValue && i.EntryFavorites.Any(j=>j.CreatedById==request.UserId),
            FavoritedCount=i.EntryFavorites.Count,
            CreatedDate=i.CreateDate,
            CreatedByUserName=i.CreatedBy.UserName,
            VoteType =
                 request.UserId.HasValue && i.EntryVotes.Any(j=>j.CreatedById==request.UserId)
                 ?i.EntryVotes.FirstOrDefault(j=>j.CreatedById == request.UserId).VoteType 
                 :Common.ViewModels.VoteType.None,
        });

        PagedViewModel<GetEntryDetailViewModel> entries = await list.GetPagedAsync(request.Page, request.PageSize);

        return new PagedViewModel<GetEntryDetailViewModel>(entries.Results, entries.PageInfo); 
    }
}
