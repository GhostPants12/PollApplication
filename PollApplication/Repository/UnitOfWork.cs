using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PollApplication.Models;

namespace PollApplication.Repository
{
    public class UnitOfWork
    {
        private ApplicationContext _context;
        private PollRepository pr;
        private QuestionRepository qr;
        private VariantRepository vr;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            pr= new PollRepository(context);
            qr=new QuestionRepository(context);
            vr = new VariantRepository(context);
        }

        public PollRepository Polls => pr;

        public QuestionRepository Questions => qr;

        public VariantRepository Variants => vr;

 
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
