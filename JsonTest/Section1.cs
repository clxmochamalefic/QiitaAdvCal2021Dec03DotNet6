using System;
using System.Text.Json.Nodes;

namespace JsonTest
{
    public class Section1
    {
        public static async Task Exec()
        {

            Console.WriteLine("Hello, World!");

            var client = new HttpClient();
            var response = await client.GetAsync(@"https://www.jma.go.jp/bosai/forecast/data/forecast/130000.json");

            var json = await response.Content.ReadAsStringAsync();

            var jsonNode = JsonNode.Parse(json);

            var publishingOffice = jsonNode[0]["publishingOffice"]?.ToString();

            Console.WriteLine($"publishingOffice: {publishingOffice}");
        }
    }
}

