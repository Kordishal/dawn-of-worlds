using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Inhabitants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Effects
{

    // TODO: Rework Modifiers to be loaded from file instead of code.
    // TODO: Expand Modifiers to subclass by category instead of one class for all.
    class Modifier
    {
        public ModifierCategory Category { get; set; }

        public ModifierTag Tag { get; set; }

        public List<ModifierTag> Excludes { get; set; }

        public List<string> Forbids { get; set; }

        public List<string> IncreasesWeight { get; set; }
        public List<string> DecreasesWeight { get; set; }

        public List<string> IncreaseCost { get; set; }
        public List<string> DecreaseCost { get; set; }

        public Modifier(ModifierCategory category, ModifierTag tag)
        {
            Tag = tag;
            Category = category;
            switch (Category)
            {
                case ModifierCategory.Domain:
                    defineDomains(tag);
                    break;
                case ModifierCategory.Province:
                    defineProvincialModifiers(tag);
                    break;
            }


            
        }

        private void defineDomains(ModifierTag tag)
        {
            switch (tag)
            {
                case ModifierTag.Architecture:                
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new List<string> { "construction" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "construction" };
                    break;
                case ModifierTag.Battle:                  
                    Excludes = new List<ModifierTag>() { ModifierTag.Peace };
                    Forbids = null;
                    IncreasesWeight = new List<string> { "military", "battle", "combat" };
                    DecreasesWeight = new List<string> { "peace" };

                    IncreaseCost = new List<string> { "peace" };
                    DecreaseCost = new List<string> { "battle" };
                    break;
                case ModifierTag.Cold:                 
                    Excludes = new List<ModifierTag>() { ModifierTag.Heat, ModifierTag.Fire };
                    Forbids = new List<string> { "heat" };
                    IncreasesWeight = new List<string> { "cold" };
                    DecreasesWeight = new List<string> { "fire", "fire" };

                    IncreaseCost = new List<string> { "cold" };
                    DecreaseCost = new List<string> { "heat" };
                    break;
                case ModifierTag.Community:                
                    Excludes = new List<ModifierTag>() { ModifierTag.Solitary };
                    Forbids = new List<string> { "solitary" };
                    IncreasesWeight = new List<string> { "community" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "community" };
                    break;
                case ModifierTag.Conquest:                
                    Excludes = new List<ModifierTag>() { ModifierTag.Peace };
                    Forbids = null;
                    IncreasesWeight = new List<string> { "military", "war", "expansion" };
                    DecreasesWeight = new List<string> { "peace" };

                    IncreaseCost = new List<string> { "peace" };
                    DecreaseCost = new List<string> { "expansion" };
                    break;
                case ModifierTag.Creation:                 
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new List<string> { "creation" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "creation" };
                    break;
                case ModifierTag.Drought:           
                    Excludes = new List<ModifierTag>() { ModifierTag.Water };
                    Forbids = new List<string> { "water" };
                    IncreasesWeight = new List<string> { "dry" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "dry" };
                    break;
                case ModifierTag.Earth:                   
                    Excludes = new List<ModifierTag>() { ModifierTag.Water };
                    Forbids = new List<string> { "water" };
                    IncreasesWeight = new List<string> { "earth" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "earth" };
                    break;
                case ModifierTag.Exploration:                
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new List<string> { "exploration", "expansion" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "exploration" };
                    break;
                case ModifierTag.Fire:                  
                    Excludes = new List<ModifierTag>() { ModifierTag.Water };
                    Forbids = new List<string> { "water" };
                    IncreasesWeight = new List<string> { "fire", "heat", "resurrection" };
                    DecreasesWeight = new List<string> { "cold" };

                    IncreaseCost = new List<string> { "cold" };
                    DecreaseCost = new List<string> { "fire" };
                    break;
                case ModifierTag.Heat:                   
                    Excludes = new List<ModifierTag>() { ModifierTag.Cold };
                    Forbids = new List<string> { "cold" };
                    IncreasesWeight = new List<string> { "heat", "fire" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "heat" };
                    break;
                case ModifierTag.Magic:
                    Excludes = new List<ModifierTag>() { ModifierTag.AntiMagic };
                    Forbids = new List<string> { "anti-magic" };
                    IncreasesWeight = new List<string> { "magic" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "magic" };
                    break;
                case ModifierTag.Metallurgy:
                    
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new List<string> { "metal" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "metal" };
                    break;
                case ModifierTag.Nature:                
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new List<string> { "nature" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "nature" };
                    break;
                case ModifierTag.Peace:                 
                    Excludes = new List<ModifierTag>() { ModifierTag.War };
                    Forbids = new List<string> { "war" };
                    IncreasesWeight = new List<string> { "peace", "alliance" };
                    DecreasesWeight = new List<string> { "military", "battle" };

                    IncreaseCost = new List<string> { "military" };
                    DecreaseCost = new List<string> { "peace" };
                    break;
                case ModifierTag.Pestilence:
                    
                    Excludes = new List<ModifierTag>() { ModifierTag.Health };
                    Forbids = new List<string> { "health" };
                    IncreasesWeight = new List<string> { "disease" };
                    DecreasesWeight = new List<string> { "healing" };

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "disease" };
                    break;
                case ModifierTag.Solitary:              
                    Excludes = new List<ModifierTag>() { ModifierTag.Community };
                    Forbids = null;
                    IncreasesWeight = new List<string> { "solitary", "isolation" };
                    DecreasesWeight = new List<string> { "community", "diplomacy" };

                    IncreaseCost = new List<string> { "community" };
                    DecreaseCost = new List<string> { "solitary" };
                    break;
                case ModifierTag.War:              
                    Excludes = new List<ModifierTag>() { ModifierTag.Peace };
                    Forbids = new List<string> { "peace" };
                    IncreasesWeight = new List<string> { "war", "miliary" };
                    DecreasesWeight = new List<string> { "alliance" };

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "war" };
                    break;
                case ModifierTag.Water:
                    
                    Excludes = new List<ModifierTag>() { ModifierTag.Fire };
                    Forbids = new List<string> { "fire" };
                    IncreasesWeight = new List<string> { "water" };
                    DecreasesWeight = new List<string> { "earth", "dry" };

                    IncreaseCost = new List<string> { "dry" };
                    DecreaseCost = new List<string> { "water" };
                    break;
                case ModifierTag.Air:             
                    Excludes = new List<ModifierTag>() { ModifierTag.Earth };
                    Forbids = new List<string> { "earth" };
                    IncreasesWeight = new List<string> { "air" };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new List<string> { "air" };
                    break;
            }
        }

        private void defineProvincialModifiers(ModifierTag tag)
        {
            switch (tag)
            {
                case ModifierTag.ThePlague:
                    Excludes = null;
                    Forbids = new List<string> { "creation" };
                    IncreasesWeight = null;
                    DecreasesWeight = new List<string> { "conquest", "construction", "army" };
                    IncreaseCost = null;
                    DecreaseCost = null;
                    break;
                case ModifierTag.Permafrost:
                    Excludes = null;
                    Forbids = new List<string> { "tree" };
                    IncreasesWeight = null;
                    DecreasesWeight = new List<string> { "life" };
                    IncreaseCost = null;
                    DecreaseCost = null;
                    break;
            }
        }

        public override string ToString()
        {
            return Tag.ToString();
        }
    }


    enum ModifierCategory
    {
        Domain,
        Province,
    }
}
