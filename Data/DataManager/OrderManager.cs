using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValeoBot;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;

namespace ValeoBot.Data.DataManager
{
    public class OrderManager : IDataRepository<Order>
        {
            readonly ApplicationDbContext _context;

            public OrderManager(ApplicationDbContext context)
            {
                _context = context;
            }

            public IEnumerable<Order> All => _context.Orders.ToList();
            public Order Get(long id)
            {
                return _context.Orders
                    .FirstOrDefault(e => e.Id == id);
            }

            public Order Add(Order entity)
            {
                var value = Get(entity.Id);
                if(value == null) { 
                   var result = _context.Orders.Add(entity);
                    _context.SaveChanges();
                    return result.Entity;
                }
                else 
                { 
                    var result = Get(entity.Id);
                    return result;
                }
            }

            public void Update(Order entity)
            {
                _context.Update(entity);
                _context.SaveChanges();
            }

            public void Delete(Order entity)
            {
                try
                {
                    _context.Orders.Remove(entity);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
            public Order[] Find(Func<Order, bool> predicator)
            {
                return _context.Orders.Where(predicator).ToArray();
            }

        }
}