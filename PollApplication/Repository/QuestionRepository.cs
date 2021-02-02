using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PollApplication.Models;

namespace PollApplication.Repository
{
    public class QuestionRepository : IRepository<Question>
    {
        private ApplicationContext _context;
        public QuestionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Create(Question item)
        {
            _context.Questions.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Question p = _context.Questions.Find(id);
            if(p!=null)
                _context.Questions.Remove(p);
        }

        public Question GetBook(int id)
        {
            return _context.Questions.Find(id);
        }

        public IEnumerable<Question> GetBookList()
        {
            return _context.Questions.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Question item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
