using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IOptions<SMTPConnection> _smtpConnection;
        private readonly IMailingService _mailingService;
        private readonly IHostingEnvironment _env;

        public ReviewCacheService(ILogger<ReviewCacheService> logger, IOptions<SMTPConnection> smtpConnection, IMailingService mailingService, IHostingEnvironment env)
        {
            _logger = logger;
            _smtpConnection = smtpConnection;
            _mailingService = mailingService;
            _env = env;
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
                if(!_env.IsDevelopment())
                    await _mailingService.SendEmailAsync(review);
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