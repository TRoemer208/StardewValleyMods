﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using StardewValley.Util;

using BriarSinger.Framework;
using StardewValley.Tools;

namespace BriarSinger.Spells.Components
{
    public class StarShot : Projectile
    {
        private static IModHelper helper;
        public static Texture2D projectile;
        public static Texture2D tail;
        public readonly int damage = new int();

        public StarShot() { }

        public StarShot(int Damage, Vector2 velocity, Vector2 startingPosition, int taillength, GameLocation location = null, Character owner = null) : this()
        {
            this.damage = Damage;
            base.damagesMonsters.Value = true;
            this.damagesMonsters.Value = true;
            base.theOneWhoFiredMe.Set(location, owner);
            base.tailLength.Value = taillength;
            xVelocity.Value = velocity.X;
            yVelocity.Value = velocity.Y;
            base.position.Value = startingPosition;
            base.ignoreObjectCollisions.Value = true;

        }

        public override void draw(SpriteBatch b)
        {
            float projectile_scale = 2f * this.localScale;

            var angle = GetAngleForTexture(base.yVelocity.Value);

            projectile = Game1.content.Load<Texture2D>("BriarSinger/MyProjectile2");
            tail = Game1.content.Load<Texture2D>("BriarSinger/MyProjectile3");

            float tail_scale = 1.67f * this.localScale;
            var tailoffset = TailOffset(new Vector2(32f, 32f), base.xVelocity.Value, base.yVelocity.Value);

            b.Draw(
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

            //Draws the tail to the game world, centered on the projectile and with each tail slowly shrinking in size.
            for (int i = base.tail.Count - 1; i >= 0; i--)
            {
                b.Draw(
                    tail,
                    Game1.GlobalToLocal(Game1.viewport, Vector2.Lerp((i == base.tail.Count - 1) ? this.position.Value : base.tail.ElementAt(i + 1), base.tail.ElementAt(i), (float)base.tailCounter / 60f) + new Vector2(0f, 0f - this.height.Value) + tailoffset),
                    new Rectangle(0, 0, 24, 24),
                    this.color.Value,
                    angle,
                    new Vector2(8f, 8f),
                    tail_scale,
                    SpriteEffects.None,
                    (this.position.Y - (float)(base.tail.Count - i) + 96f) / 10000f);
            }

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

        public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
        {
            t.performUseAction(tileLocation);
            this.explosionAnimation(location);
        }

        public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
        {
            location.playSound("hitEnemy");
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
            multiplayer.broadcastSprites(location, new TemporaryAnimatedSprite("TileSheets\\animations", new Rectangle(128, 256, 64, 64), 60, 6, 1, base.position.Value, flicker: true, flipped: false));
            base.destroyMe = true;
            this.destroyMe = true;
            location.projectiles.RemoveWhere((Func<Projectile, bool>)(projectile =>
            {
                return projectile.destroyMe;
            }));
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
            return angle;
        }
        public static Vector2 TailOffset(Vector2 basevector, float xVelocity, float yVelocity)
        {
            if (yVelocity < 0f)
            {
                basevector.Y += 19f;
            }
            else if (yVelocity > 0f)
            {
                basevector.Y -= 19f;
            }

            if (xVelocity < 0f)
            {
                basevector.X += 19f;
                basevector.Y += 4f;
            }
            else if (xVelocity > 0f)
            {
                basevector.X -= 19f;
                basevector.Y += 4f;
            }
            return basevector;
        }


        public virtual Farmer GetPlayerWhoFiredMe(GameLocation location)
        {
            return (base.theOneWhoFiredMe.Get(location) as Farmer) ?? Game1.player;
        }


    }
}