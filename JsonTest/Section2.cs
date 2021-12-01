using System;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace JsonTest
{
    public class Section2
    {
        private static string _json = "";

        private static async Task<string> FetchWeatherFromJMA()
        {
            if (string.IsNullOrWhiteSpace(_json))
            {
                var client = new HttpClient();
                var response = await client.GetAsync(@"https://www.jma.go.jp/bosai/forecast/data/forecast/130000.json");

                _json = await response.Content.ReadAsStringAsync();
            }

            return _json;
        }

        public static async Task ExecWithNewtonsoftJson()
        {
            var json = await FetchWeatherFromJMA();
            var weatherModelList = WeatherModel.FromJson(json);

            Console.WriteLine("ExecWithNewtonsoftJson()");
            foreach (var m in weatherModelList)
            {
                var area = m.TimeSeries.First().Areas.First();
                Console.WriteLine(m.ReportDatetime.ToString());
                Console.WriteLine(area.Area.Name);

                if (area.Weathers == null)
                {
                    continue;
                }

                foreach (var w in area.Weathers)
                {
                    Console.WriteLine(w);
                }
            }
        }

        public static async Task ExecWithSystemTextJson()
        {
            var json = await FetchWeatherFromJMA();
            var jsonNode = JsonNode.Parse(json);

            if (jsonNode == null)
            {
                Console.WriteLine("jsonNode is null");
                return;
            }

            Console.WriteLine("ExecWithSystemTextJson()");
            var jsonArray = jsonNode.AsArray();
            Console.WriteLine(jsonArray.Count);
            foreach (var m in jsonArray)
            {
                var area = m["timeSeries"]?.AsArray().FirstOrDefault()["areas"]?.AsArray().FirstOrDefault();

                if (area == null)
                {
                    continue;
                }

                Console.WriteLine(m["reportDatetime"]?.ToString() ?? string.Empty);
                Console.WriteLine(area["area"]?["name"] ?? string.Empty);

                if (area["weathers"] == null)
                {
                    continue;
                }

                var weathers = area["weathers"].AsArray();

                foreach (var w in weathers)
                {
                    Console.WriteLine(w.ToString());
                }
            }
        }
    }
}

