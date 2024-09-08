﻿using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BriarSinger
{
    public class ModConfig
    {
        ///The button to cast Bolt.
        public SButton CastBoltButton { get; set; } = SButton.Q;

        ///The button to cast the currently active spell.
        public SButton CastSpellButton { get; set; } = SButton.G;

        public ModConfig() { }
    }
}
