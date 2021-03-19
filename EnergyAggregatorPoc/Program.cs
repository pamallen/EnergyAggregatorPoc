using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EnergyAggregatorPoc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DateTime.TryParse(args[0], out DateTime startTime);
            DateTime.TryParse(args[1], out DateTime endTime);

            var aggregator = Aggregator.Create(TimeSpan.FromMinutes(15));

            aggregator.AddFactory(PowerFactory.Create("https://interview.beta.bcmenergy.fr/hawes", "JSON", TimeSpan.FromMinutes(15)));
            aggregator.AddFactory(PowerFactory.Create("https://interview.beta.bcmenergy.fr/barnsley", "JSON", TimeSpan.FromMinutes(30)));

            // Not enough time to implement reading from csv files, so for now we'll pretend this factory doesn't exist
            //aggregator.AddFactory(PowerFactory.Create("https://interview.beta.bcmenergy.fr/hounslow", "CSV", TimeSpan.FromMinutes(60)));

            var result = JsonConvert.SerializeObject(await aggregator.AggregateReadings(startTime, endTime, args[2]), Formatting.Indented);

            Console.WriteLine(result);
        }
    }
}
