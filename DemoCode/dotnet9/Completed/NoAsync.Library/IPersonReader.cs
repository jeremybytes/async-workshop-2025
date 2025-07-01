using TaskAwait.Shared;

namespace NoAsync.Library;

public interface IPersonReader
{
    List<Person> GetPeople();
    Person? GetPerson(int id);
}
