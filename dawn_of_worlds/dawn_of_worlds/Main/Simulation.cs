using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.Creations.Diplomacy;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Log;
using dawn_of_worlds.Names;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Main
{
    class Simulation
    {
        public static TimeLine Time { get; set; }

        public Simulation()
        {
            Time = new TimeLine();
        }

        public void Run()
        {
            for (int i = 0; i < Constants.TOTAL_TURNS; i++)
            {
                Time.Advance();
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@  TURN " + i.ToString() + "  @@@@@@@@@@@@@@@@@@@@@@@@@@@");
                foreach (Deity deity in Program.World.Deities)
                {
                    deity.calculatePowerPoints();

                    foreach (City city in deity.FoundedCities)
                    {
                        city.Modifiers.not_hasRaisedArmy = true;
                    }
                }


                for (int j = 0; j < Constants.DEITY_ACTIONS_PER_TURN; j++)
                {
                    foreach (Deity deity in Program.World.Deities)
                    {
                        deity.Turn();
                    }
                }


                foreach (Deity deity in Program.World.Deities)
                {
                    for (int j = 0; j < deity.Powers.Count; j++)
                    {
                        if (deity.Powers[j].isObsolete)
                        {
                            deity.Powers.Remove(deity.Powers[j]);
                            j -= 1;
                        }
                    }
                }

                Program.WorldHistory.AddRecord(RecordType.TerrainMap, Map.generateTerrainMap(), Map.printMap);
                Program.WorldHistory.AddRecord(RecordType.BiomeMap, Map.generateBiomeMap(), Map.printMap);
                Program.WorldHistory.AddRecord(RecordType.ClimateMap, Map.generateClimateMap(), Map.printMap);

                foreach (Race race in Program.World.Races)
                {
                    Program.WorldHistory.AddRecord(RecordType.RaceSettlementMap, race, Map.generateRaceSettlementMap(race), Map.printMap);
                }

                foreach (Nation nation in Program.World.Nations)
                {
                    Program.WorldHistory.AddRecord(RecordType.NationTerritoryMap, nation, Map.generateNationTerritoryMap(nation), Map.printMap);
                }

                Program.WorldHistory.AddRecord(RecordType.GlobalTerritoryMap, Map.generateTerritoryMap(), Map.printMap);
            }
        }
    }
}
