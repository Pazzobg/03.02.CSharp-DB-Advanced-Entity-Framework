using System;
using System.Reflection;

public class StartUp
{
    static void Main(string[] args)
    {
        Type personType = typeof(Person);
        ConstructorInfo emptyCtor = personType.GetConstructor(new Type[] { });
        ConstructorInfo ageCtor = personType.GetConstructor(new[] { typeof(int) });
        ConstructorInfo nameAgeCtor = personType.GetConstructor(new[] { typeof(string), typeof(int) });
        bool swapped = false;
        if (nameAgeCtor == null)
        {
            nameAgeCtor = personType.GetConstructor(new[] { typeof(int), typeof(string) });
            swapped = true;
        }

        string name = Console.ReadLine();
        int age = int.Parse(Console.ReadLine());

        Person basePerson = (Person)emptyCtor.Invoke(new object[] { });
        Person personWithAge = (Person)ageCtor.Invoke(new object[] { age });
        Person personWithAgeAndName = swapped ? (Person)nameAgeCtor.Invoke(new object[] { age, name }) : (Person)nameAgeCtor.Invoke(new object[] { name, age });

        Console.WriteLine("{0} {1}", basePerson.Name, basePerson.Age);
        Console.WriteLine("{0} {1}", personWithAge.Name, personWithAge.Age);
        Console.WriteLine("{0} {1}", personWithAgeAndName.Name, personWithAgeAndName.Age);

        //var first = new Person
        //{
        //    Name = "Ivan",
        //    Age = 20
        //};

        //var secondNoParams = new Person();

        //var thirdYrsParam = new Person(12);

        //var fourthAllParams = new Person("Marto", 24);

        //Console.WriteLine(first.Name + " " + first.Age);
        //Console.WriteLine(secondNoParams.Name + " " + secondNoParams.Age);
        //Console.WriteLine(thirdYrsParam.Name + " " + thirdYrsParam.Age);
        //Console.WriteLine(fourthAllParams.Name + " " + fourthAllParams.Age);
    }
}