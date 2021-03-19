using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic;

namespace EnergyAggregatorPoc
{
    public class EnergyProduction
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double Power { get; }


        private EnergyProduction(DateTime startTime, DateTime endTime, double power)
        {
            StartTime = startTime;
            EndTime = endTime;
            Power = power;
        }

        public static EnergyProduction Create(DateTime startTime, DateTime endTime, double power)
        {
            if (endTime < startTime)
            {
                Console.WriteLine("Start time must be before end time");
            }

            return new EnergyProduction(startTime, endTime, power);
        }
    }
}
