using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PollApplication.Models;

namespace PollApplication.Repository
{
    public class VariantRepository : IRepository<Variant>
    {
        private ApplicationContext _context;
        public VariantRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Create(Variant item)
        {
            _context.Variants.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Variant p = _context.Variants.Find(id);
            if(p!=null)
                _context.Variants.Remove(p);
        }

        public Variant GetBook(int id)
        {
            return _context.Variants.Find(id);
        }

        public IEnumerable<Variant> GetBookList()
        {
            return _context.Variants.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Variant item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
