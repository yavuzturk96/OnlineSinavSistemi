namespace Online_Sınav_Sistemi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SınavaSoruEkleme
    {
        public int SınavaSoruEklemeID { get; set; }

        public int? SınavID { get; set; }

        public int? SoruID { get; set; }

        public virtual Sınav Sınav { get; set; }

        public virtual Soru Soru { get; set; }
    }
}
