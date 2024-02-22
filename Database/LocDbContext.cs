using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Translator_Project_Management.Models.Database;
using File = Translator_Project_Management.Models.Database.File;

namespace Translator_Project_Management.Database
{
    public class LocDbContext : IdentityDbContext<User>
	{
        public LocDbContext(DbContextOptions<LocDbContext> options)
            :base(options)
        {            
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Models.Database.File> Files { get; set; }

        public DbSet<Client> Clients { get; set; }
        
        public DbSet<SourceLine> SourceLines { get; set; }

        public DbSet<TransLine> TranslationLines { get; set; }

        public DbSet<Language> Languages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure Project and File relationship
			modelBuilder.Entity<Project>()
				.HasMany(p => p.Files)
				.WithOne(f => f.Project)
				.HasForeignKey(f => f.ProjectId);

			// Configure File and Line relationships
			modelBuilder.Entity<File>()
				.HasMany(f => f.SourceLines)
				.WithOne(l => l.File)
				.HasForeignKey(l => l.FileId);

			modelBuilder.Entity<File>()
				.HasMany(f => f.TranslationLines)
				.WithOne(l => l.File)
				.HasForeignKey(l => l.FileId);

			//modelBuilder.Entity<UserSourceLineMapping>()
			//.HasKey(m => new { m.UserId, m.SourceLineId });

			//modelBuilder.Entity<UserSourceLineMapping>()
			//	.HasOne(m => m.User)
			//	.WithMany(u => u.SourceLines)
			//	.HasForeignKey(m => m.UserId);

			//modelBuilder.Entity<UserSourceLineMapping>()
			//	.HasOne(m => m.SourceLine)
			//	.WithMany(s => s.Users)
			//	.HasForeignKey(m => m.SourceTextId);
		}
	}
}
