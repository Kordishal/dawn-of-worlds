using dawn_of_worlds.Names;
using dawn_of_worlds.WorldClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.Effects
{
    class Disease
    {

        public Name Name { get; set; }
        public int Virulence { get; set; } 
        public Modifier Effect { get; set; }

        public List<Province> AffectedProvinces { get; set; }

        public Disease(string name, int virulence, Modifier effect)
        {
            Name = new Name(name, name + "s", name.ToLower());
            Virulence = virulence;
            Effect = effect;
            AffectedProvinces = new List<Province>();
        }
    }

    struct Diseases
    {
        public static List<Disease> DiseasesList { get; set; }

        public static Disease Plague { get; set; }

        public static void DefineDiseases()
        {
            Plague = new Disease("The Plague", 10, new Modifier(ModifierCategory.Province, ModifierTag.ThePlague));

            DiseasesList = new List<Disease>() { Plague };
        }
    }
}
