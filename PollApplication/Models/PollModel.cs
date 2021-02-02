using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PollApplication.Models
{
    public class PollModel
    {
        [DisplayName("Название опроса")]
        [Required(ErrorMessage = "Не указано название опроса.")]
        public string Name { get; set; }

        [DisplayName("Тип опроса")]
        [Required(ErrorMessage = "Не указан тип опроса")]
        public PollType Type { get; set; }

        [DisplayName("Количество вопросов")]
        [Required(ErrorMessage = "Не указан тип опроса")]
        public int Count { get; set; }
    }

    public enum PollType { Check, Radio}
}
