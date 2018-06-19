namespace Online_Sınav_Sistemi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sonuc")]
    public partial class Sonuc
    {
        public int SonucID { get; set; }

        public int? SınavID { get; set; }

        public int? UyeID { get; set; }

        public int? Dogru { get; set; }

        public int? Yanlıs { get; set; }

        public int? Puan { get; set; }

        public virtual Sınav Sınav { get; set; }

        public virtual Uye Uye { get; set; }
    }
}
