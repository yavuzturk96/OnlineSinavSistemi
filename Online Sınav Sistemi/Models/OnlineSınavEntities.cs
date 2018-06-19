namespace Online_Sınav_Sistemi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OnlineSınavEntities : DbContext
    {
        public OnlineSınavEntities()
            : base("name=OnlineSınavEntities1")
        {
        }

        public virtual DbSet<Bolum> Bolum { get; set; }
        public virtual DbSet<Ders> Ders { get; set; }
        public virtual DbSet<Donem> Donem { get; set; }
        public virtual DbSet<Konu> Konu { get; set; }
        public virtual DbSet<Sınav> Sınav { get; set; }
        public virtual DbSet<SınavaSoruEkleme> SınavaSoruEkleme { get; set; }
        public virtual DbSet<Sonuc> Sonuc { get; set; }
        public virtual DbSet<Soru> Soru { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Uye> Uye { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
