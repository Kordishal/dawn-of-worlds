using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.CelestialPowers.ShapeClimatePowers
{
    class IncreaseTemperature : ShapeClimate
    {
        public override void Effect(World current_world, Deity creator, int current_age)
        {
            Area location = current_world.AreaGrid[Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_X), Main.MainLoop.RND.Next(Main.MainLoop.AREA_GRID_Y)];

            switch (location.AreaClimate.Temperature)
            {
                case Temperature.ExtremelyCold:
                    if (location.AreaClimate.ExtremTemperatureFactor > 0)
                        location.AreaClimate.ExtremTemperatureFactor -= 1;
                    else
                        location.AreaClimate.Temperature = Temperature.VeryCold;
                    break;
                case Temperature.VeryCold:
                    location.AreaClimate.Temperature = Temperature.Cold;
                    break;
                case Temperature.Cold:
                    location.AreaClimate.Temperature = Temperature.VeryCool;
                    break;
                case Temperature.VeryCool:
                    location.AreaClimate.Temperature = Temperature.Cool;
                    break;
                case Temperature.Cool:
                    location.AreaClimate.Temperature = Temperature.BelowAverage;
                    break;
                case Temperature.BelowAverage:
                    location.AreaClimate.Temperature = Temperature.Average;
                    break;
                case Temperature.Average:
                    location.AreaClimate.Temperature = Temperature.AboveAverage;
                    break;
                case Temperature.AboveAverage:
                    location.AreaClimate.Temperature = Temperature.Warm;
                    break;
                case Temperature.Warm:
                    location.AreaClimate.Temperature = Temperature.VeryWarm;
                    break;
                case Temperature.VeryWarm:
                    location.AreaClimate.Temperature = Temperature.Hot;
                    break;
                case Temperature.Hot:
                    location.AreaClimate.Temperature = Temperature.VeryHot;
                    break;
                case Temperature.VeryHot:
                    location.AreaClimate.Temperature = Temperature.ExtremelyHot;
                    break;
                case Temperature.ExtremelyHot:
                    location.AreaClimate.ExtremTemperatureFactor += 1;
                    break;
            }
        }

        public IncreaseTemperature()
        {
            Name = "Increase Temperature";
        }
    }
}
