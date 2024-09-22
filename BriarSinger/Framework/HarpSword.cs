using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLocation = xTile.Dimensions.Location;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using SpaceCore;
using SpaceCore.VanillaAssetExpansion;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.Tools;


namespace BriarSinger.Framework;

[XmlType("Mods_Teoshen_Harpsword")]

public class HarpSword : MeleeWeapon
{
    protected static readonly SCApi SCApi = new();

    public HarpSword() : base(
        name: "HarpSword",
        upgradeLevel: 0,
        initialParentTileIndex: 0,
        indexOfMenuItemView: 0,
        stackable: false,
        numAttachmentSlots: 0);


    public new Item getOne()
    {
        return new HarpSword(); //add in the stuff for upgrades when that's implemented
    }
    protected override Item GetOneNew()
    {
        return new HarpSword();
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
        try
        {
            base.DoFunction(location, x, y, power, who);

            location.performToolAction(this, x / Game1.tileSize, y / Game1.tileSize); //figure out how far out this needs to ogo

        }
        catch (Exception ex)
        {
            ModEntry.ModMonitor.Log($"Unexpected error:\n\n{ex}", StardewModdingAPI.LogLevel.Error);
        }
    }
    public override bool onRelease(GameLocation location, int x, int y, Farmer who) => false;

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Microsoft.Xna.Framework.Color color, bool drawShadow)
    {
        spriteBatch.Draw(
            texture: this.GetTexture(),
            position: location + new Vector2(32f, 32f),
            new Rectangle(96, 16, 16, 16),
            color: color * transparency,
            rotation: 0f,
            new Vector2(8f, 8f),
            scale: Game1.pixelZoom * scaleSize,
            effects: SpriteEffects.None,
            layerDepth);
    }

    public virtual Texture2D GetTexture() => AssetManager.ToolTexture;
    protected override string loadDisplayName() => HarpSwordName();
    protected override string loadDescription() => HarpSwordDescription();

}

