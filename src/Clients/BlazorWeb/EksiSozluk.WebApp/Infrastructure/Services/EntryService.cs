using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.Page;
using EksiSozluk.Common.Models.RequestModels.EntryCommands;
using EksiSozluk.Common.Models.RequestModels.EntryCommentCommands;
using System.Net.Http.Json;
using EksiSozluk.WebApp.Infrastructure.Services.Interfaces;

namespace EksiSozluk.WebApp.Infrastructure.Services;

public class EntryService : IEntryService
{
    private readonly HttpClient client;

    public EntryService(HttpClient client)
    {
        this.client = client;
    }


    public async Task<List<GetEntriesViewModel>> GetEntires()
    {
        List<GetEntriesViewModel>? result = await client.GetFromJsonAsync<List<GetEntriesViewModel>>("/api/entry?todaysEnties=false&count=30");

        return result;
    }

    public async Task<GetEntryDetailsViewModel> GetEntryDetail(Guid entryId)
    {
        GetEntryDetailsViewModel? result = await client.GetFromJsonAsync<GetEntryDetailsViewModel>($"/api/entry/{entryId}");
        return result;
    }

    public async Task<PagedViewModel<GetEntryDetailsViewModel>> GetMainPageEntries(int page, int pageSize)
    {
        PagedViewModel<GetEntryDetailsViewModel>? result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailsViewModel>>($"/api/entry/mainpageentries?page={page}&pageSize={pageSize}");
        return result;
    }

    public async Task<PagedViewModel<GetEntryDetailsViewModel>> GetProfilePageEntries(int page, int pageSize, string userName = null)
    {
        PagedViewModel<GetEntryDetailsViewModel>? result = await client.GetFromJsonAsync<PagedViewModel<GetEntryDetailsViewModel>>($"/api/entry/UserEntries?userName={userName}&page={page}&pageSize={pageSize}");

        return result;
    }

    public async Task<PagedViewModel<GetEntryCommentsViewModel>> GetEntryComments(Guid entryId, int page, int pageSize)
    {
        PagedViewModel<GetEntryCommentsViewModel>? result = await client.GetFromJsonAsync<PagedViewModel<GetEntryCommentsViewModel>>($"/api/entry/comments/{entryId}?page={page}&pageSize={pageSize}");

        return result;
    }


    public async Task<Guid> CreateEntry(CreateEntryCommand command)
    {
        HttpResponseMessage res = await client.PostAsJsonAsync("/api/Entry/CreateEntry", command);

        if (!res.IsSuccessStatusCode)
            return Guid.Empty;

        var guidStr = await res.Content.ReadAsStringAsync();

        return new Guid(guidStr.Trim('"'));
    }

    public async Task<Guid> CreateEntryComment(CreateEntryCommentCommand command)
    {
        HttpResponseMessage res = await client.PostAsJsonAsync("/api/Entry/CreateEntryComment", command);

        if (!res.IsSuccessStatusCode)
            return Guid.Empty;

        string guidStr = await res.Content.ReadAsStringAsync();

        return new Guid(guidStr.Trim('"'));
    }

    public async Task<List<SearchEntryViewModel>> SearchBySubject(string searchText)
    {
        List<SearchEntryViewModel>? result = await client.GetFromJsonAsync<List<SearchEntryViewModel>>($"/api/entry/Search?searchText={searchText}");

        return result;
    }
}
