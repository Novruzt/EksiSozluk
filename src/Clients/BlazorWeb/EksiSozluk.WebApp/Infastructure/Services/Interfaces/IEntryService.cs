using EksiSozluk.Common.Models.Page;
using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.RequestModels.EntryCommands;
using EksiSozluk.Common.Models.RequestModels.EntryCommentCommands;

namespace EksiSozluk.WebApp.Infastructure.Services.Interfaces;
public interface IEntryService
{
    Task<Guid> CreateEntry(CreateEntryCommand command);
    Task<Guid> CreateEntryComment(CreateEntryCommentCommand command);
    Task<List<GetEntriesViewModel>> GetEntires();
    Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize);
    Task<GetEntryDetailsViewModel> GetEntryDetail(Guid entryId);
    Task<PagedViewModel<GetEntryDetailsViewModel>> GetMainPageEntries(int page, int pageSize);
    Task<PagedViewModel<GetEntryDetailsViewModel>> GetProfilePageEntries(int page, int pageSize, string userName = null);
    Task<List<SearchEntryViewModel>> SearchBySubject(string searchText);
}