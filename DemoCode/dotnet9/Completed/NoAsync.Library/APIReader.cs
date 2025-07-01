using Microsoft.VisualStudio.Threading;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace NoAsync.Library;

public class APIReader : IPersonReader
{
    public List<Person> GetPeople()
    {
        PersonReader taskLibraryReader = new();

        // Block / Deadlock
        //return taskLibraryReader.GetPeopleAsync().Result;
        //return taskLibraryReader.GetPeopleAsync().GetAwaiter().GetResult();

        // Block / Deadlock
        //Task<List<Person>> peopleTask = taskLibraryReader.GetPeopleAsync();
        ////peopleTask.Wait();
        //Task.WaitAll([peopleTask]);
        //return peopleTask.Result;

        // Block / No Deadlock
        JoinableTaskContext context = new();
        JoinableTaskFactory factory = new(context);

        List<Person> people = factory.Run(async () =>
            await taskLibraryReader.GetPeopleAsync());
        return people;
    }

    public Person? GetPerson(int id)
    {
        throw new NotImplementedException();
    }
}
