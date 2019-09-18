namespace finnfox
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipRacunovodstvenePromene")]
    public partial class TipRacunovodstvenePromene
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipRacunovodstvenePromene()
        {
            RacunovodstvenaPromenas = new HashSet<RacunovodstvenaPromena>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TipPromeneId { get; set; }

        [Required(ErrorMessage ="polje {0} je obavezno")]
        [StringLength(50)]
        [Display (Name ="Naziv tipa" )]
        public string NazivTipa { get; set; }

        [Display(Name ="Prihod/Rashod")]
        public bool PozitivnostTipa { get; set; }

       [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RacunovodstvenaPromena> RacunovodstvenaPromenas { get; set; }
    }
}
