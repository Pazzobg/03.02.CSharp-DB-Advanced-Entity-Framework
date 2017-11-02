using System.Collections.Generic;

public class Family
{
    private List<Person> familyMembers;

    public Family()
    {
        this.familyMembers = new List<Person>();
    }

    private List<Person> FamilyMembers
    {
        get
        {
            return this.familyMembers;
        }
    }

    public void AddMember(Person member)
    {
        this.FamilyMembers.Add(member);
    }

    public Person GetOldestMember()
    {
        int maxAge = int.MinValue;
        Person oldestMember = default(Person);

        foreach (var member in this.FamilyMembers)
        {
            if (member.Age > maxAge)
            {
                oldestMember = member;
                maxAge = member.Age;
            }
        }

        return oldestMember;
    }
}
