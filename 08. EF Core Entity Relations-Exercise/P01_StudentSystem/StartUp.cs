namespace P01_StudentSystem
{
    using P01_StudentSystem.Data;
    using P01_StudentSystem.Data.Models;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new StudentSystemContext())
            {
                // ResetDatabase(context);

                Seed(context);
            }
        }

        private static void ResetDatabase(StudentSystemContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();
        }

        private static void Seed(StudentSystemContext context)
        {
            var students = new[]
            {
                new Student("Ivan Ivanov", new DateTime(2017, 09, 15)),
                new Student("Petar Petrov", new DateTime(2017, 09, 16)),
                new Student("Georgi Georgiev", new DateTime(2017, 09, 17))
            };

            context.Students.AddRange(students);

            var courses = new[]
            {
                new Course("C#", new DateTime(2017, 09, 20), new DateTime(2017, 12, 15), 349.99m),
                new Course("Java", new DateTime(2017, 09, 21), new DateTime(2017, 12, 16), 379.99m),
                new Course("JavaScript", new DateTime(2017, 09, 26), new DateTime(2017, 12, 21), 319.99m)
            };

            context.Courses.AddRange(courses);

            var resources = new[]
            {
                new Resource("Video lesson", "youuuube.com", ResourceType.Video, courses[0]),
                new Resource("Exercises", "asddsa.som", ResourceType.Document, courses[2]),
                new Resource("First Steps in Java", "sofffuni.gb", ResourceType.Presentation, courses[1])
            };

            context.Resources.AddRange(resources);

            var studentsCourses = new[]
            {
                new StudentCourse(students[0], courses[0]),
                new StudentCourse(students[1], courses[1]),
                new StudentCourse(students[2], courses[2])
            };

            context.StudentCourses.AddRange(studentsCourses);

            var homeworks = new[]
            {
                new Homework("ww.myC#homework.som", ContentType.Application, new DateTime(2017, 10, 01), students[0], courses[0]),
                new Homework("ww.myJavaHomework.som", ContentType.Pdf, new DateTime(2017, 10, 04), students[1], courses[1]),
                new Homework("ww.myJShomework.som", ContentType.Zip, new DateTime(2017, 10, 11), students[2], courses[2]),
            };

            context.HomeworkSubmissions.AddRange(homeworks);

            context.SaveChanges();
        }
    }
}
