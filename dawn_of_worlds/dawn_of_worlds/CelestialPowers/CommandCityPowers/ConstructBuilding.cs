using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Objects;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.CommandCityPowers
{
    class ConstructBuilding : CommandCity
    {
        private BuildingType _type { get; set; }

        protected override void initialize()
        {
            base.initialize();
            Name = "Construct Building";
            Tags = new List<string>() { "construction" };
        }

        public override int Effect(Deity creator)
        {
            Building building = new Building(Program.GenerateNames.GetName("building_names"), creator, _type);

            building.City = _commanded_city;
            _commanded_city.Buildings.Add(building);

            building.Terrain = _commanded_city.TerrainFeature;

            building.Effect();
            return 0;
        }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            return true;
        }

        public ConstructBuilding(City command_city, BuildingType type) : base(command_city)
        {
            _type = type;
            initialize();
        }

    }
}
