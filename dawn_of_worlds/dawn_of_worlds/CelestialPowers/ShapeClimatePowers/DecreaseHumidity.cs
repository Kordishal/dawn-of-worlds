using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class DecreaseHumidity : ShapeClimate
    {
        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

            switch (location.AreaClimate.Humidity)
            {
                case Humidity.ExtremelyDry:
                    location.AreaClimate.ExtremTemperatureFactor += 1;
                    break;
                case Humidity.VeryDry:
                    location.AreaClimate.Humidity = Humidity.ExtremelyDry;
                    break;
                case Humidity.Dry:
                    location.AreaClimate.Humidity = Humidity.VeryDry;
                    break;
                case Humidity.SlightlyHumid:
                    location.AreaClimate.Humidity = Humidity.Dry;
                    break;
                case Humidity.Humid:
                    location.AreaClimate.Humidity = Humidity.SlightlyHumid;
                    break;
                case Humidity.VeryHumid:
                    location.AreaClimate.Humidity = Humidity.Humid;
                    break;
                case Humidity.ExtremelyHumid:
                    if (location.AreaClimate.ExtremHumidityFactor > 0)
                        location.AreaClimate.ExtremHumidityFactor -= 1;
                    else
                        location.AreaClimate.Humidity = Humidity.VeryHumid;
                    break;
            }
        }

        public DecreaseHumidity()
        {
            Name = "Decrease Humidity";
        }
    }
}
