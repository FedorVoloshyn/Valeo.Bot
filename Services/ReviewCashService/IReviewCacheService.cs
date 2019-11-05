namespace Valeo.Bot.Services.ReviewCashService
{
    public interface IReviewCacheService
    {
        void AddReviewText(long chatId, string text);
        void AddReview(long chatId);
        void ClearReview(long chatId);
        bool HasUnfinishedReview(long chatId);
    }
}