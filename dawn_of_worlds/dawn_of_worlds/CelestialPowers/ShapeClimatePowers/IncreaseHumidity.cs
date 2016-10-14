using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class IncreaseHumidity : ShapeClimate
    {
        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

            switch (location.AreaClimate.Humidity)
            {
                case Humidity.ExtremelyDry:
                    if (location.AreaClimate.ExtremHumidityFactor > 0)
                        location.AreaClimate.ExtremTemperatureFactor -= 1;
                    else
                        location.AreaClimate.Humidity = Humidity.VeryDry;
                    break;
                case Humidity.VeryDry:
                    location.AreaClimate.Humidity = Humidity.Dry;
                    break;
                case Humidity.Dry:
                    location.AreaClimate.Humidity = Humidity.SlightlyHumid;
                    break;
                case Humidity.SlightlyHumid:
                    location.AreaClimate.Humidity = Humidity.Humid;
                    break;
                case Humidity.Humid:
                    location.AreaClimate.Humidity = Humidity.VeryHumid;
                    break;
                case Humidity.VeryHumid:
                    location.AreaClimate.Humidity = Humidity.ExtremelyHumid;
                    break;
                case Humidity.ExtremelyHumid:
                    location.AreaClimate.ExtremHumidityFactor += 1;
                    break;
            }
        }

        public IncreaseHumidity()
        {
            Name = "Increase Humidity";
        }
    }
}
