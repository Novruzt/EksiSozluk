using EksiSozluk.Common.Models.Page;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.Infrastructure.Extensions;
public static class PageExtensions
{
    public static async Task<PagedViewModel<T>> GetPagedAsync<T>(this IQueryable<T> query, 
                                                                      int currentPage,
                                                                      int pageSize) where T : class
    {
        int count = await query.CountAsync();

        Page paging = new(currentPage, pageSize, count);

        ImmutableList<T> data = query.Skip(paging.Skip).Take(paging.PageSize).AsNoTracking().ToImmutableList();
       
        return new PagedViewModel<T>(data, paging);
    }
}