using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class PollViewModel
    {
        public string SearchString { get; set; }
        public List<Poll> Polls { get; set; }
    }
}
