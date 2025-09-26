using Microsoft.Azure.Cosmos;
using Pokemonfn;

public class CosmosService
{
    private readonly Container _container;

    public CosmosService(string connectionString, string databaseId, string containerId)
    {
        var client = new CosmosClient(connectionString);
        _container = client.GetContainer(databaseId, containerId);
    }

    public async Task InsertAsync(Pokemon pkmn)
    {
        var response = await _container.CreateItemAsync(pkmn, new PartitionKey(pkmn.PartitionKey));
        Console.WriteLine($"Status: {response.StatusCode}, ActivityId: {response.ActivityId}");
    }
}
