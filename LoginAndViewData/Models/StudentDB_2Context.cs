using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LoginAndViewData.Models
{
    public partial class StudentDB_2Context : DbContext
    {
        public StudentDB_2Context()
        {
        }

        public StudentDB_2Context(DbContextOptions<StudentDB_2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<MovieStudent> MovieStudents { get; set; } = null!;
        public virtual DbSet<StudentDetail> StudentDetail { get; set; } = null!;
        public virtual DbSet<StudentLogin> StudentLogins { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<SubjectStudent> SubjectStudents { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=STSPC89;Database=StudentDB_2;User Id=sa;Password=root@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.MovieName).HasMaxLength(50);
            });

            modelBuilder.Entity<MovieStudent>(entity =>
            {
                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieStudents)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovieStudents_Movies");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.MovieStudents)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MovieStudents_StudentDetail");
            });

            modelBuilder.Entity<StudentDetail>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.ToTable("StudentDetail");

                entity.Property(e => e.BloodGroup).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.FileName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.MobileNo).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<StudentLogin>(entity =>
            {
                entity.ToTable("StudentLogin");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.SubjectName).HasMaxLength(50);
            });

            modelBuilder.Entity<SubjectStudent>(entity =>
            {
                entity.ToTable("SubjectStudent");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.SubjectStudents)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectStudent_StudentDetail");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectStudents)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectStudent_Subjects");

                //entity.HasKey(ss => new { ss.StudentId,ss.SubjectId});
            });

            OnModelCreatingPartial(modelBuilder);
        }

        //public void DeleteAndReseed(int? idToDelete) {
        //    var entityToDelete = StudentDetail.Find(idToDelete);
        //    if (entityToDelete != null)
        //    {
        //        StudentDetail.Remove(entityToDelete);
        //        SaveChanges();
        //        var maxId = StudentDetail.Max(e => (int?)e.StudentId) ?? 0;

        //        Database.ExecuteSqlRaw($"DBCC CHECKIDENT('StudentDetail',RESEED,{maxId})");

        //    }
        //    else {
        //        Console.WriteLine("Entity Not found!");
        //    }
        //}
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
