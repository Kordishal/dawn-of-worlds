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
            isPrimary = true;
            Tags = new List<CreationTag>() { CreationTag.Creation, CreationTag.Plain, CreationTag.Earth };
        }

        // Grasslands are never created in landmass, as they are the default terrain type.
        // but are kept on ocean provinces in case of islands (NYI)
        public override bool isObsolete
        {
            get
            {
                //return _location.Region.Type == RegionType.Continent;
                return true;
            }
        }

        public override int Effect(Deity creator)
        {
            Grassland grassland = new Grassland("PlaceHolder", SelectedProvince, creator);

            switch (SelectedProvince.LocalClimate)
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
            SelectedProvince.PrimaryTerrainFeature = grassland;
            SelectedProvince.isDefault = false;

            creator.TerrainFeatures.Add(grassland);
            creator.LastCreation = grassland;

            Program.WorldHistory.AddRecord(grassland, grassland.printTerrainFeature);

            return 0;
        }
    }
}
