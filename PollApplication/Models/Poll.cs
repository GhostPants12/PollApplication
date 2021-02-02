using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Question> Questions { get; set; }
        public int Count { get; set; }
        public PollType Type { get; set; }

        public Poll()
        {
        }
    }
}
