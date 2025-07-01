using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TaskAwait.Framework.Shared;

namespace TaskAwait.Framework.Library
{
    public class PersonReader
    {
        HttpClient client = new HttpClient() 
            { BaseAddress = new Uri("http://localhost:9874") };
        JsonSerializerOptions options = new JsonSerializerOptions() 
            { PropertyNameCaseInsensitive = true };

        public async Task<List<Person>> GetPeopleAsync()
        {
            HttpResponseMessage response =
                await client.GetAsync("people");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unable to retrieve People. Status code {response.StatusCode}");

            var stringResult =
                await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Person>>(stringResult, options);
            if (result is null)
                throw new JsonException("Unable to deserialize List<Person> object (json object may be empty)");
            return result;
        }

        public async Task<Person> GetPersonAsync(int id,
            CancellationToken cancelToken = new CancellationToken())
        {
            //throw new NotImplementedException("Jeremy did not implement GetPersonAsync");

            HttpResponseMessage response =
                await client.GetAsync($"people/{id}", cancelToken).ConfigureAwait(false);

            cancelToken.ThrowIfCancellationRequested();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unable to retrieve Person. Status code {response.StatusCode}");

            var stringResult =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<Person>(stringResult, options);
            if (result is null)
                throw new JsonException("Unable to deserialize Person object (json object may be empty)");
            return result;
        }

        public async Task<List<int>> GetIdsAsync(
            CancellationToken cancelToken = new CancellationToken())
        {
            HttpResponseMessage response =
                await client.GetAsync("people/ids", cancelToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unable to retrieve IDs. Status code {response.StatusCode}");

            var stringResult =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<List<int>>(stringResult);
            if (result is null)
                throw new JsonException("Unable to deserialize List<int> object (json object may be empty)");
            return result;
        }
    }
}
