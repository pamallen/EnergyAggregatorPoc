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

        public Task<List<EnergyReading>> AggregateReadings(DateTime startTime, DateTime endTime, string format)
        {
            PowerFactories.ForEach(async pf =>
            {
                await pf.GetEnergyProductions(startTime, endTime);
            });

        }
    }
}
