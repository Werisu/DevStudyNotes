using DevStudyNotes.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevStudyNotes.API.Persistnece
{
    public class StudyNoteDbContext : DbContext
    {
        private readonly DbContextOptions<StudyNoteDbContext> _options;

        public StudyNoteDbContext(DbContextOptions<StudyNoteDbContext> options) : base(options)
        {
            this._options = options;
        }

        public DbSet<StudyNote> StudyNotes { get; set; }
        public DbSet<StudyNoteReaction> StudyNoteReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudyNote>(e =>
            {
                e.HasKey(s => s.Id);

                e.HasMany(s => s.Reactions)
                .WithOne()
                .HasForeignKey(s => s.StudyNoteId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<StudyNoteReaction>(sn =>
            {
                sn.HasKey(s => s.Id);
            });
        }
    }
}
