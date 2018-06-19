using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Sınav_Sistemi.Models
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int Point { get; set; }
        public string Title { get; set; }
        public List<Secenek> Choices { get; set; }
        public int SelectedChoiceId { get; set; }
        public int RightChoiceId { get; set; }
        public List<Cevap> Answers { get; set; }
        public byte[] Image { get; set; }
    }
}