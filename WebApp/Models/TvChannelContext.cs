using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

public partial class TvChannelContext : DbContext
{
    public TvChannelContext()
    {
    }

    public TvChannelContext(DbContextOptions<TvChannelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appeal> Appeals { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<WeeklyProgramList> WeeklyProgramLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SUSSY-BAKA\\SQLEXPRESS;Initial Catalog=TV_Channel;Integrated Security=True;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appeal>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AppealPurpose)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("appealPurpose");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.Organization)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("organization");
            entity.Property(e => e.ProgramId).HasColumnName("programId");

            entity.HasOne(d => d.Program).WithMany(p => p.Appeals)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appeals_Appeals");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fullName");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("position");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.GenreId).HasColumnName("genreId");
            entity.Property(e => e.Length)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("length");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Genre).WithMany(p => p.Programs)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Programs_Genres");
        });

        modelBuilder.Entity<WeeklyProgramList>(entity =>
        {
            entity.ToTable("WeeklyProgramList");

            entity.HasIndex(e => new { e.WeekNumber, e.WeekMonth, e.WeekYear }, "IX_WeeklyProgramList");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.EmployeesId).HasColumnName("employeesId");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.Guests)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("guests");
            entity.Property(e => e.ProgramId).HasColumnName("programId");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
            entity.Property(e => e.WeekMonth).HasColumnName("weekMonth");
            entity.Property(e => e.WeekNumber).HasColumnName("weekNumber");
            entity.Property(e => e.WeekYear).HasColumnName("weekYear");

            entity.HasOne(d => d.Employees).WithMany(p => p.WeeklyProgramLists)
                .HasForeignKey(d => d.EmployeesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeeklyProgramList_Employees");

            entity.HasOne(d => d.Program).WithMany(p => p.WeeklyProgramLists)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeeklyProgramList_Programs");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
