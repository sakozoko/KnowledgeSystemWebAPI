using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<AnswerEntity> Answers { get; set; }
    public DbSet<QuestionEntity> Questions { get; set; }
    public DbSet<TestEntity> Tests { get; set; }
    public DbSet<PassedTestEntity> PassedTests { get; set; }
    public DbSet<AnswerDumpEntity> AnswerDumps { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    
    //data annotation
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region UserEntity

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<UserEntity>()
            .Property(c => c.Email)
            .HasMaxLength(320)
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Phone)
            .IsUnique();
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.Phone)
            .HasMaxLength(15)
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.FirstName)
            .HasMaxLength(64)
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.Surname)
            .HasMaxLength(64)
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.Password)
            .HasMaxLength(128)
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.UserName)
            .IsUnique();
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.UserName)
            .HasMaxLength(64)
            .IsRequired();

        #endregion

        #region TestEntity

        modelBuilder.Entity<TestEntity>()
            .Property(t => t.Description)
            .HasMaxLength(512)
            .IsRequired();
        modelBuilder.Entity<TestEntity>()
            .Property(t => t.Title)
            .HasMaxLength(128)
            .IsRequired();

        #endregion

        #region QuestionEntity
        
        modelBuilder.Entity<QuestionEntity>()
            .Property(q => q.Text)
            .HasMaxLength(512)
            .IsRequired();

        #endregion

        #region AnswerEntity

        modelBuilder.Entity<AnswerEntity>()
            .Property(a => a.Text)
            .HasMaxLength(512)
            .IsRequired();
        
        modelBuilder.Entity<AnswerEntity>()
            .Property(a => a.IsCorrect)
            .IsRequired();
        
        #endregion

        #region PassedTestEntity

        
        modelBuilder.Entity<PassedTestEntity>()
            .Property(p => p.StartedDate)
            .IsRequired();

        #endregion
        
        #region RoleEntity
        
        modelBuilder.Entity<RoleEntity>()
            .HasIndex(r => r.Name)
            .IsUnique();
        
        #endregion
    }

}
