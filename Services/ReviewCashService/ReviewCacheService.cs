using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ValeoBot.Configuration;

namespace Valeo.Bot.Services.ReviewCashService
{
    public class ReviewCacheService : IReviewCacheService
    {
        
        #region Cache
        private static readonly List<Feedback> _reviewCache = new List<Feedback>();
        #endregion
        private readonly ILogger<ReviewCacheService> _logger;
        private readonly ILogger<MailingService> _emaillogger;
        private readonly IOptions<SMTPConnection> _stmpConnection;

        public ReviewCacheService(ILogger<ReviewCacheService> logger, IOptions<SMTPConnection> stmpConnection, ILogger<MailingService> emaillogger)
        {
            _logger = logger;
            _stmpConnection = stmpConnection;
            _emaillogger = emaillogger;
        }

        public void AddReview(long chatId)
        {
            var nonCompletedReview = _reviewCache.FirstOrDefault(v => v.ChatId == chatId);
            if(nonCompletedReview != null)
            {
                throw new AggregateException($"Trying to add trying to add review without complating previous, chatId: {chatId}.");
            }
            else
            {
                Feedback review = new Feedback() {ChatId = chatId};
                _reviewCache.Add(review);
            }
        }

        public async void AddReviewText(long chatId, string text)
        {            
            var review = _reviewCache.FirstOrDefault(v => v.ChatId == chatId);
            if (review == null)
            {
                throw new AggregateException($"Trying to add Text to null object, chatId: {chatId}, value: {text}.");
            }
            else
            {  
                review.Text = text;
                
                MailingService mailingService = new MailingService(_stmpConnection.Value);
                await mailingService.SendEmailAsync(review);

                _reviewCache.Remove(review);
            }
        }

        public void ClearReview(long chatId)
        {
            var review = _reviewCache.FirstOrDefault(n => n.ChatId == chatId);
            if(review != null)
            {
                _reviewCache.Remove(review);
            }
        }

        public bool HasUnfinishedReview(long chatId)
        {
            return _reviewCache.FirstOrDefault(review => review.ChatId == chatId) != null ? true : false;
        }
    }
}