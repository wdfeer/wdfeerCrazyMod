namespace wdfeerCrazyMod.Weapons;

public class CopperShortswordMouseControlled : ModItem
{
    public override string Texture => "Terraria/Images/Item_" + ItemID.CopperShortsword;
    public override void SetDefaults()
    {
        Item.damage = 19;
        Item.DamageType = DamageClass.Summon;
        Item.knockBack = 4;
        Item.value = Item.buyPrice(silver: 90);
        Item.rare = 3;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.CopperShortsword);
        recipe.AddIngredient(ItemID.TissueSample, 8);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();

        recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.CopperShortsword);
        recipe.AddIngredient(ItemID.ShadowScale, 8);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
    static Projectile[] projectiles = new Projectile[255];
    public override void HoldItem(Player player)
    {
        if (Main.myPlayer != player.whoAmI)
            return;
        var projectile = projectiles[player.whoAmI];
        if (projectile == null || !projectile.active || projectile.type != ModContent.ProjectileType<Projectiles.CopperShortswordMouseControlled>() || projectile.owner != player.whoAmI)
        {
            int projectileID = Projectile.NewProjectile(Item.GetSource_FromThis(),
                                  Main.MouseWorld,
                                  Vector2.Zero,
                                  ModContent.ProjectileType<Projectiles.CopperShortswordMouseControlled>(),
                                  Item.damage,
                                  Item.knockBack,
                                      player.whoAmI);
            projectiles[player.whoAmI] = Main.projectile[projectileID];
        }
    }
}