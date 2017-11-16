namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Course
    {
        public Course()
        {

        }

        public Course(string name, DateTime start, DateTime end, decimal price)
        {
            this.Name = name;
            this.StartDate = start;
            this.EndDate = end;
            this.Price = price;
        }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentsEnrolled { get; set; } = new HashSet<StudentCourse>();

        public ICollection<Resource> Resources { get; set; } = new List<Resource>();

        public ICollection<Homework> HomeworkSubmissions { get; set; } = new HashSet<Homework>();
    }
}
