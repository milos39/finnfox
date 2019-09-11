namespace finnfox
{
    using finnfox.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RacunovodstvenaPromena")]
    public partial class RacunovodstvenaPromena
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PromenaId { get; set; }

        [Required(ErrorMessage = "polje {0} je obavezno")]
        [StringLength(50)]
        [Display(Name = "Naziv promene")]

        public string NazivPromene { get; set; }

        [Required(ErrorMessage = "polje {0} je obavezno")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString ="{dd/MM/yy}")]
        [Display(Name = "Datum promene")]
        public DateTime DatumPromene { get; set; }


        [Display(Name = "Tip promene")]
        public int TipPromeneId { get; set; }

        public virtual TipRacunovodstvenePromene TipRacunovodstvenePromene { get; set; }

        [Required]
        [StringLength(128)]
        public virtual string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "polje {0} je obavezno")]
        [Display(Name = "Kolicina novca")]
        [RegularExpression(@"^[-+]?[0-9]*\.?[0-9]+$", ErrorMessage = "{0} mora biti broj")]

        public double KolicinaNovca { get; set; }

        public string Valuta { get; set; }
        
 

    }
}
