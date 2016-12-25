using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dawn_of_worlds.Actors;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.WorldClasses;
using dawn_of_worlds.Creations.Geography;
using dawn_of_worlds.Main;

namespace dawn_of_worlds.Creations.Conflict
{
    class Battle : Creation
    {
        public Province Province { get; set; }
        private TerrainFeatures Location { get; set; }

        public Army AttackingArmy { get; set; }
        public Army DefendingArmy { get; set; }

        public int TotalAttackStrenghtBonus { get; set; }
        public int TotalDefenceStrenghtBonus { get; set; }


        private void chooseBattleLocation()
        {
            List<WeightedObjects<TerrainFeatures>> possible_battle_locations = new List<WeightedObjects<TerrainFeatures>>();

            switch (Province.Type)
            {
                case TerrainType.Plain:
                    possible_battle_locations.Add(new WeightedObjects<TerrainFeatures>(Province.PrimaryTerrainFeature));
                    break;
                case TerrainType.MountainRange:
                case TerrainType.HillRange:
                case TerrainType.Ocean:
                    break;
            }

            foreach (TerrainFeatures terrain in Province.SecondaryTerrainFeatures)
                possible_battle_locations.Add(new WeightedObjects<TerrainFeatures>(terrain));

            foreach (WeightedObjects<TerrainFeatures> terrain in possible_battle_locations)
            {
                terrain.Weight += terrain.Object.Modifiers.NaturalDefenceValue * 10;

                if (Province.Owner == DefendingArmy.Owner)
                    terrain.Weight += terrain.Object.Modifiers.FortificationDefenceValue * 10;
            }
            Location = WeightedObjects<TerrainFeatures>.ChooseXHeaviestObjects(possible_battle_locations, 1)[0];
        }

        private void calculateModifiers()
        {
            TotalDefenceStrenghtBonus = DefendingArmy.StrenghtBonus;

            TotalDefenceStrenghtBonus += Location.Modifiers.NaturalDefenceValue;
            if (DefendingArmy.Owner == Province.Owner)
                TotalDefenceStrenghtBonus += Location.Modifiers.FortificationDefenceValue;

            TotalAttackStrenghtBonus = AttackingArmy.StrenghtBonus;
        }

        public void Fight()
        {
            int attacker_strenght = Main.Constants.Random.Next(2, 13) + TotalAttackStrenghtBonus;
            int defender_strenght = Main.Constants.Random.Next(2, 13) + TotalDefenceStrenghtBonus;

            // defender wins
            if (defender_strenght >= attacker_strenght)
            {
                AttackingArmy.isScattered = true;
            }
            // attacker wins
            else
            {
                DefendingArmy.isScattered = true;
            }
        }



        public Battle(string name, Deity creator, Army attacker, Army defender, Province province) : base(name, creator)
        {
            Province = province;
            AttackingArmy = attacker;
            DefendingArmy = defender;
            TotalAttackStrenghtBonus = 0;
            TotalDefenceStrenghtBonus = 0;
        }
    }
}
