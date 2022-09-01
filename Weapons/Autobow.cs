using Terraria.DataStructures;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Weapons;

internal class Autobow : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Throws a bow that shoots at enemies rapidly while it flies\nArrows deal 50% of the damage");
    }

    public override void SetDefaults()
    {
        Item.damage = 80;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 9f;
        Item.width = 18;
        Item.height = 36;
        Item.useTime = 48;
        Item.useAnimation = 48;
        Item.scale = 0;
        Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
        Item.value = Item.sellPrice(gold: 15);
        Item.rare = 5;
        Item.UseSound = SoundID.Item1; // What sound should play when using the item
        Item.shoot = ModContent.ProjectileType<AutobowProjectile>();
        Item.shootSpeed = 10;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        return false;
    }
}
