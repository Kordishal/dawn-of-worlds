using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Objects;

namespace dawn_of_worlds.CelestialPowers.CommandCityPowers
{
    class ConstructBuilding : CommandCity
    {
        private BuildingType _type { get; set; }

        public override void Effect(Deity creator)
        {
            Building building = new Building(null, creator, _type);

            building.City = _commanded_city;
            _commanded_city.Buildings.Add(building);

            building.Terrain = _commanded_city.TerrainFeature;

            building.Effect();
        }

        public ConstructBuilding(City command_city, BuildingType type) : base(command_city)
        {
            _type = type;
        }

    }
}
