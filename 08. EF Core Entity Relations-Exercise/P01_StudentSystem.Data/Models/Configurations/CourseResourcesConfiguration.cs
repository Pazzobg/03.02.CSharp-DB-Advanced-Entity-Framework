namespace P01_StudentSystem.Data.Models.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CourseResourcesConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .HasMany(c => c.Resources)
                .WithOne(r => r.Course)
                .HasForeignKey(r => r.CourseId);
        }

        //builder.Entity<Resource>()  --???
        //    .HasOne(r => r.Course)
        //    .WithMany(c => c.Resources)
        //    .HasForeignKey(r => r.CourseId);

    }
}
