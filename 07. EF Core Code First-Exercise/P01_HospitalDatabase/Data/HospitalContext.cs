namespace P01_HospitalDatabase.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;

    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {

        }

        public HospitalContext(DbContextOptions options)
            : base (options)
        {

        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientMedicament { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.ConnectionString);   
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Patient>(e =>
            {
                e.Property(p => p.FirstName).IsRequired().HasMaxLength(50).IsUnicode();
                e.Property(p => p.LastName).IsRequired().HasMaxLength(50).IsUnicode();
                e.Property(p => p.Address).HasMaxLength(250).IsUnicode();
                e.Property(p => p.Email).IsRequired().HasColumnType("varchar(80)");
                e.Property(p => p.HasInsurance).IsRequired();
            });

            builder.Entity<Doctor>(e =>
            {
                e.Property(d => d.Name).IsRequired().HasMaxLength(100).IsUnicode();
                e.Property(d => d.Specialty).IsRequired().HasMaxLength(100).IsUnicode();
            });

            builder.Entity<Visitation>(e =>
            {
                e.Property(v => v.Date).IsRequired();
                e.Property(v => v.Comments).IsRequired().HasMaxLength(250).IsUnicode();
                e.Property(v => v.PatientId).IsRequired();
            });

            builder.Entity<Diagnose>(e =>
            {
                e.Property(d => d.Name).IsRequired().HasMaxLength(50).IsUnicode();
                e.Property(d => d.Comments).HasMaxLength(250).IsUnicode();
                e.Property(d => d.PatientId).IsRequired();
            });

            builder.Entity<Medicament>(e =>
            {
                e.Property(m => m.Name).IsRequired().HasMaxLength(50).IsUnicode();
            });

            builder.Entity<Visitation>()
                .HasOne(v => v.Patient)
                .WithMany(p => p.Visitations)
                .HasForeignKey(v => v.PatientId);

            builder.Entity<Visitation>()
                .HasOne(v => v.Doctor)
                .WithMany(d => d.Visitations)
                .HasForeignKey(v => v.DoctorId);

            builder.Entity<Diagnose>()
                .HasOne(d => d.Patient)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.PatientId);

            builder.Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(v => v.Patient)
                .HasForeignKey(v => v.PatientId);

            builder.Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithOne(d => d.Patient)
                .HasForeignKey(d => d.PatientId);

            builder.Entity<Doctor>()
                .HasMany(d => d.Visitations)
                .WithOne(v => v.Doctor)
                .HasForeignKey(v => v.DoctorId);

            builder.Entity<PatientMedicament>()
                .HasKey(pm => new { pm.PatientId, pm.MedicamentId });

            builder.Entity<PatientMedicament>()
                .HasOne(pm => pm.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(pm => pm.PatientId);

            builder.Entity<PatientMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(pm => pm.MedicamentId);
        }
    }
}
