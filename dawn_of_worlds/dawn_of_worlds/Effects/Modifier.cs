using dawn_of_worlds.Creations.Civilisations;
using dawn_of_worlds.Creations.Inhabitants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Effects
{
    class Modifier
    {
        public ModifierCategory Category { get; set; }

        public ModifierTag Tag { get; set; }

        public ModifierTag[] Excludes { get; set; }

        public CreationTag[] Forbids { get; set; }

        public CreationTag[] IncreasesWeight { get; set; }
        public CreationTag[] DecreasesWeight { get; set; }

        public CreationTag[] IncreaseCost { get; set; }
        public CreationTag[] DecreaseCost { get; set; }

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
                    IncreasesWeight = new CreationTag[] { CreationTag.Construction };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Construction };
                    break;
                case ModifierTag.Battle:                  
                    Excludes = new ModifierTag[] { ModifierTag.Peace };
                    Forbids = null;
                    IncreasesWeight = new CreationTag[] { CreationTag.Military, CreationTag.Battle, CreationTag.Combat };
                    DecreasesWeight = new CreationTag[] { CreationTag.Peace };

                    IncreaseCost = new CreationTag[] { CreationTag.Peace };
                    DecreaseCost = new CreationTag[] { CreationTag.Battle };
                    break;
                case ModifierTag.Cold:                 
                    Excludes = new ModifierTag[] { ModifierTag.Heat, ModifierTag.Fire };
                    Forbids = new CreationTag[] { CreationTag.Heat };
                    IncreasesWeight = new CreationTag[] { CreationTag.Cold };
                    DecreasesWeight = new CreationTag[] { CreationTag.Fire, CreationTag.Heat };

                    IncreaseCost = new CreationTag[] { CreationTag.Cold };
                    DecreaseCost = new CreationTag[] { CreationTag.Heat };
                    break;
                case ModifierTag.Community:                
                    Excludes = new ModifierTag[] { ModifierTag.Solitary };
                    Forbids = new CreationTag[] { CreationTag.Solitary };
                    IncreasesWeight = new CreationTag[] { CreationTag.Community };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Community };
                    break;
                case ModifierTag.Conquest:                
                    Excludes = new ModifierTag[] { ModifierTag.Peace };
                    Forbids = null;
                    IncreasesWeight = new CreationTag[] { CreationTag.Military, CreationTag.War, CreationTag.Expansion };
                    DecreasesWeight = new CreationTag[] { CreationTag.Peace };

                    IncreaseCost = new CreationTag[] { CreationTag.Peace };
                    DecreaseCost = new CreationTag[] { CreationTag.Expansion };
                    break;
                case ModifierTag.Creation:                 
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new CreationTag[] { CreationTag.Creation };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Creation };
                    break;
                case ModifierTag.Drought:           
                    Excludes = new ModifierTag[] { ModifierTag.Water };
                    Forbids = new CreationTag[] { CreationTag.Water };
                    IncreasesWeight = new CreationTag[] { CreationTag.Dry };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Dry };
                    break;
                case ModifierTag.Earth:                   
                    Excludes = new ModifierTag[] { ModifierTag.Water };
                    Forbids = new CreationTag[] { CreationTag.Water };
                    IncreasesWeight = new CreationTag[] { CreationTag.Earth };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Earth };
                    break;
                case ModifierTag.Exploration:                
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new CreationTag[] { CreationTag.Exploration, CreationTag.Expansion };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Exploration };
                    break;
                case ModifierTag.Fire:                  
                    Excludes = new ModifierTag[] { ModifierTag.Water };
                    Forbids = new CreationTag[] { CreationTag.Water };
                    IncreasesWeight = new CreationTag[] { CreationTag.Fire, CreationTag.Heat, CreationTag.Resurrection };
                    DecreasesWeight = new CreationTag[] { CreationTag.Cold };

                    IncreaseCost = new CreationTag[] { CreationTag.Cold };
                    DecreaseCost = new CreationTag[] { CreationTag.Fire };
                    break;
                case ModifierTag.Heat:                   
                    Excludes = new ModifierTag[] { ModifierTag.Cold };
                    Forbids = new CreationTag[] { CreationTag.Cold };
                    IncreasesWeight = new CreationTag[] { CreationTag.Heat, CreationTag.Fire };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Heat };
                    break;
                case ModifierTag.Magic:                 
                    Excludes = new ModifierTag[] { ModifierTag.AntiMagic };
                    Forbids = new CreationTag[] { CreationTag.AntiMagic };
                    IncreasesWeight = new CreationTag[] { CreationTag.Magic };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Magic };
                    break;
                case ModifierTag.Metallurgy:
                    
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new CreationTag[] { CreationTag.Metal };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Metal };
                    break;
                case ModifierTag.Nature:                
                    Excludes = null;
                    Forbids = null;
                    IncreasesWeight = new CreationTag[] { CreationTag.Nature };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Nature };
                    break;
                case ModifierTag.Peace:                 
                    Excludes = new ModifierTag[] { ModifierTag.War };
                    Forbids = new CreationTag[] { CreationTag.War };
                    IncreasesWeight = new CreationTag[] { CreationTag.Peace, CreationTag.Alliance };
                    DecreasesWeight = new CreationTag[] { CreationTag.Military, CreationTag.Battle };

                    IncreaseCost = new CreationTag[] { CreationTag.Military };
                    DecreaseCost = new CreationTag[] { CreationTag.Peace };
                    break;
                case ModifierTag.Pestilence:
                    
                    Excludes = new ModifierTag[] { ModifierTag.Health };
                    Forbids = new CreationTag[] { CreationTag.Health };
                    IncreasesWeight = new CreationTag[] { CreationTag.Disease };
                    DecreasesWeight = new CreationTag[] { CreationTag.Healing };

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Disease };
                    break;
                case ModifierTag.Solitary:              
                    Excludes = new ModifierTag[] { ModifierTag.Community };
                    Forbids = null;
                    IncreasesWeight = new CreationTag[] { CreationTag.Solitary, CreationTag.Isolationism };
                    DecreasesWeight = new CreationTag[] { CreationTag.Community, CreationTag.Diplomacy };

                    IncreaseCost = new CreationTag[] { CreationTag.Community };
                    DecreaseCost = new CreationTag[] { CreationTag.Solitary };
                    break;
                case ModifierTag.War:              
                    Excludes = new ModifierTag[] { ModifierTag.Peace };
                    Forbids = new CreationTag[] { CreationTag.Peace };
                    IncreasesWeight = new CreationTag[] { CreationTag.War, CreationTag.Military };
                    DecreasesWeight = new CreationTag[] { CreationTag.Alliance };

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.War };
                    break;
                case ModifierTag.Water:
                    
                    Excludes = new ModifierTag[] { ModifierTag.Fire };
                    Forbids = new CreationTag[] { CreationTag.Fire };
                    IncreasesWeight = new CreationTag[] { CreationTag.Water };
                    DecreasesWeight = new CreationTag[] { CreationTag.Earth, CreationTag.Dry };

                    IncreaseCost = new CreationTag[] { CreationTag.Dry };
                    DecreaseCost = new CreationTag[] { CreationTag.Water };
                    break;
                case ModifierTag.Wind:             
                    Excludes = new ModifierTag[] { ModifierTag.Earth };
                    Forbids = new CreationTag[] { CreationTag.Earth };
                    IncreasesWeight = new CreationTag[] { CreationTag.Wind };
                    DecreasesWeight = null;

                    IncreaseCost = null;
                    DecreaseCost = new CreationTag[] { CreationTag.Wind };
                    break;
            }
        }

        private void defineProvincialModifiers(ModifierTag tag)
        {
            switch (tag)
            {
                case ModifierTag.ThePlague:
                    Excludes = null;
                    Forbids = new CreationTag[] { CreationTag.Creation };
                    IncreasesWeight = null;
                    DecreasesWeight = new CreationTag[] { CreationTag.Conquest, CreationTag.Construction, CreationTag.Army };
                    IncreaseCost = null;
                    DecreaseCost = null;
                    break;
                case ModifierTag.Permafrost:
                    Excludes = null;
                    Forbids = new CreationTag[] { CreationTag.Tree };
                    IncreasesWeight = null;
                    DecreasesWeight = new CreationTag[] { CreationTag.Life };
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
