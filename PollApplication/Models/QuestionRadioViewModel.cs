using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class QuestionRadioViewModel
    {
        public int QuestionId { get; set; }

        public string Question { get; set; }

        public List<Variant> Variants { get; set; }
        [Required]
        public int AnswerId { get; set; }
    }
}
