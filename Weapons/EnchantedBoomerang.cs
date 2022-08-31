using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Weapons;

internal class EnchantedBoomerang : ModItem
{
    public override string Texture => "Terraria/Images/Item_" + ItemID.EnchantedBoomerang;
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Summons an enchanted boomerang to fight for you");

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 17;
        Item.knockBack = 3f;
        Item.mana = 15; // mana cost
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
        Item.value = Terraria.Item.sellPrice(silver: 60);
        Item.rare = 2;
        Item.UseSound = SoundID.Item44; // What sound should play when using the item

        // These below are needed for a minion weapon
        Item.noMelee = true; // this item doesn't do any melee damage
        Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
        Item.buffType = ModContent.BuffType<Buffs.EnchantedBoomerangBuff>();
        // No buffTime because otherwise the item tooltip would say something like "1 minute duration"
        Item.shoot = ModContent.ProjectileType<Projectiles.EnchantedBoomerang>(); // This item creates the minion projectile
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
            .AddIngredient(ItemID.EnchantedBoomerang)
            .AddIngredient(ItemID.TissueSample, 8)
            .AddTile(TileID.Anvils)
            .Register();
        CreateRecipe()
            .AddIngredient(ItemID.EnchantedBoomerang)
            .AddIngredient(ItemID.ShadowScale, 8)
            .AddTile(TileID.Anvils)
            .Register();
        CreateRecipe()
            .AddIngredient(ItemID.Wood, 12)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.TissueSample, 8)
            .AddTile(TileID.WorkBenches)
            .Register();
        CreateRecipe()
            .AddIngredient(ItemID.Wood, 12)
            .AddIngredient(ItemID.FallenStar, 5)
            .AddIngredient(ItemID.ShadowScale, 8)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
