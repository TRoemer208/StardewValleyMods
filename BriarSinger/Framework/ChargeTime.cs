using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;

using StardewValley;
using StardewValley.Tools;

using BriarSinger;
using Microsoft.Xna.Framework.Graphics;


namespace BriarSinger.Framework
{
    public class ChargeTime
    {

        public bool isCharging;
        private static bool chargeComplete;
        ModEntry modEntryInstance = new ModEntry();

        internal static float GetChargeTime()
        {
            var caster = Game1.player;
           
           
            if (caster.CurrentTool?.Name == "HarpSword" && ModEntry.IsActionButtonDown() == true)
            {
                if (chargeComplete == true)
                    {

                    return 1f;
                }
                var requiredChargeTime = 750;
                var chargeStartTime = Game1.currentGameTime.TotalGameTime.TotalSeconds * 1000;

                return Utility.Clamp((float)((Game1.currentGameTime.TotalGameTime.TotalMilliseconds - chargeStartTime) / (double)requiredChargeTime), 0f, 1f);

            }
            return 0f;
        }

        public static void TickUpdate(Farmer who)
        {
            
           if (who.CurrentTool?.Name != "HarpSword")
            {
                return;
            }
            if (who.IsLocalPlayer)
            {
                    var currentChargeTime = GetChargeTime();

                    if (currentChargeTime >= 1f)
                    {
                        chargeComplete = true;
                        Game1.currentLocation.playSound("select");
                    }
                    else if (currentChargeTime < 1f)
                    {
                        chargeComplete = false;
                    }
            }

            //track how long the charging has been going on here and play a sound and the animation when it's done.
        }

        public void ReleaseCharge()
        {
            //links to bool in buttonreleased event
            if (ModEntry.IsActionButtonDown() == false)
            {
               
                if (isCharging)
                {
                    if (chargeComplete)
                    {
                        var caster = Game1.player;
                        modEntryInstance.CastStarShot(caster);
                    }
                    ResetCharge();
                }
            }
        }

        public void WhileCharging()
        {
            if (ModEntry.IsActionButtonDown() == true)
            {
                isCharging = true;
                chargeComplete = false;
                Game1.player.CanMove = false;
            }
        }

        private void ResetCharge()
        {
            isCharging = false;
            chargeComplete = false;
            Game1.player.CanMove = true;
        }

        public static void DrawReticle(SpriteBatch b)
        {
            Point mousePos = Game1.getMousePosition();
            Vector2 mousePosVector = new Vector2(mousePos.X, mousePos.Y);
            b.Draw(Game1.mouseCursors,
                mousePosVector,
                Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 43),
                Color.White,
                0f,
                new Vector2(32f, 32f),
                1f,
                SpriteEffects.None,
                0.999999f);
        }

        //create a "chargetime" thing that counts up as the action button is held down. After 750ms, it is charged, plays a sound, and releasing it will shoot the starshot. Try to add a little sparkly animation so the player knows it's charged.
        //also see if you can override the default right click of the harpsword and change the special attack cooldown.

    }
}
