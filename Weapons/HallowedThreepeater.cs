using Terraria.DataStructures;

namespace wdfeerCrazyMod.Weapons;

public class HallowedThreepeater : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.HallowedRepeater);
        Item.damage /= 3;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.HallowedBar, 12);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }
    public override Vector2? HoldoutOffset()
    {
        return new Vector2(-3, 0);
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        {
            Vector2 muzzleOffset = velocity.SafeNormalize(Vector2.Zero) * Item.width;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;
        }
        for (int i = 0; i < 3; i++)
        {
            int projectileID = Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(14)), type, damage, knockback, player.whoAmI);
            Main.projectile[projectileID].noDropItem = true;
            Main.projectile[projectileID].CritChance = player.GetWeaponCrit(Item);
        }
        return false;
    }
}