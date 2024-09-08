﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using StardewValley;
using StardewModdingAPI;
using SpaceShared.APIs;
using BriarSinger;
using BriarSinger.Spells;
using SpaceShared;

namespace BriarSinger.Framework
{
    internal static class ManaApi
    {
        public static int GetCurrentMana(this Farmer player)
        {
            return ModEntry.ManaBarApi.GetMana(player);
        }
        public static void addMana(this Farmer player, int amt)
        {
            ModEntry.ManaBarApi.AddMana(player, amt);
        }

        public static int getMaxMana(this Farmer player)
        {
            return ModEntry.ManaBarApi.GetMaxMana(player);
        }

        public static void setMaxMana(this Farmer player, int newCap)
        {
            ModEntry.ManaBarApi.SetMaxMana(player, newCap);
        }

       
        
    }
   
}
