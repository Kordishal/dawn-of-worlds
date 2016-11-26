using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.CelestialPowers.ShapeLandPowers
{
    class CreateLake : ShapeLand
    {

        public override bool Precondition(World current_world, Deity creator, int current_age)
        {
            // can't be created in oceans
            if (_location.Type == TerrainType.Ocean)
                return false;

            // Can't be created without a river.
            if (!_location.hasRivers)
                return false;

            return true;
        }

        public override int Weight(World current_world, Deity creator, int current_age)
        {
            int weight = base.Weight(current_world, creator, current_age);

            if (creator.Domains.Contains(Domain.Water))
                weight += Constants.WEIGHT_MANY_CHANGE;

            if (creator.Domains.Contains(Domain.Drought))
                weight -= Constants.WEIGHT_MANY_CHANGE;

            return weight >= 0 ? weight : 0;
        }

        public override void Effect(World current_world, Deity creator, int current_age)
        {
            // Create the lake
            Lake lake = new Lake(Constants.Names.GetName("lakes"), _location, creator);
            lake.BiomeType = BiomeType.PermanentFreshWaterLake;

            // Choose random river which the lake is connected to.
            List<TerrainFeatures> rivers = _location.SecondaryTerrainFeatures.FindAll(x => x.GetType() == typeof(River));
            River river = (River)rivers[Constants.RND.Next(rivers.Count)];

            river.ConnectedLakes.Add(lake);
            lake.SourceRivers.Add(river);
            lake.OutGoingRiver = river;

            _location.SecondaryTerrainFeatures.Add(lake);
            _location.UnclaimedTerritory.Add(lake);

            // Add lake to deity lists
            creator.TerrainFeatures.Add(lake);
            creator.LastCreation = lake;          
        }


        public CreateLake(Terrain location) : base (location)
        {
            Name = "Create Lake in Terrain " + location.Name;
        }
    }
}
