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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PromenaId { get; set; }

        [Required]
        [StringLength(50)]
        public string NazivPromene { get; set; }

        public DateTime DatumPromene { get; set; }

        public int TipPromeneId { get; set; }

        public virtual TipRacunovodstvenePromene TipRacunovodstvenePromene { get; set; }

        [Required]
        [StringLength(128)]
        public virtual string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


        public double KolicinaNovca { get; set; }

        public string Valuta { get; set; }
        
 

    }
}
