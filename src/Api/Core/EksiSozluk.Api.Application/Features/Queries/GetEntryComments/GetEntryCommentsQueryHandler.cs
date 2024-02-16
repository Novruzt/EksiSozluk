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

namespace EksiSozluk.Api.Application.Features.Queries.GetEntryComments;
public class GetEntryCommentsQueryHandler : IRequestHandler<GetEntryCommentsQuery, PagedViewModel<GetEntryCommentsViewModel>>
{
    private readonly IEntryCommentRepository entryCommentRepository;
    public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
    {
        this.entryCommentRepository = entryCommentRepository;
    }

    public async Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request, CancellationToken cancellationToken)
    {
        var query = entryCommentRepository.AsQueryable();
        query = query.Include(q => q.EntryCommentFavorites)
                     .Include(q => q.CreatedBy)
                     .Include(q => q.EntryCommentVotes)
                     .Where(q=>q.EntryId==request.EntryId);

        var list = query.Select(i => new GetEntryCommentsViewModel()
        {
            Id = i.Id,
            Content = i.Content,
            IsFavorited = request.UserId.HasValue && i.EntryCommentFavorites.Any(j => j.CreatedById == request.UserId),
            FavoritedCount = i.EntryCommentFavorites.Count,
            CreatedDate = i.CreateDate,
            CreatedByUserName = i.CreatedBy.UserName,
            VoteType =
                 request.UserId.HasValue && i.EntryCommentVotes.Any(j => j.CreatedById == request.UserId)
                 ? i.EntryCommentVotes.FirstOrDefault(j => j.CreatedById == request.UserId).VoteType
                 : Common.ViewModels.VoteType.None,
        });

       PagedViewModel<GetEntryCommentsViewModel> entries = await list.GetPagedAsync(request.Page, request.PageSize);

        return entries;
    }
}
