namespace People.Service.Data;

public interface IPeopleProvider
{
    List<Person> GetPeople();
    Person? GetPerson(int id);
}