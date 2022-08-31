using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Weapons;

internal class OrbOfMagic : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Summons an orb of magic to fight for you");

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 99;
        Item.knockBack = 2f;
        Item.mana = 24; // mana cost
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.scale = 0;
        Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
        Item.value = Terraria.Item.sellPrice(gold: 20);
        Item.rare = 9;
        Item.UseSound = SoundID.Item44; // What sound should play when using the item

        // These below are needed for a minion weapon
        Item.noMelee = true; // this item doesn't do any melee damage
        Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
        Item.buffType = ModContent.BuffType<Buffs.OrbOfMagicBuff>();
        // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
        Item.shoot = ModContent.ProjectileType<Projectiles.OrbOfMagic>(); // This item creates the minion projectile
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
        position = Main.MouseWorld;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        player.AddBuff(Item.buffType, 2);

        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;

        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FragmentStardust, 8)
            .AddIngredient(ItemID.SpectreBar, 12)
            .AddTile(TileID.LunarCraftingStation)
            .Register();
    }
}
