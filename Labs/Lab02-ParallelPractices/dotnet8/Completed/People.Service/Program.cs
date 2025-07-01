using People.Service.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IPeopleProvider, HardCodedPeopleProvider>();
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);

var app = builder.Build();

app.MapGet("/people", async (IPeopleProvider provider) => 
    {
        await Task.Delay(3000);
        return provider.GetPeople();
    })
    .WithName("GetPeople");

app.MapGet("/people/{id}", async (IPeopleProvider provider, int id) =>
    {
        await Task.Delay(500);
        var result = provider.GetPerson(id);
        return result switch
        {
            null => Results.NoContent(),
            _ => Results.Json(result)
        };
    })
    .WithName("GetPersonById");

app.MapGet("/people/ids",
    (IPeopleProvider provider) => provider.GetPeople().Select(p => p.Id).ToList())
    .WithName("GetAllPersonIds");

app.Run();