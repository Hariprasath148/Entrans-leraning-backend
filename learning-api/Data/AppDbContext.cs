using learning_api.Models;
using Microsoft.EntityFrameworkCore;

namespace learning_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<QuestionPaper> QuestionPaper { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<QuestionAttempt> QuestionAttempts { get; set; }
        public DbSet<UserQuestionPaperAnswer> QuestionPaperAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuestionPaper>()
                .HasMany(Q => Q.Questions)
                .WithMany(QP => QP.QuestionPapers);

            modelBuilder.Entity<QuestionAttempt>()
                .HasOne(U => U.User)
                .WithMany()
                .HasForeignKey(U => U.UserId);

            modelBuilder.Entity<QuestionAttempt>()
                .HasOne(QP => QP.QuestionPaper)
                .WithMany()
                .HasForeignKey(QP => QP.QuestionPaperId);

            modelBuilder.Entity<UserQuestionPaperAnswer>()
                .HasOne(QA => QA.QuestionAttempt)
                .WithMany(UA => UA.UserQuestionPaperAnswers)
                .HasForeignKey(UA => UA.QuestionAttemptId);


            //modelBuilder.Entity<User>()
            //    .HasOne(U => U.QuestionPaper)
            //    .WithOne(QP => QP.User)
            //    .HasForeignKey<QuestionPaper>(QP => QP.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<QuestionPaper>()
            //    .HasMany(QP => QP.Questions)
            //    .WithMany(Q => Q.QuestionPapers);
        }
    } 
}
