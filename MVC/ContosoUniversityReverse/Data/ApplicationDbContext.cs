using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ContosoUniversityReverse.Models;

namespace ContosoUniversityReverse.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<Instructor> Instructors { get; set; } = null!;
        public virtual DbSet<OfficeAssignment> OfficeAssignments { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseId).ValueGeneratedNever();

                entity.Property(e => e.DepartmentId).HasDefaultValueSql("((1))");

                entity.HasMany(d => d.Instructors)
                    .WithMany(p => p.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "CourseAssignment",
                        l => l.HasOne<Instructor>().WithMany().HasForeignKey("InstructorId"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("CourseId"),
                        j =>
                        {
                            j.HasKey("CourseId", "InstructorId");

                            j.ToTable("CourseAssignment");

                            j.HasIndex(new[] { "InstructorId" }, "IX_CourseAssignment_InstructorID");

                            j.IndexerProperty<int>("CourseId").HasColumnName("CourseID");

                            j.IndexerProperty<int>("InstructorId").HasColumnName("InstructorID");
                        });
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasBaseType<Person>();
                // entity.Property(e => e.Id).ValueGeneratedNever();
                
                // entity.HasOne(d => d.IdNavigation)
                //     .WithOne(p => p.Instructor)
                //     .HasForeignKey<Instructor>(d => d.Id)
                //     .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<OfficeAssignment>(entity =>
            {
                entity.Property(e => e.InstructorId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasBaseType<Person>();
                // entity.Property(e => e.Id).ValueGeneratedNever();

                // entity.HasOne(d => d.IdNavigation)
                //     .WithOne(p => p.Student)
                //     .HasForeignKey<Student>(d => d.Id)
                //     .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
