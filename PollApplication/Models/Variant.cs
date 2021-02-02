using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        public int VariantIndex { get; set; }

        public Variant()
        {
        }
    }
}
