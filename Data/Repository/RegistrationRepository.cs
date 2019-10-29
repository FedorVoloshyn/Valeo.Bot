using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ValeoBot.Data.Entities;

namespace ValeoBot.Data.Repository
{
    public class RegistrationRepository : IDataRepository<Registration>
    {
        private readonly ILogger<RegistrationRepository> _logger;
        private readonly ApplicationDbContext _context;

        public RegistrationRepository(
            ApplicationDbContext context, 
            ILogger<RegistrationRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<Registration> All()
        { 
            try
            {
                return _context.Registrations.ToList(); 
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on get All Registrations.");
                throw;
            }
        } 
        public Registration Get(long id)
        {
            try
            {
                var res = _context.Registrations
                    .FirstOrDefault(e => e.Id == id);
                return res;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Get Registration: {id}.");
                throw;
            }
        }

        public Registration Add(Registration entity)
        {
            try
            {
                var value = Get(entity.Id);
                if (value == null)
                {
                    var result = _context.Registrations.Add(entity);
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
                _logger.LogError(e, $"Error on Add new Registration: {entity.ToString()}.");
                throw;
            }
        }

        public void Update(Registration entity)
        {
            try
            {
                _context.Registrations.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update Registration: {entity.ToString()}.");
                throw;
            }
        }

        public void Delete(Registration entity)
        {
            try
            {
                _context.Registrations.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Delete Registration: {entity.ToString()}.");
                throw;
            }
        }
        public Registration[] Find(Func<Registration, bool> predicator)
        {
            try
            {
                return _context.Registrations.Where(predicator).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Find Registration: {predicator.ToString()}.");
                throw;
            }
        }
    }
}