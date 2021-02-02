using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class StatViewModel
    {
        public string QuestionText { get; set; }
        public int Id { get; set; }
        public List<SimpleReportViewModel> Models { get; set; }
    }
}
