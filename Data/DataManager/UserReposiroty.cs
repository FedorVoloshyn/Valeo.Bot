using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using ValeoBot;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;

namespace ValeoBot.Data.DataManager
{
    public class UserReposiroty : IDataRepository<User>
    {
        private readonly ILogger<UserReposiroty> _logger;
        private readonly ApplicationDbContext _context;

        public UserReposiroty(ApplicationDbContext context, ILogger<UserReposiroty> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<User> All()
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
        public User Get(long id)
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

        public User Add(User entity)
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

        public void Update(User entity)
        {
            try
            {
                _context.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update User: {entity.ToString()}.");
                throw;
            }
        }

        public void Delete(User entity)
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
        public User[] Find(Func<User, bool> predicator)
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