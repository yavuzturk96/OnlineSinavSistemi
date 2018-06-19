using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Sınav_Sistemi.Models
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public int UyeId { get; set; }
        public int SoruId { get; set; }
        public int SecilenCevapId { get; set; }

        public int DogruCevapMı { get; set; }
        public List<Cevap> Answers { get; set; }
    }
}