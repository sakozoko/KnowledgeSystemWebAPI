using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<AnswerEntity> Answers { get; set; } = null!;
    public DbSet<QuestionEntity> Questions { get; set; } = null!;
    public DbSet<TestEntity> Tests { get; set; } = null!;
    public DbSet<PassedTestEntity> PassedTests { get; set; } = null!;
    public DbSet<AnswerDumpEntity> AnswerDumps { get; set; } = null!;
    public DbSet<RoleEntity> Roles { get; set; } = null!;

    //data annotation
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region UserEntity

        modelBuilder.Entity<UserEntity>()
            .HasOne(c => c.Role)
            .WithMany()
            .HasForeignKey("RoleEntityId")
            .IsRequired();
        modelBuilder.Entity<UserEntity>()
            .Property(u => u.CreatedDate)
            .IsRequired();

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
        modelBuilder.Entity<TestEntity>()
            .HasOne(t => t.UserCreator)
            .WithMany(c => c.CreatedTests)
            .HasForeignKey("UserEntityCreatorId")
            .IsRequired();

        modelBuilder.Entity<TestEntity>()
            .Property(t => t.CreatedDate)
            .IsRequired();

        modelBuilder.Entity<TestEntity>()
            .Property(t => t.MaxMark)
            .HasColumnType("decimal(18,4)");

        #endregion

        #region QuestionEntity

        modelBuilder.Entity<QuestionEntity>()
            .Property(q => q.Text)
            .HasMaxLength(512)
            .IsRequired();
        modelBuilder.Entity<QuestionEntity>()
            .HasOne(q => q.Test)
            .WithMany(t => t.Questions)
            .HasForeignKey("TestEntityId")
            .IsRequired();
        
        modelBuilder.Entity<QuestionEntity>()
            .Property(q=>q.Mark)
            .HasColumnType("decimal(18,4)");

        #endregion

        #region AnswerEntity

        modelBuilder.Entity<AnswerEntity>()
            .Property(a => a.Text)
            .HasMaxLength(512)
            .IsRequired();

        modelBuilder.Entity<AnswerEntity>()
            .Property(a => a.IsCorrect)
            .IsRequired();

        modelBuilder.Entity<AnswerEntity>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey("QuestionEntityId")
            .IsRequired();

        #endregion

        #region PassedTestEntity

        modelBuilder.Entity<PassedTestEntity>()
            .Property(p => p.StartedDate)
            .IsRequired();

        modelBuilder.Entity<PassedTestEntity>()
            .Property<int>("UserEntityId");

        modelBuilder.Entity<PassedTestEntity>()
            .HasOne(pt => pt.Test)
            .WithMany()
            .HasForeignKey("TestEntityId");

        modelBuilder.Entity<PassedTestEntity>()
            .HasOne(pt => pt.User)
            .WithMany(u => u.PassedTests)
            .HasForeignKey("UserEntityId")
            .IsRequired();
        
        modelBuilder.Entity<PassedTestEntity>()
            .Property(pt=>pt.Mark)
            .HasColumnType("decimal(18,4)");

        #endregion

        #region RoleEntity

        modelBuilder.Entity<RoleEntity>()
            .HasIndex(r => r.Name)
            .IsUnique();

        #endregion

        #region AnswerDumpEntity

        modelBuilder.Entity<AnswerDumpEntity>()
            .HasOne(ad => ad.Answer)
            .WithMany()
            .HasForeignKey("AnswerEntityId");

        modelBuilder.Entity<AnswerDumpEntity>()
            .HasOne(ad => ad.Question)
            .WithMany()
            .HasForeignKey("QuestionEntityId");
        modelBuilder.Entity<AnswerDumpEntity>()
            .HasOne(ad => ad.PassedTest)
            .WithMany(pt => pt.Answers)
            .HasForeignKey("PassedTestEntityId")
            .IsRequired();

        #endregion
    }
}