using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Objects;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Effects;
using dawn_of_worlds.Creations.Civilisations;

namespace dawn_of_worlds.CelestialPowers.CommandNationPowers
{
    class ConstructFortification : CommandNation
    {
        private BuildingType _type { get; set; }

        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);
            if (potential_construction_sites().Count > 0)
                return true;
            else
                return false;
        }


        protected override void initialize()
        {
            Name = "Construct Fortification (" + _type.ToString() + ")";
            Tags = new List<CreationTag>() { CreationTag.Construction, CreationTag.Military };
        }


        public override int Effect(Deity creator)
        {
            TerrainFeatures terrain = WeightedObjects<TerrainFeatures>.ChooseRandomObject(potential_construction_sites(), rnd);

            Building building = new Building(null, creator, _type);
            building.Terrain = terrain;
            building.Effect();

            terrain.Buildings.Add(building);

            return 0;
        }

        public ConstructFortification(Civilisation commanded_nation, BuildingType type) : base(commanded_nation)
        {
            _type = type;
            initialize();
        }

        private List<WeightedObjects<TerrainFeatures>> potential_construction_sites()
        {
            List<WeightedObjects<TerrainFeatures>> terrain_features = new List<WeightedObjects<TerrainFeatures>>();

            // Every building can only be built once per terrain feature.
            foreach (Province province in _commanded_nation.Territory)
            {
                if (!province.PrimaryTerrainFeature.Buildings.Exists(x => x.Type == _type) && province.PrimaryTerrainFeature.City == null)
                    terrain_features.Add(new WeightedObjects<TerrainFeatures>(province.PrimaryTerrainFeature));

                foreach (TerrainFeatures terrain in province.SecondaryTerrainFeatures)
                    if (!terrain.Buildings.Exists(x => x.Type == _type) && terrain.City == null)
                        terrain_features.Add(new WeightedObjects<TerrainFeatures>(terrain));
            }

            foreach (WeightedObjects<TerrainFeatures> weighted_terrain in terrain_features)
            {
                weighted_terrain.Weight += 5;
            }

            return terrain_features;
        }
    }
}
