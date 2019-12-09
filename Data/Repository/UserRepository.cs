using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Valeo.Bot.Data.Entities;

namespace Valeo.Bot.Data.Repository
{
    public class UserRepository : IDataRepository<ValeoUser>
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly ApplicationDbContext _context;

        public UserRepository(
            ApplicationDbContext context,
            ILogger<UserRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<ValeoUser> All()
        { 
            try
            {
                return _context.Users.ToList(); 
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on get All Users.");
                throw;
            }
        } 
        public ValeoUser Get(long id)
        {
            try
            {
                return _context.Users
                    .FirstOrDefault(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Get User: {id}.");
                throw;
            }
        }

        public ValeoUser Add(ValeoUser entity)
        {
            try
            {
                var value = Get(entity.Id);
                if (value == null)
                {
                    var result = _context.Users.Add(entity);
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
                _logger.LogError(e, $"Error on Add new User: {entity.ToString()}.");
                throw;
            }
        }

        public void Update(ValeoUser entity)
        {
            try
            {
                _context.Users.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update User: {entity.ToString()}.");
                throw;
            }
        }

        public void Delete(ValeoUser entity)
        {
            try
            {
                _context.Users.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Delete User: {entity.ToString()}.");
                throw;
            }
        }
        public ValeoUser[] Find(Func<ValeoUser, bool> predicator)
        {
            try
            {
                return _context.Users.Where(predicator).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Find User: {predicator.ToString()}.");
                throw;
            }
        }
    }
}