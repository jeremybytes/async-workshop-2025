using TaskAwait.Library;
using TaskAwait.Shared;

namespace NoAsync.Library;

public class AsyncConstructorReader : IPersonReader
{
    private static List<Person> people = [];

    public AsyncConstructorReader()
    {
        PersonReader reader = new();

        // NOT ALLOWED
        //people = await reader.GetPeopleAsync();
    }

    public List<Person> GetPeople()
    {
        return people;
    }

    public Person? GetPerson(int id)
    {
        return people.FirstOrDefault(p => p.Id == id);
    }
}
