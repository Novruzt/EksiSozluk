using EksiSozluk.Common.ViewModels;
using EksiSozluk.WebApp.Infastructure.Services.Interfaces;

namespace EksiSozluk.WebApp.Infastructure.Services;

public class VoteService : IVoteService
{
    private readonly HttpClient client;

    public VoteService(HttpClient client)
    {
        this.client = client;
    }

    public async Task DeleteEntryVote(Guid entryId)
    {
        await client.PostAsync($"/api/Vote/DeleteEntryVote/{entryId}", null);
    }

    public async Task DeleteEntryCommentVote(Guid entryCommentId)
    {
        await client.PostAsync($"/api/Vote/DeleteEntryCommentVote/{entryCommentId}", null);
    }

    public async Task CreateEntryUpVote(Guid entryId)
    {
        await CreateEntryVote(entryId, VoteType.UpVote);
    }

    public async Task CreateEntryDownVote(Guid entryId)
    {
        await CreateEntryVote(entryId, VoteType.DownVote);
    }

    public async Task CreateEntryCommentUpVote(Guid entryCommentId)
    {
        await CreateEntryCommentVote(entryCommentId, VoteType.UpVote);
    }

    public async Task CreateEntryCommentDownVote(Guid entryCommentId)
    {
        await CreateEntryCommentVote(entryCommentId, VoteType.DownVote);
    }

    private async Task<HttpResponseMessage> CreateEntryVote(Guid entryId, VoteType voteType = VoteType.UpVote)
    {
        HttpResponseMessage result = await client.PostAsync($"/api/vote/entry/{entryId}?voteType={voteType}", null);

        return result;
    }

    private async Task<HttpResponseMessage> CreateEntryCommentVote(Guid entryCommentId, VoteType voteType = VoteType.UpVote)
    {
        HttpResponseMessage result = await client.PostAsync($"/api/vote/entrycomment/{entryCommentId}?voteType={voteType}", null);

        return result;
    }
}
