using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ValeoBot;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;

namespace ValeoBot.Data.DataManager
{
    public class UserManager : IDataRepository<User>
    {
        readonly ApplicationDbContext _context;

        public IEnumerable<User> All => _context.Users.ToList();
        public UserManager(ApplicationDbContext context)
        {
            _context = context;
        }
 
        public User Get(long id)
        {
            return _context.Users
                  .FirstOrDefault(e => e.Id == id);
        }
 
        public User Add(User entity)
        {
            try
            {
                var result = _context.Users.Add(entity);
                _context.SaveChanges();
                return result.Entity;
            }
            catch(Exception ex)
            {
                var result = _context.Users
                  .FirstOrDefault(e => e.Id == entity.Id);
                  return result;
            }
        }
 
        public void Update(User user)
        { 
            _context.Users.Update(user);
            _context.SaveChanges();
        }
 
        public void Delete(User User)
        {
            _context.Users.Remove(User);
            _context.SaveChanges();
        }

        public User[] Find(Func<User, bool> predicator)
        {
            return _context.Users.Where(predicator).ToArray();
        }
    }
}