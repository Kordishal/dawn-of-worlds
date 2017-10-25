using System;
using System.Collections.Generic;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Main;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.WorldModel;
using dawn_of_worlds.Names;

namespace dawn_of_worlds.Creations.Objects
{
    [Serializable]
    class Building : Creation
    {
        public BuildingCategory Category { get; set; }
        public BuildingType Type { get; set; }
        public BuildingEffect Effect { get; set; }

        public TerrainFeatures Terrain { get; set; }
        public City City { get; set; }


        public void TempleEffect()
        {
            Creator.Modifiers.BonusPowerPoints += 1;
        }

        public void ShrineEffect()
        {
            Creator.Modifiers.PowerPointModifier += 0.01;
        }

        public void CityWallsEffect()
        {
            City.Modifiers.FortificationLevel += 1;
            City.TerrainFeature.Modifiers.FortificationDefenceValue += 1;
        }

        public void FortressEffect()
        {
            Terrain.Modifiers.FortificationDefenceValue += 2;
        }

        public Building(string name, Deity creator, BuildingType type) : base(name, creator)
        {
            Type = type;
            switch (Type)
            {
                case BuildingType.CityWall:
                    Name = "City Wall";
                    Category = BuildingCategory.Military;
                    Effect = CityWallsEffect;
                    break;
                case BuildingType.Fortress:
                    Name = "Fortress";
                    Category = BuildingCategory.Military;
                    Effect = FortressEffect;
                    break;
                case BuildingType.Temple:
                    Name = "Temple";
                    Category = BuildingCategory.Religious;
                    Effect = TempleEffect;
                    break;
                case BuildingType.Shrine:
                    Name = "Shrine";
                    Category = BuildingCategory.Religious;
                    Effect = ShrineEffect;
                    break;
            }


        }
    }

    public delegate void BuildingEffect();

    enum BuildingCategory
    {
        Religious,
        Military,
        Trade,
        Education,
        Administration,
        Wonder,
    }

    enum BuildingType
    {
        CityWall,
        Fortress,
        Temple,
        Shrine,
    }

}
