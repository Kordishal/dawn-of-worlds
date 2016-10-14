using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.WorldClasses
{
    class Climate
    {

        public Temperature Temperature { get; set; }
        public int ExtremTemperatureFactor { get; set; }

        public Humidity Humidity { get; set; }
        public int ExtremHumidityFactor { get; set; }

        public Climate()
        {
            Temperature = Temperature.Average;
            ExtremTemperatureFactor = 0;

            Humidity = Humidity.SlightlyHumid;
            ExtremHumidityFactor = 0;
        }

        public override string ToString()
        {
            return "Climate: " + Temperature.ToString() + "|" + Humidity.ToString();
        }

    }


    public enum Temperature
    {
        ExtremelyCold, // -30+ Everything is Frozen
        VeryCold, // 0 - -30
        Cold, // 0
        VeryCool, // 5 
        Cool, // 10
        BelowAverage, // Temperate 15
        Average, // Room 20
        AboveAverage, // Temperate Summer 25
        Warm, // Mediterranean 30
        VeryWarm, // Mediteranean Warm Summer 35
        Hot, // Tropical/Deserts 40 
        VeryHot, // Dry & Prone to catch fire 50 - 100
        ExtremelyHot // Everything on Fire 100+
    }

    public enum Humidity
    {
        ExtremelyHumid, // Unnatural
        VeryHumid, // Tropical
        Humid, // Mediterranean
        SlightlyHumid, // Temperate
        Dry, // Savannah/Steppe
        VeryDry, // Desert
        ExtremelyDry // Unnatural
    }
}
