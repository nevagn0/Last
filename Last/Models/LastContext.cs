using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Last.Models;

public partial class LastContext : DbContext
{
    public LastContext()
    {
    }

    public LastContext(DbContextOptions<LastContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<Comm> Comms { get; set; }

    public virtual DbSet<Passport> Passports { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vacin> Vacins { get; set; }

    public virtual DbSet<Vetcin> Vetcins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Last;Username=postgres;Password=Misha1029!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("newtable_pk");

            entity.ToTable("animal");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Age)
                .HasColumnType("character varying")
                .HasColumnName("age");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Type).HasColumnType("character varying");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Animals)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("newtable_user_fk");
        });

        modelBuilder.Entity<Comm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comm_pk");

            entity.ToTable("comm");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Descrip)
                .HasColumnType("character varying")
                .HasColumnName("descrip");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Vetclinid).HasColumnName("vetclinid");

            entity.HasOne(d => d.User).WithMany(p => p.Comms)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("comm_user_fk");

            entity.HasOne(d => d.Vetclin).WithMany(p => p.Comms)
                .HasForeignKey(d => d.Vetclinid)
                .HasConstraintName("comm_vetcin_fk");
        });

        modelBuilder.Entity<Passport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("passport_pk");

            entity.ToTable("passport");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Number).HasColumnType("character varying");
            entity.Property(e => e.Seria)
                .HasColumnType("character varying")
                .HasColumnName("seria");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Passport)
                .HasForeignKey<Passport>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("passport_animal_fk");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("record_pk");

            entity.ToTable("record");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Com)
                .HasColumnType("character varying")
                .HasColumnName("com");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Vetclinid).HasColumnName("vetclinid");

            entity.HasOne(d => d.User).WithMany(p => p.Records)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("record_user_fk");

            entity.HasOne(d => d.Vetclin).WithMany(p => p.Records)
                .HasForeignKey(d => d.Vetclinid)
                .HasConstraintName("record_vetcin_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pk");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Firstname)
                .HasColumnType("character varying")
                .HasColumnName("firstname");
            entity.Property(e => e.Password).HasColumnType("character varying");
            entity.Property(e => e.Secondname)
                .HasColumnType("character varying")
                .HasColumnName("secondname");
        });

        modelBuilder.Entity<Vacin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vacin_pk");

            entity.ToTable("vacin");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Passportid).HasColumnName("passportid");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");

            entity.HasOne(d => d.Passport).WithMany(p => p.Vacins)
                .HasForeignKey(d => d.Passportid)
                .HasConstraintName("vacin_passport_fk");
        });

        modelBuilder.Entity<Vetcin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vetcin_pk");

            entity.ToTable("vetcin");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasColumnType("character varying")
                .HasColumnName("adress");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
