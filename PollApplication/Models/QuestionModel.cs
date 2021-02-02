using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class QuestionModel
    {
        public string Name { get; set; }
        [Required]
        public string Text { get; set; }
        public PollType Type { get; set; }
        public int Count { get; set; }
        [Required]
        public List<string> Variants { get; set; }
    }
}
