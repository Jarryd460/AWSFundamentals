using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Text.Json;

namespace Movies.Api;

public class DataSeeder
{
    public async Task ImportDataAsync()
    {
        var dynamoDb = new AmazonDynamoDBClient();
        var lines = await File.ReadAllLinesAsync("./movies.csv");
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0)
            {
                continue; //Skip header
            }

            var line = lines[i];
            var commaSplit = line.Split(',');

            var title = commaSplit[0];
            var year = int.Parse(commaSplit[1]);
            var ageRestriction = int.Parse(commaSplit[2]);
            var rottenTomatoes = int.Parse(commaSplit[3]);

            var movie1 = new Movie1
            {
                Id = Guid.NewGuid(),
                Title = title,
                AgeRestriction = ageRestriction,
                ReleaseYear = year,
                RottenTomatoesPercentage = rottenTomatoes
            };

            var movie1AsJson = JsonSerializer.Serialize(movie1);
            var item1AsDocument = Document.FromJson(movie1AsJson);
            var item1AsAttributes = item1AsDocument.ToAttributeMap();

            var create1ItemRequest = new PutItemRequest
            {
                // Need to manually create the tables. Table names that were used are movies-title-wrotten and movies-year-title.
                TableName = "movies-year-title",
                Item = item1AsAttributes
            };

            var response1 = await dynamoDb.PutItemAsync(create1ItemRequest);
            await Task.Delay(300);

            var movie2 = new Movie2
            {
                Id = Guid.NewGuid(),
                Title = title,
                AgeRestriction = ageRestriction,
                ReleaseYear = year,
                RottenTomatoesPercentage = rottenTomatoes
            };

            var movie2AsJson = JsonSerializer.Serialize(movie2);
            var item2AsDocument = Document.FromJson(movie2AsJson);
            var item2AsAttributes = item2AsDocument.ToAttributeMap();

            var create2ItemRequest = new PutItemRequest
            {
                // Need to manually create the tables. Table names that were used are movies-title-wrotten and movies-year-title.
                TableName = "movies-title-wrotten",
                Item = item2AsAttributes
            };

            var response2 = await dynamoDb.PutItemAsync(create2ItemRequest);
            await Task.Delay(300);
        }
    }
}
