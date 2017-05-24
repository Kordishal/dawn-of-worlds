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
using dawn_of_worlds.Log;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Effects;

namespace dawn_of_worlds.Creations.Conflict
{
    class Battle : Creation
    {
        public War War { get; set; }

        public Province Province { get; set; }
        private TerrainFeatures Location { get; set; }

        // Attacker = 0
        // Defender = 1
        public Army[] InvolvedArmies { get; set; }
        public int[] Modifiers { get; set; }
        public int[] Score { get; set; }

        private int _count { get { return InvolvedArmies.Length; } }

        private void chooseBattleLocation()
        {
            List<WeightedObjects<TerrainFeatures>> possible_battle_locations = new List<WeightedObjects<TerrainFeatures>>();

            possible_battle_locations.Add(new WeightedObjects<TerrainFeatures>(Province.PrimaryTerrainFeature));

            foreach (TerrainFeatures terrain in Province.SecondaryTerrainFeatures)
                possible_battle_locations.Add(new WeightedObjects<TerrainFeatures>(terrain));

            foreach (WeightedObjects<TerrainFeatures> terrain in possible_battle_locations)
            {
                terrain.Weight += terrain.Object.Modifiers.NaturalDefenceValue * 10;

                if (Province.Owner == InvolvedArmies[1].Owner)
                    terrain.Weight += terrain.Object.Modifiers.FortificationDefenceValue * 10;
            }

            Location = WeightedObjects<TerrainFeatures>.ChooseXHeaviestObjects(possible_battle_locations, 1)[0];
        }

        public void Fight()
        {
            for (int i = 0; i < _count; i++)
            {
                Modifiers[i] = InvolvedArmies[i].getTotalModifier();
                Score[i] = Constants.Random.Next(2, 13) + Modifiers[i];
            }

            // defender wins
            if (Score[0] <= Score[1])
            {
                InvolvedArmies[0].isScattered = true;
            }
            // attacker wins
            else
            {
                InvolvedArmies[1].isScattered = true;
            }

            Program.WorldHistory.AddRecord(RecordType.BattleReport, War, this, printBattle);
        }

        public static string printBattle(Record record)
        {
            string result = "";
            result += "Name: " + record.Battle.Name + "\n";
            result += "Province: " + record.Battle.Province.Name + "\n";
            result += "Terrain: " + record.Battle.Location.Name + "\n";
            result += "Attacker: " + record.Battle.InvolvedArmies[0].Name + "\n";
            result += "Attacker Bonus: " + record.Battle.Modifiers[0] + "\n";
            result += "Attacker Score: " + record.Battle.Score[0] + "\n";
            result += "Defender: " + record.Battle.InvolvedArmies[1].Name + "\n";
            result += "Defender Bonus: " + record.Battle.Modifiers[1] + "\n";
            result += "Defender Score: " + record.Battle.Score[1] + "\n";
            result += "Winner: ";
            if (record.Battle.InvolvedArmies[0].isScattered)
                result += record.Battle.InvolvedArmies[1] + "\n";
            else if (record.Battle.InvolvedArmies[1].isScattered)
                result += record.Battle.InvolvedArmies[0] + "\n";
            return result;
        }

        public Battle(string name, Deity creator, Army attacker, Army defender, Province province, War war) : base(name, creator)
        {
            War = war;
            Province = province;
            InvolvedArmies = new Army[2] { attacker, defender };
            Modifiers = new int[2] { 0, 0 };
            Score = new int[2] { 0, 0 };

            chooseBattleLocation();
        }
    }
}
