﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Models.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;

        public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
        }

        public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = entryRepository.AsQueryable();

            if (request.TodaysEntries)
            {
                query=query.Where(q=>q.CreateDate>=DateTime.Now.Date)
                           .Where(q=>q.CreateDate<=DateTime.Now.AddDays(1).Date);
            }

            query = query.Include(q=>q.EntryComments)
                 .OrderBy(q=>Guid.NewGuid())
                 .Take(request.Count);

            return await query.ProjectTo<GetEntriesViewModel>(mapper.ConfigurationProvider)
                              .ToListAsync(cancellationToken);
        }
    }
}
