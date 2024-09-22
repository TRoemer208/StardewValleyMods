using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Netcode;

using StardewValley.Constants;
using StardewValley.GameData.LocationContexts;
using StardewValley.Extensions;


using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using SpaceShared.APIs;



namespace HotSpringRegeneration
{
    public class ModEntry : Mod
    {


        private ModConfig Config;
        public int counter;
        public double timer = 0;
        public int lastCounter = 0;

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            Helper.Events.Player.Warped += Player_Warped;
            Helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;
        }

        private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
                return;
            {
                configMenu.Register(
                    mod: this.ModManifest,
                    reset: () => this.Config = new ModConfig(),
                    save: () => this.Helper.WriteConfig(this.Config)
                    );
                configMenu.AddBoolOption(
      mod: this.ModManifest,
      name: () => "Work only in the farm cave?",
      tooltip: () => "Limit the changes to only the farm cave hot springs.",
      getValue: () => this.Config.WorkOnlyInFarmCave,
      setValue: value => this.Config.WorkOnlyInFarmCave = value
      );
                configMenu.AddNumberOption(
             mod: this.ModManifest,
             name: () => "Stamina Regeneration Multiplier",
             getValue: () => this.Config.StamRegenMult,
             setValue: value => this.Config.StamRegenMult = value,
             min: 10,
             max: 200,
             interval: 10

   );
                configMenu.AddNumberOption(
             mod: this.ModManifest,
             name: () => "Health Regeneration Multiplier",
             getValue: () => this.Config.HealthRegenMult,
             setValue: value => this.Config.HealthRegenMult = value,
             min: 10,
             max: 200,
             interval: 10

   );

            }
        }

        private void Player_Warped(object? sender, WarpedEventArgs e)
        {
            if (Game1.currentLocation.NameOrUniqueName != "FarmCave" && Config.WorkOnlyInFarmCave == true)
            {
                Helper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked;
                return;
            }
            else if ((Config.WorkOnlyInFarmCave == false) || (Config.WorkOnlyInFarmCave == true && Game1.currentLocation.NameOrUniqueName == "FarmCave"))
            {
                Helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;

            }
        }

        private void GameLoop_UpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            Farmer player = Game1.player;
            var location = Game1.currentLocation;
            double currentTime = Game1.currentGameTime.TotalGameTime.TotalMilliseconds;
            bool isPaused = !Game1.shouldTimePass();



            if (player == null || isPaused)
            {
                return;
            }

            if (!Game1.eventUp && (Game1.activeClickableMenu == null || Game1.IsMultiplayer) && !Game1.paused)
            {
                timer += Game1.currentGameTime.ElapsedGameTime.TotalMilliseconds;
                if (timer >= 100)
                {
                    counter++;

                    if (counter > lastCounter)
                    {
                        ChangeStaminaRegen(player, location, currentTime);
                        ChangeHealthRegen(player, location, currentTime);
                    }

                    lastCounter = counter;
                    timer = 0;
                }


            }
        }


        private void ChangeStaminaRegen(Farmer player, GameLocation location, double currentTime)
        {

            if (Config.WorkOnlyInFarmCave == true && Game1.currentLocation.NameOrUniqueName != "FarmCave" || Config.StamRegenMult == 100 || (counter % 10 == 0))
            {
                return;
            }

            else if (Game1.currentLocation.NameOrUniqueName == "FarmCave" && Game1.player.swimming.Value == true || Config.WorkOnlyInFarmCave == false && Game1.player.swimming.Value == true)
            {
                if (player.stamina < 20 || player.stamina >= player.maxStamina)
                {
                    return;
                }
                //adds stamina if multi is over 100
                if (Config.StamRegenMult > 100)
                {
                    if (counter % 10 < ((Config.StamRegenMult - 100) / 10))
                            {
                        player.stamina++;
                    }
                }
                //subtracts stamina if multi is under 100
                if (Config.StamRegenMult < 100)
                {
                    if (counter % 10 <= (Config.StamRegenMult / 10))
                    {
                        return;
                    }
                    else player.stamina--;
                }
            }
        }



        private void ChangeHealthRegen(Farmer player, GameLocation location, double currentTime)
        {

            if (Config.WorkOnlyInFarmCave == true && Game1.currentLocation.NameOrUniqueName != "FarmCave" || Config.HealthRegenMult == 100 || (counter % 10 == 0))
            {
                return;
            }

            else if (Game1.currentLocation.NameOrUniqueName == "FarmCave" && Game1.player.swimming.Value == true || Config.WorkOnlyInFarmCave == false && Game1.player.swimming.Value == true)
            {
                if (player.health < 20 || player.health >= player.maxHealth)
                {
                    return;
                }
                //adds health if multi is over 100
                if (Config.HealthRegenMult > 100)
                {
                    if (counter % 10 < ((Config.HealthRegenMult - 100) / 10))
                    {
                        player.health++;
                    }
                }
                //subtracts health if multi is under 100
                if (Config.HealthRegenMult < 100)
                {
                    if (counter % 10 <= (Config.HealthRegenMult / 10))
                    {
                        return;
                    }
                    else player.health--;
                }
            }
        }
    }
}
