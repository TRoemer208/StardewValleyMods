using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BriarSinger
{
    public class ModConfig
    {
        ///<summary>The button to cast Bolt.</summary>
        public SButton CastBoltButton { get; set; } = SButton.Q;

        public ModConfig() { }
    }
}
