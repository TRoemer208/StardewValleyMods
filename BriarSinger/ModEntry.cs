using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewValley;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using SpaceCore;
using SpaceShared.APIs;
using SpaceShared.ConsoleCommands;

using BriarSinger.Spells.Components;
using BriarSinger.Framework;
using StardewValley.Enchantments;
using StardewValley.Monsters;


namespace BriarSinger
{
    public class ModEntry : Mod
    {
        public static IModHelper helper;
        private ModConfig Config;

        //Calling APIs and related content
        public static IManaBarApi ManaBarApi;
        public static ContentPatcher.IContentPatcherAPI ContentPatcherApi;
        public static string BriarSingerContentPatcherId = "Teoshen.CP.BriarSinger";

        /// <summary>
        /// Initalize all the classes in ModEntry
        /// </summary>
        /// <param name="helper"></param>
        public override void Entry(IModHelper helper)
        {
            ModEntry.helper = helper;

            LoadAssets();
            SetUpEvents();
            this.Config = this.Helper.ReadConfig<ModConfig>();
        }

        //Initialize mana bar
        private static Texture2D ManaBg;
        private static Texture2D ManaFg;

        //Defines the color of the mana bar and loads it.
        private static void LoadAssets()
        {
            ModEntry.ManaBg = helper.ModContent.Load<Texture2D>("assets/farmer/manabg.png");
            Color manaCol = new Color(120, 48, 255);
            ModEntry.ManaFg = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            ModEntry.ManaFg.SetData(new[] { manaCol });
        }


        //Sets up events for the mod.
        private void SetUpEvents()
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
        }

        ///<summary>Event called when the game launches to load content packs.</summary>
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
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
                configMenu.AddKeybind(
                      mod: this.ModManifest,
                      name: () => "Bolt",
                      tooltip: () => "The button to cast Bolt",
                      getValue: () => this.Config.CastBoltButton,
                      setValue: value => this.Config.CastBoltButton = value
                  );
            }

            //hook Mana Bar
            {
                var manaBar = this.Helper.ModRegistry.GetApi<IManaBarApi>("spacechase0.ManaBar");

                if (manaBar == null)
                {
                    this.Monitor.Log("No mana bar API", LogLevel.Error);
                    return;
                }

                ModEntry.ManaBarApi = manaBar;

            }

            //hook Content Patcher
            {
                var api = this.Helper.ModRegistry.GetApi<ContentPatcher.IContentPatcherAPI>("Pathoschild.ContentPatcher");
                ModEntry.ContentPatcherApi = api;
            }
        }

  

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.Name.IsEquivalentTo("BriarSinger/MyProjectile"))
            {
                e.LoadFromModFile<Texture2D>("assets/spellcomponents/boltprojectile.png", AssetLoadPriority.Medium);
            }
        }

        ///<summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        public void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (Game1.activeClickableMenu != null)
                return;
            var caster = Game1.player;

            if (Context.IsPlayerFree)
                if (e.Button == this.Config.CastBoltButton)
                {
                     CastBolt(caster);
                    }
            ModEntry.FixMana(Game1.player); //remove this after adding mana regen
        }

        //Bolt spell information
        private void CastBolt(Farmer caster)
        {
            Monster closestMonster = MonsterHelper.GetClosestMonsterToCursor();
            Vector2 startPos1 = TranslateVector(new Vector2(0, 96), caster.FacingDirection);
            int damage = 25; //replace this with the scaling factor when the profession levels are added

            Game1.currentLocation.projectiles.Add(new Bolt(damage, 0, 10, closestMonster.getStandingPosition() + new Vector2(-16, -256), 3, 10, true, true, caster.currentLocation, caster));
        }

        //Math to figure out if the game needs to change your direction based on where you cast your spell.
        public static Vector2 TranslateVector(Vector2 vector, int facingDirection)
        {
            float outx = vector.X;
            float outy = vector.Y;
            switch (facingDirection)
            {
                case 2:
                    break;
                case 3:
                    outx = -vector.Y;
                    outy = vector.X;
                    break;
                case 0:
                    outx = -vector.X;
                    outy = -vector.Y;
                    break;
                case 1:
                    outx = vector.Y;
                    outy = -vector.X;
                    break;
            }
            return new Vector2(outx, outy);
        }

        //Add some mana to make sure the mana bar will show up. Change this after mana is complete so that it refills your mana bar at the start of the day.
        public static void FixMana(Farmer player)
        {
            if (player.getMaxMana() <= 0)
            {
                player.setMaxMana(100);
                player.addMana(25);
            }
        }

    }
    
}
