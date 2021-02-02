using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PollApplication.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<Variant> Variants { get; set; }
        public int? LinkedPollId { get; set; }
        [ForeignKey("LinkedPollId")]
        public Poll LinkedPoll { get; set; }
        public int QuestionIndex { get; set; }

        public Question()
        {
            Variants = new List<Variant>();
        }
    }
}
