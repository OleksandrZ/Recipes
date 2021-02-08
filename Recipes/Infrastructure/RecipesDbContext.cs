using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;

namespace Recipes.Infrastructure
{
    public class RecipesDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeFavorite> RecipeFavorites { get; set; }
        public DbSet<FollowedUsers> FollowedUsers { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Photo> Images { get; set; }
        public RecipesDbContext(DbContextOptions options) : base(options)
        {
        }

        public RecipesDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Recipes;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RecipeFavorite>(b =>
            {
                b.HasKey(t => new { t.RecipeId, t.UserId });

                b.HasOne(pt => pt.Recipe)
                    .WithMany(p => p.RecipeFavorites)
                    .HasForeignKey(pt => pt.RecipeId);
                b.HasOne(pt => pt.User)
                    .WithMany(p => p.RecipeFavorites)
                    .HasForeignKey(pt => pt.UserId);
            });

            builder.Entity<FollowedUsers>(b =>
            {
                b.HasKey(t => new { t.ObserverId, t.TargetId });

                // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider,
                // app.ApplicationServices.GetRequiredService<ConduitContext>().Database.EnsureCreated(); throws the following error:
                // System.Data.SqlClient.SqlException
                // HResult = 0x80131904
                // Message = Introducing FOREIGN KEY constraint 'FK_FollowedPeople_Persons_TargetId' on table 'FollowedPeople' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
                // Could not create constraint or index. See previous errors.
                b.HasOne(pt => pt.Observer)
                    .WithMany(p => p.Followers)
                    .HasForeignKey(pt => pt.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                // we need to add OnDelete RESTRICT otherwise for the SqlServer database provider,
                // app.ApplicationServices.GetRequiredService<ConduitContext>().Database.EnsureCreated(); throws the following error:
                // System.Data.SqlClient.SqlException
                // HResult = 0x80131904
                // Message = Introducing FOREIGN KEY constraint 'FK_FollowingPeople_Persons_TargetId' on table 'FollowedPeople' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
                // Could not create constraint or index. See previous errors.
                b.HasOne(pt => pt.Target)
                    .WithMany(t => t.Following)
                    .HasForeignKey(pt => pt.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}