using AzureWebsite.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzureWebsite.Api
{
    public class NotesDb : DbContext
    {
        public NotesDb(DbContextOptions<NotesDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureNoteEntity(modelBuilder.Entity<Note>());
        }

        private void ConfigureNoteEntity(EntityTypeBuilder<Note> entity)
        {
            entity.ToTable("Notes");


            entity.Property(p => p.Title).HasMaxLength(100).IsRequired();
            entity.Property(p => p.Content).IsRequired(false);
            entity.Property(p => p.CreatedOn).IsRequired();
            entity.Property(p => p.UpdatedOn).IsRequired(false);

        }

        public DbSet<Note> Notes { get; protected set; }
    }
}