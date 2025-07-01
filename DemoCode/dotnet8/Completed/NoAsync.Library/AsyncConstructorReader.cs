using TaskAwait.Library;
using TaskAwait.Shared;

namespace NoAsync.Library;

public class AsyncConstructorReader : IPersonReader
{
    private static List<Person> people = [];

    //public AsyncConstructorReader()
    //{
    //    PersonReader reader = new();

    //    // NOT ALLOWED
    //    //people = await reader.GetPeopleAsync();
    //}

    private AsyncConstructorReader() { }

    public static async Task<AsyncConstructorReader> CreateReaderAsync()
    {
        AsyncConstructorReader myself = new();
        PersonReader taskLibraryReader = new();

        people = await taskLibraryReader.GetPeopleAsync();
        
        return myself;
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
