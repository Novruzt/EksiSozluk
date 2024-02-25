namespace EksiSozluk.WebApp.Infastructure.Services.Interfaces;

public interface IFavService
{
    Task CreateEntryCommentFav(Guid entryCommentId);
    Task CreateEntryFav(Guid entryId);
    Task DeleteEntryCommentFav(Guid entryCommentId);
    Task DeleteEntryFav(Guid entryId);
}