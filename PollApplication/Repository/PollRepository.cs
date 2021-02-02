using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PollApplication.Models;

namespace PollApplication.Repository
{
    public class PollRepository : IRepository<Poll>
    {
        private ApplicationContext _context;
        public PollRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Create(Poll item)
        {
            _context.Polls.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Poll p = _context.Polls.Find(id);
            if(p!=null)
                _context.Polls.Remove(p);
        }

        public Poll GetBook(int id)
        {
            return _context.Polls.Find(id);
        }

        public IEnumerable<Poll> GetBookList()
        {
            return _context.Polls.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Poll item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
