using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StardewValley;
using StardewValley.Util;
using StardewValley.Monsters;

namespace BriarSinger.Framework
{
    
    public static class MonsterHelper
    {
        public static Monster GetClosestMonsterToCursor()
        {
            Monster closestMonsterCursor = Utility.getClosestMonsterWithinRange(Game1.currentLocation, Game1.currentCursorTile*64, 128);

            if (closestMonsterCursor == null)
            {
                Game1.showRedMessage("No enemies in range");
            }

            return closestMonsterCursor;
        }
    }

}
