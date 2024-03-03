using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using StrongmanApp.Models;
using System;
namespace StrongmanApp.Models
{
    public class SportDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }




        

        public virtual DbSet<Competition> Competitions { get; set; }

        public virtual DbSet<CompetitionEvent> CompetitionEvents { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Event> Events { get; set; }

        public virtual DbSet<EventVideo> EventVideos { get; set; }

        public virtual DbSet<Federation> Federations { get; set; }

        public virtual DbSet<Lineup> Lineups { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<Result> Results { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Video> Videos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDbFunction(typeof(SportDbContext).GetMethod(nameof(ResultsTVF))).HasName("TableRes");

            modelBuilder.Entity<ResultsTVF>(entity => entity.HasKey(x => x.ID));
            /*modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("country_soft_delete"));
            });
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("event_soft_delete"));

               
            });*/
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            
            modelBuilder.Entity<Federation>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("federation_soft_delete"));
                



            });
            
            modelBuilder.Entity<News>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("NewsUpdate"));


            });
            modelBuilder.Entity<Result>(entity =>
            {
               
                entity.ToTable(tb =>
                {
                    tb.HasTrigger("Results_Insert");
                    tb.HasTrigger("Results_Update");
                });
               
            });
            modelBuilder.Entity<User>(entity => entity.ToTable(tb =>
            {
                tb.HasTrigger("User_last_updated");
                tb.HasTrigger("Users_age_update");
                tb.HasTrigger("user_soft_delete");
            }));

            modelBuilder.Entity<Competition>(entity => 
            entity.HasOne(d => d.Federation).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.FederationId)
                .HasConstraintName("FK_Competitions_Federations").OnDelete(DeleteBehavior.Cascade));
        }
       
        public IQueryable<ResultsTVF> ResultsTVF(int competition_id)
     => FromExpression(() => ResultsTVF(competition_id));
        //public DbSet<StrongmanApp.Models.UserRoles> AspNetUserRoles { get; set; } = default!;
        //public DbSet<StrongmanApp.Models.Role> AspNetRole { get; set; } = default!;
        //public DbSet<AspNetRole> AspNetRole { get; set; }=default!
    }
}