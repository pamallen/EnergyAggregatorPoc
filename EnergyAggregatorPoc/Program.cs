using System;
using System.Threading.Tasks;

namespace EnergyAggregatorPoc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            double.TryParse(args[0], out double startTimeStamp);
            double.TryParse(args[1], out double endTimeStamp);

            var aggregator = Aggregator.Create(TimeSpan.FromMinutes(15));

            aggregator.AddFactory(PowerFactory.Create("https://interview.beta.bcmenergy.fr/hawes", "JSON", TimeSpan.FromMinutes(15)));
            aggregator.AddFactory(PowerFactory.Create("https://interview.beta.bcmenergy.fr/barnsley", "JSON", TimeSpan.FromMinutes(30)));
            aggregator.AddFactory(PowerFactory.Create("https://interview.beta.bcmenergy.fr/hounslow", "CSV", TimeSpan.FromMinutes(60)));


            await aggregator.AggregateReadings(DateTime.UnixEpoch.AddSeconds(startTimeStamp),
                DateTime.UnixEpoch.AddSeconds(endTimeStamp), args[2]);
            Console.WriteLine("Hello World!");
        }
    }
}
