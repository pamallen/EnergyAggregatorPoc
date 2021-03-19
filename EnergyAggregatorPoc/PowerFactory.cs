using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EnergyAggregatorPoc
{
    public class PowerFactory
    {
        public string ApiUrl { get; }
        public string ResponseFormat { get; }
        public TimeSpan TimeStep { get; }

        public List<EnergyProduction> EnergyProductions = new List<EnergyProduction>();


        private PowerFactory(string apiUrl, string responseFormat, TimeSpan timeStep)
        {
            ApiUrl = apiUrl;
            ResponseFormat = responseFormat;
            TimeStep = timeStep;
        }

        public static PowerFactory Create(string apiUrl, string responseFormat, TimeSpan timeStep)
        {
            return new PowerFactory(apiUrl, responseFormat, timeStep);
        }

        public async Task GetEnergyProductions(DateTime startTime, DateTime endTime)
        {
            EnergyProductions = new List<EnergyProduction>();

            var client = new HttpClient();

            var requestUri = ApiUrl + $"?from={startTime:dd-MM-yyyy}&to={endTime:dd-MM-yyyy}";

            var response = await client.GetAsync(requestUri);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readings = JsonConvert.DeserializeObject<List<EnergyReading>>(responseContent);

            var result = new List<EnergyProduction>();
            readings?.ForEach(reading =>
            {
                EnergyProductions.Add(EnergyProduction.Create(DateTime.UnixEpoch.AddSeconds(reading.Start),
                    DateTime.UnixEpoch.AddSeconds(reading.End),
                    reading.Power));
            });
        }
    }
}
