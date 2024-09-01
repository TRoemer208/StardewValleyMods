/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;

using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using StardewValley.Util;

using BriarSinger.Framework;

namespace BriarSinger.Spells.Components
{
    public class StarShot : Projectile
    {
        private static IModHelper helper;
        public static Texture2D projectile;
        public readonly int damage = new int();

       public StarShot() { }

        public StarShot(int Damage, float xVelocity, float yVelocity, Vector2 startingPosition, int taillength, int Velocity, bool Explodes, bool isSeeking, GameLocation location = null, Character owner = null) : this()
        {
            this.damage = Damage;
            base.damagesMonsters.Value = true;
            this.damagesMonsters.Value = true;
            base.currentTileSheetIndex.Value = 1;
            base.theOneWhoFiredMe.Set(location, owner);
            base.tailLength.Value = taillength;
            base.xVelocity.Value = xVelocity;
            base.yVelocity.Value = yVelocity;
            base.position.Value = startingPosition;
            base.ignoreObjectCollisions.Value = true;

        }
        
        public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
        {
            t.performUseAction(tileLocation);
            this.explosionAnimation(location);
        }
     
        public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
        {
            location.playSound("thunder_small");
            Farmer player = this.GetPlayerWhoFiredMe(location);
            this.explosionAnimation(location);
            if (n is Monster)
            {
                location.damageMonster(n.GetBoundingBox(), this.damage, this.damage, isBomb: false, player, isProjectile: true);
            }
            base.destroyMe = true;
            this.destroyMe = true;
        }
           private void explosionAnimation(GameLocation location)
           {
            Multiplayer multiplayer = Game1.Multiplayer;
            multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(448, 256, 64, 64), 60, 5, 1, base.position.Value + new Vector2(16, 256), flicker: false, flipped: false));
               base.destroyMe = true;
               this.destroyMe = true;
           }
      
        public override void behaviorOnCollisionWithPlayer(GameLocation location, Farmer player)
        {
            if ((bool)base.damagesMonsters.Value)
            {
                return;
            }
            this.explosionAnimation(location);
        }
        public override void behaviorOnCollisionWithOther(GameLocation location)
        {
            if (base.ignoreObjectCollisions.Value == false)
            {
                this.explosionAnimation(location);
            }
        }
        public override void updatePosition(GameTime time)
        {
            base.position.X += xVelocity.Value;
            base.position.Y += yVelocity.Value;
        }


        public static float GetAngleForTexture(float yVelocity)
        {
            var angle = 0f;

            if (yVelocity > 0f)
            {
                angle = (float)Math.PI / 2.0f; // 90 degrees
            }
            else if (yVelocity < 0f)
            {
                angle = (float)Math.PI / -2.0f; // 270 degrees
            }
            return angle;
        }



        public virtual Farmer GetPlayerWhoFiredMe(GameLocation location)
        {
            return (base.theOneWhoFiredMe.Get(location) as Farmer) ?? Game1.player;
        }

        public override void draw(SpriteBatch b)
        {
            float projectile_scale = 2f * this.localScale;
          
            var angle = GetAngleForTexture(base.yVelocity.Value);
           
            projectile = Game1.content.Load<Texture2D>("BriarSinger/MyProjectile");


            b.Draw(
              //  Projectile.projectileSheet,
                projectile,
                Game1.GlobalToLocal(Game1.viewport, this.position.Value + new Vector2(0f, 0f - this.height.Value) + new Vector2(32f, 32f)),
                new Rectangle(0, 0, 24, 24),
                this.color.Value,
                angle,
                new Vector2(8f, 8f),
                projectile_scale,
                SpriteEffects.None,
                (this.position.Value.Y + 96f) / 100000f
                );

            if (this.height.Value > 0f)
            {
                b.Draw(Game1.shadowTexture,
                    Game1.GlobalToLocal(Game1.viewport, this.position.Value + new Vector2(32f, 32f)),
                    Game1.shadowTexture.Bounds,
                    Color.White * 0.75f, 0f,
                    new Vector2(Game1.shadowTexture.Bounds.Center.X,
                    Game1.shadowTexture.Bounds.Center.Y), 2f,
                    SpriteEffects.None,
                    (this.position.Y - 1f) / 10000f);
            }

        }


    }
}
*/