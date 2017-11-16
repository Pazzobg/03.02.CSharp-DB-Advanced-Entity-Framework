namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;
    using P01_StudentSystem.Data.Models.Configurations;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

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
            /*  o	StudentId
                o	Name (up to 100 characters, unicode)
                o	PhoneNumber (exactly 10 characters, not unicode, not required)
                o	RegisteredOn
                o	Birthday (not required) */

            builder.Entity<Student>(en =>
            {
                en.Property(s => s.Name).IsUnicode(true).HasMaxLength(100).IsRequired(true);
                en.Property(s => s.PhoneNumber).HasColumnType("CHAR(10)");
                en.Property(s => s.RegisteredOn).IsRequired(true);
                en.Property(s => s.Birthday).IsRequired(false);
            });

            /*  o	CourseId
                o	Name (up to 80 characters, unicode)
                o	Description (unicode, not required)
                o	StartDate
                o	EndDate
                o	Price */

            builder.Entity<Course>(en =>
            {
                en.Property(c => c.Name).IsUnicode(true).HasMaxLength(80).IsRequired(true);
                en.Property(c => c.Description).IsUnicode(true).IsRequired(false);
                en.Property(c => c.StartDate).IsRequired(true);
                en.Property(c => c.EndDate).IsRequired(true);
                en.Property(c => c.Price).IsRequired(true);
            });

            /*  o	ResourceId
                o	Name (up to 50 characters, unicode)
                o	Url (not unicode)
                o	ResourceType (enum – can be Video, Presentation, Document or Other)
                o	CourseId */

            builder.Entity<Resource>(en =>
            {
                en.Property(r => r.Name).IsUnicode(true).HasMaxLength(50).IsRequired(true);
                en.Property(r => r.Url).IsUnicode(false).IsRequired(true);
                en.Property(r => r.ResourceType).IsRequired(true);
            });

            /*  o	HomeworkId
                o	Content (string, linking to a file, not unicode)
                o	ContentType (enum – can be Application, Pdf or Zip)
                o	SubmissionTime
                o	StudentId
                o	CourseId */

            builder.Entity<Homework>(en =>
            {
                en.Property(h => h.Content).IsUnicode(false).IsRequired(true);
                en.Property(h => h.ContentType).IsRequired(true);
                en.Property(h => h.SubmissionTime).IsRequired(true);
            });




            // Course -> Resources
            builder.ApplyConfiguration(new CourseResourcesConfiguration());

            // StudentsCourses
            builder.ApplyConfiguration(new StudentsCoursesConfiguration());

            // Homeworks
            builder.ApplyConfiguration(new HomeworkSubmissionsConfiguration());
        }
    }
}
