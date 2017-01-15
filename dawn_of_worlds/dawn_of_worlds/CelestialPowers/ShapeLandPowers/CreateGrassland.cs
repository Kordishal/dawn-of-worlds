using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateGrassland : ShapeLand
    {

        protected override void initialize()
        {
            base.initialize();
            Name = "Create Grassland";
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Plain, CreationTag.Earth };
        }

        // Grasslands are never created in landmass, as they are the default terrain type.
        // but are kept on ocean provinces in case of islands (NYI)
        public override bool isObsolete
        {
            get
            {
                return _location.Region.Type == RegionType.Continent;
            }
        }


        public override bool Precondition(Deity creator)
        {
            base.Precondition(creator);

            // needs a possible terrain in the area.
            if (candidate_terrain().Count == 0)
                return false;

            return true;
        }

        public override void Effect(Deity creator)
        {
            List<Province> grassland_locations = candidate_terrain();
            Province grassland_location = grassland_locations[Constants.Random.Next(grassland_locations.Count)];

            Grassland grassland = new Grassland("PlaceHolder", grassland_location, creator);

            switch (grassland_location.LocalClimate)
            {
                case Climate.Arctic:
                    grassland.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.SubArctic:
                    grassland.BiomeType = BiomeType.Tundra;
                    break;
                case Climate.Temperate:
                    grassland.BiomeType = BiomeType.TemperateGrassland;
                    break;
                case Climate.SubTropical:
                    grassland.BiomeType = BiomeType.TropicalGrassland;
                    break;
                case Climate.Tropical:
                    grassland.BiomeType = BiomeType.TropicalGrassland;
                    break;
            }

            grassland.Name.Singular = Constants.Names.GetName("grasslands");
            grassland_location.PrimaryTerrainFeature = grassland;

            creator.TerrainFeatures.Add(grassland);
            creator.LastCreation = grassland;

            Program.WorldHistory.AddRecord(grassland, grassland.printTerrainFeature);
        }

        public CreateGrassland(Area location) : base(location) { }

        private List<Province> candidate_terrain()
        {
            List<Province> terrain_list = new List<Province>();
            foreach (Province terrain in _location.Provinces)
            {
                if (terrain.Type == TerrainType.Island)
                    terrain_list.Add(terrain);
            }

            return terrain_list;
        }
    }
}
