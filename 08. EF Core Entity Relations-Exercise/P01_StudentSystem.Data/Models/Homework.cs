namespace P01_StudentSystem.Data.Models
{
    using System;

    public class Homework
    {
        public Homework()
        {

        }

        public Homework(string contentLink, ContentType contentType, DateTime submissionTime, Student student, Course course)
        {
            this.Content = contentLink;
            this.ContentType = contentType;
            this.SubmissionTime = submissionTime;
            this.Student = student;
            this.Course = course;
        }

        public int HomeworkId { get; set; }

        public string Content { get; set; } //linking to a file?!?!?!

        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }  
        public Student Student { get; set; }      

        public int CourseId { get; set; }   
        public Course Course { get; set; } 
    }
}
