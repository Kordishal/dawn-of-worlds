using System;
using System.Collections.Generic;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Diplomacy;

namespace dawn_of_worlds.Main
{
    class GameState
    {
        public World World { get; set; }

        public List<Deity> Deities { get; set; }
        public List<Race> Races { get; set; }
        public List<Civilisation> Civilizations { get; set; }
        public List<City> Cities { get; set; }
        public List<Order> Orders { get; set; }

        public Area[,] AreaGrid { get; set; }
        public Area getArea(SystemCoordinates coords) { return AreaGrid[coords.X, coords.Y]; }
        public Province[,] ProvinceGrid { get; set; }
        public Province getProvince(SystemCoordinates coords) { return ProvinceGrid[coords.X, coords.Y]; }

        public List<War> OngoingWars { get; set; }

        public GameState()
        {
            Deities = new List<Deity>();
            Races = new List<Race>();
            Civilizations = new List<Civilisation>();
            Cities = new List<City>();
            Orders = new List<Order>();

            OngoingWars = new List<War>();
        }

    }
}
