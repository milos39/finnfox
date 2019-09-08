using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace finnfox.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [StringLength(256)]
        public string UserLastName { get; set; }
        public ICollection<RacunovodstvenaPromena> RacunovodstvenaPromenas { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {


        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("UserLastName", this.UserLastName.ToString()));
            userIdentity.AddClaim(new Claim("UserID", this.Id) );
            //userIdentity.AddClaim(new Claim("RacunovodstvenaPromena", this.promena));


            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<RacunovodstvenaPromena> RacunovodstvenaPromenas { get; set; }
        public virtual DbSet<TipRacunovodstvenePromene> TipRacunovodstvenePromenes { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RacunovodstvenaPromena>()
              .HasRequired(m => m.ApplicationUser)
              .WithMany(m => m.RacunovodstvenaPromenas)
              .HasForeignKey<string>(m => m.ApplicationUserId);
            
        }
    }
}