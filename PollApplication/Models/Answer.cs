using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class PollAnswer
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User LinkedUser { get; set; }
        public int? QuestionId { get; set; }
        public int? VariantId { get; set; }
    }
}
