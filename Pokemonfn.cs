using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pokemonfn
{
    public class Pokemonfn
    {
        [Function("Pokemonfn")]
        [CosmosDBOutput("prmlearning", "pokemon", Connection = "CosmosDbConnection")]
        public static Pokemon Run(
            [QueueTrigger("pokemon", Connection = "AzureWebJobsStorage")] QueueMessage msg,
            FunctionContext ctx
        )
        {
            var logger = ctx.GetLogger("pkmn");

            try
            {
                var pkmn = msg.Body.ToObjectFromJson<Pokemon>();
                if (pkmn is null)
                {
                    throw new Exception("Invalid Pokemon object.");
                }

                logger.LogInformation($"Inserting Pokémon: {JsonSerializer.Serialize(pkmn)}");

                return pkmn;
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to process message: {ex.Message}");
                throw;
            }
        }

    }

    public class Pokemon
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("nmb")]
        public int Nmb { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("partitionKey")]
        public string PartitionKey { get; set; } = "pokemon";
    }
}
