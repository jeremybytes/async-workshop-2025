using TaskAwait.Shared;

namespace NoAsync.Library;

public class HardCodedPersonReader : IPersonReader
{
    public List<Person> GetPeople()
    {
        return
        [
            new(1, "John", "Koenig", new(1975, 10, 17), 6, ""),
            new(2, "Dylan", "Hunt", new(2000, 10, 2), 8, ""),
            new(3, "Leela", "Turanga", new(1999, 3, 28), 8, "{1} {0}"),
            new(4, "John", "Crichton", new(1999, 3, 19), 7, ""),
            new(5, "Dave", "Lister", new(1988, 2, 15), 9, ""),
            new(6, "Laura", "Roslin", new(2003, 12, 8), 6, ""),
            new(7, "John", "Sheridan", new(1994, 1, 26), 6, ""),
            new(8, "Dante", "Montana", new(2000, 11, 1), 5, ""),
            new(9, "Isaac", "Gampu", new(1977, 9, 10), 4, ""),
            new(10, "Naomi", "Nagata", new(2015, 11, 23), 7, ""),
            new(11, "John", "Boon", new(1993, 01, 06), 5, ""),
            new(12, "Kerr", "Avon", new(1978, 01, 02), 8, ""),
            new(13, "Ed", "Mercer", new(2017, 09, 10), 8, ""),
            new(14, "Devon", "", new(1973, 09, 23), 4, "{0}"),
        ];
    }

    public Person? GetPerson(int id)
    {
        return GetPeople().FirstOrDefault(p => p.Id == id);
    }
}
