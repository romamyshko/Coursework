using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DBControl.Models
{
    public partial class UniversityContext : DbContext
    {
        private readonly string _connectionSting;

        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {
        }

        public UniversityContext(DbConnectionInfo dbConnectionInfo)
        {
            _connectionSting = dbConnectionInfo.Connection;
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentSubject> StudentsSubjects { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionSting);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack")
                .HasAnnotation("Relational:Collation", "Ukrainian_Ukraine.1251");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.FacultyId).HasColumnName("faculty_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.FacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("faculty_forKey");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("faculties");

                entity.Property(e => e.FacultyId).HasColumnName("faculty_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("course_forKey");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasColumnName("fullname");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_forKey");
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.HasKey(e => e.StudentSubjectId)
                    .HasName("students_subjects_pkey");

                entity.ToTable("students_subjects");

                entity.Property(e => e.StudentSubjectId).HasColumnName("student_subject_id");

                entity.Property(e => e.Mark).HasColumnName("mark");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentsSubjects)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("student_forkey");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.StudentsSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("subject_forKey");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("subjects");

                entity.Property(e => e.SubjectId).HasColumnName("subject_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
