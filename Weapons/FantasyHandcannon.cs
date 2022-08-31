using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Weapons;

public class FantasyHandcannon : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Shoots fantasy bullets\nSuccessful hits spawn a swarm of fantasy bullets, each dealing 40% damage");
    }
    public override void SetDefaults()
    {
        Item.damage = 88;
        Item.crit = 20;
        Item.DamageType = DamageClass.Ranged;
        Item.noMelee = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.scale = 0.75f;
        Item.useTime = 56;
        Item.useAnimation = 56;
        Item.knockBack = 9;
        Item.value = Item.buyPrice(gold: 4);
        Item.rare = 5;
        Item.UseSound = SoundID.Item40;
        Item.autoReuse = false;
        Item.shootSpeed = 16;
        Item.shoot = ModContent.ProjectileType<Projectiles.FantasyBulletProjectileInitial>();
        Item.useAmmo = AmmoID.Bullet;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.PhoenixBlaster);
        recipe.AddIngredient(ItemID.SoulofLight, 9);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FantasyBulletProjectileInitial>(), damage, knockback, player.whoAmI);
        return false;
    }
}