using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StardewModdingAPI;

namespace HotSpringRegeneration
{
    public class ModConfig
    {
        //The amount, as a percentage of the base value of 10 stamina per second, to modify the stamina regeneration.
        private int stamRegenMult = 100;
        public int StamRegenMult
        {
            get => this.stamRegenMult;
            set => this.stamRegenMult = Math.Clamp(value - value % 10, 10, 200);
        }


        //The amount, as a percentage of the base value of 10 health per second, to modify the health regeneration.
        private int healthRegenMult = 100;

        public int HealthRegenMult
        {
            get => this.healthRegenMult;
            set => this.healthRegenMult = Math.Clamp(value - value % 10, 10, 200);
        }

        public bool WorkOnlyInFarmCave
        {
            get; set;
        }
        public ModConfig() 
        {
            WorkOnlyInFarmCave = true;
        }
       
    }
}
