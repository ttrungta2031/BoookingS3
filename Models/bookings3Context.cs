using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BoookingS3.Models
{
    public partial class bookings3Context : DbContext
    {
        public bookings3Context()
        {
        }

        public bookings3Context(DbContextOptions<bookings3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<BookingService> BookingServices { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceEvidence> ServiceEvidences { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<Spa> Spas { get; set; }
        public virtual DbSet<StaffService> StaffServices { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<staff> staff { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("workstation id=bookings3.mssql.somee.com;packet size=4096;user id=SE141052_SQLLogin_1;pwd=pif8sdlzcr;data source=bookings3.mssql.somee.com;persist security info=False;initial catalog=bookings3");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.CreateDay).HasColumnType("date");

                entity.Property(e => e.Feedback).HasMaxLength(100);

                entity.Property(e => e.Score)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.SpaId).HasColumnName("SpaID");

                entity.Property(e => e.TimeEnd)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStart)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Booking_Customer");

                entity.HasOne(d => d.Spa)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.SpaId)
                    .HasConstraintName("FK_Booking_Spa");
            });

            modelBuilder.Entity<BookingService>(entity =>
            {
                entity.ToTable("BookingService");

                entity.Property(e => e.TimeEnd)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStart)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.BookingServices)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_BookingService_Booking");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.BookingServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_BookingService_Service");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.BookingServices)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_BookingService_Staff");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Gender).HasMaxLength(20);

                entity.Property(e => e.Skin)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Customer_User");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentDay).HasColumnType("date");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_Payment_Booking");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.BookingServiceId).HasColumnName("BookingServiceID");

                entity.Property(e => e.CreateDay).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.Property(e => e.SpaId).HasColumnName("SpaID");

                entity.Property(e => e.UrlImage).IsUnicode(false);

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_Service_ServiceType");

                entity.HasOne(d => d.Spa)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.SpaId)
                    .HasConstraintName("FK_Service_Spa");
            });

            modelBuilder.Entity<ServiceEvidence>(entity =>
            {
                entity.ToTable("ServiceEvidence");

                entity.Property(e => e.CreateDay).HasColumnType("date");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.BookingService)
                    .WithMany(p => p.ServiceEvidences)
                    .HasForeignKey(d => d.BookingServiceId)
                    .HasConstraintName("FK_ServiceEvidence_BookingService");
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("ServiceType");

                entity.Property(e => e.ServiceName).HasMaxLength(50);

                entity.HasOne(d => d.Spa)
                    .WithMany(p => p.ServiceTypes)
                    .HasForeignKey(d => d.SpaId)
                    .HasConstraintName("FK_ServiceType_Spa1");
            });

            modelBuilder.Entity<Spa>(entity =>
            {
                entity.ToTable("Spa");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UrlImage).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Spas)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Spa_User");
            });

            modelBuilder.Entity<StaffService>(entity =>
            {
                entity.ToTable("StaffService");

                entity.Property(e => e.LevelExp)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.StaffServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_StaffService_Service");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.StaffServices)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_StaffService_Staff");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.Code)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDay).HasColumnType("date");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_UserRole1");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.CreateDay).HasColumnType("date");

                entity.Property(e => e.Dob)
                    .HasMaxLength(10)
                    .HasColumnName("DOB")
                    .IsFixedLength(true);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.HasOne(d => d.Spa)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.SpaId)
                    .HasConstraintName("FK_Staff_Spa");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
