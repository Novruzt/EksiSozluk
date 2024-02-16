using AutoMapper;
using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetEntryDetails;
public class GetEntryDetailQueryHandler : IRequestHandler<GetEntryDetailsQuery, GetEntryDetailsViewModel>
{
    private readonly IEntryRepository entryRepository;

    public GetEntryDetailQueryHandler(IEntryRepository entryRepository, IMapper mapper)
    {
        this.entryRepository = entryRepository;
    }

    public async Task<GetEntryDetailsViewModel> Handle(GetEntryDetailsQuery request, CancellationToken cancellationToken)
    {
        var query = entryRepository.AsQueryable();

        query = query.Include(i => i.EntryFavorites)
                     .Include(i => i.CreatedBy)
                     .Include(i => i.EntryVotes)
                     .Where(i => i.Id == request.EntryId);

        var list = query.Select(i => new GetEntryDetailsViewModel()
        {
            Id = i.Id,
            Subject = i.Subject,
            Content = i.Content,
            IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j => j.CreatedById == request.UserId),
            FavoritedCount = i.EntryFavorites.Count,
            CreatedDate = i.CreateDate,
            CreatedByUserName = i.CreatedBy.UserName,
            VoteType =
                    request.UserId.HasValue && i.EntryVotes.Any(j => j.CreatedById == request.UserId)
                    ? i.EntryVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                    : Common.ViewModels.VoteType.None
        });

        return await list.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
