using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyAggregatorPoc
{
    class Aggregator
    {
        public TimeSpan TimeStep { get; }

        public List<PowerFactory> PowerFactories = new List<PowerFactory>();


        private Aggregator(TimeSpan timeStep)
        {
            TimeStep = timeStep;
        }

        public static Aggregator Create(TimeSpan timeStep)
        {
            return new Aggregator(timeStep);
        }

        public void AddFactory(PowerFactory powerFactory)
        {
            PowerFactories.Add(powerFactory);
        }

        public async Task<List<EnergyReading>> AggregateReadings(DateTime startTime, DateTime endTime, string format)
        {
            //PowerFactories.ForEach(async pf =>
            //{
            //    await pf.GetEnergyProductions(startTime, endTime);
            //});
            await PowerFactories[0].GetEnergyProductions(startTime, endTime);
            await PowerFactories[1].GetEnergyProductions(startTime, endTime);

            var aggregatedResults = new List<EnergyReading>();
            var currentStartTime = startTime;
            var currentEndTime = startTime + TimeStep;


            while (currentStartTime < endTime)
            {
                double currentAggregatedPower = 0;
                PowerFactories.ForEach(pf =>
                {
                    var currentEnergyProduction = pf.EnergyProductions.Find(ep => ep.StartTime <= currentStartTime && ep.EndTime >= currentEndTime);
                    currentAggregatedPower += currentEnergyProduction?.Power ?? 0;
                });

                var currentAggregatedReading = new EnergyReading
                {
                    Start = currentStartTime.Subtract(DateTime.UnixEpoch).Ticks,
                    End = currentEndTime.Subtract(DateTime.UnixEpoch).Ticks,
                    Power = currentAggregatedPower
                };

                aggregatedResults.Add(currentAggregatedReading);

                currentStartTime += TimeStep;
                currentEndTime += TimeStep;
            }

            return aggregatedResults;
        }
    }
}
