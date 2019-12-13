using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Valeo.Bot;
using Valeo.Bot.Data.Entities;

namespace Valeo.Bot.Data.Repository
{
    public class FeedbackRepository : IDataRepository<Feedback>
    {
        private readonly ILogger<FeedbackRepository> _logger;
        private readonly ApplicationDbContext _context;

        public FeedbackRepository(
            ApplicationDbContext context, 
            ILogger<FeedbackRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<Feedback> All()
        {
            try
            {
                return _context.Feedbacks.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on get All Feedbacks.");
                throw;
            }
        }
        public Feedback Get(long id)
        {
            try
            {
                return _context.Feedbacks
                    .FirstOrDefault(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Get Feedback: {id}.");
                throw;
            }
        }

        public Feedback Add(Feedback entity)
        {
            try
            {
                var value = Get(entity.Id);
                if (value == null)
                {
                    var result = _context.Feedbacks.Add(entity);
                    _context.SaveChanges();
                    return result.Entity;
                }
                else
                {
                    var result = Get(entity.Id);
                    return result;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Add new Feedback: {entity.ToString()}.");
                throw;
            }
        }

        public void Update(Feedback entity)
        {
            try
            {
                _context.Feedbacks.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update Feedback: {entity.ToString()}.");
                throw;
            }
        }

        public void Delete(Feedback entity)
        {
            try
            {
                _context.Feedbacks.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Delete Feedback: {entity.ToString()}.");
                throw;
            }
        }
        public Feedback[] Find(Func<Feedback, bool> predicator)
        {
            try
            {
                return _context.Feedbacks.Where(predicator).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Find Feedback: {predicator.ToString()}.");
                throw;
            }
        }
    }
}