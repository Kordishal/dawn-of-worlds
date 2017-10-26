using dawn_of_worlds.Actors;
using dawn_of_worlds.CelestialPowers;
using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Inhabitants;
using dawn_of_worlds.Creations.Organisations;
using dawn_of_worlds.Log;
using System;
using System.Collections.Generic;

namespace dawn_of_worlds.Main
{
    class Simulation
    {
        public static TimeLine Time { get; set; }

        public Simulation()
        {
            Time = new TimeLine(837432);
        }

        /// <summary>
        /// Main loop which runs the simulation for TOTAL_TURNS.
        /// </summary>
        public void Run()
        {
            for (int i = 0; i < Constants.TOTAL_TURNS; i++)
            {
                Time.Advance();
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@  TURN " + i.ToString() + "  @@@@@@@@@@@@@@@@@@@@@@@@@@@");

                NewTurn();

                foreach (Deity deity in Program.State.Deities)
                {
                    deity.addPowerPoints();
                    deity.Turn();
                }

                RecordHistory();

                Program.Log.WriteToJson(Program.State.World, i, Program.Log.WorldDirectory + Program.State.World.Name + "_" + i + "_" + ".json");
                
                
            }
        }


        /// <summary>
        /// Any clean up code comes in here.
        /// </summary>
        public void NewTurn()
        {
            // all cities can now be used to raise armies again.
            foreach (City city in Program.State.Cities)
                city.Modifiers.not_hasRaisedArmy = true;

            // Remove all obsolete powers to save some computing time.
            List<Power> _powers_for_removal = new List<Power>();
            foreach (Deity deity in Program.State.Deities)
            {
                foreach (Power power in deity.Powers)
                    if (power.isObsolete)
                        _powers_for_removal.Add(power);

                foreach (Power power in _powers_for_removal)
                    deity.Powers.Remove(power);
            }
        }

        /// <summary>
        /// Record all of history.
        /// </summary>
        public void RecordHistory()
        {
            Program.WorldHistory.AddRecord(RecordType.TerrainMap, Map.generateTerrainMap(), Map.printMap);
            Program.WorldHistory.AddRecord(RecordType.BiomeMap, Map.generateBiomeMap(), Map.printMap);
            Program.WorldHistory.AddRecord(RecordType.ClimateMap, Map.generateClimateMap(), Map.printMap);

            foreach (Race race in Program.State.Races)
            {
                Program.WorldHistory.AddRecord(RecordType.RaceSettlementMap, race, Map.generateRaceSettlementMap(race), Map.printMap);
            }

            foreach (Civilisation nation in Program.State.Civilizations)
            {
                Program.WorldHistory.AddRecord(RecordType.NationTerritoryMap, nation, Map.generateNationTerritoryMap(nation), Map.printMap);
            }

            Program.WorldHistory.AddRecord(RecordType.GlobalTerritoryMap, Map.generateTerritoryMap(), Map.printMap);
        }
    }
}
