using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Items
{
	public class CopperShortswordMouseControlled : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.CopperShortsword;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Shortsword");
			Tooltip.SetDefault("Holding this item summons a Copper Shortsword, which follows your cursor around");
		}
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
		float projectileSpeed = 8;
		Projectile projectile;
        public override void HoldItem(Player player)
        {
            if (projectile == null || !projectile.active)
            {
				int projectileID = Projectile.NewProjectile(Item.GetSource_ItemUse(Item),
                                          Main.MouseWorld,
                                          Vector2.Zero,
                                          ModContent.ProjectileType<Projectiles.CopperShortswordMouseControlled>(),
                                          Item.damage,
                                          Item.knockBack,
										  player.whoAmI);
				projectile = Main.projectile[projectileID];
            }
            else
            {
				projectile.timeLeft = 15;
				Vector2 diff = (Main.MouseWorld - projectile.Center);
				if (diff.Length() > 45)
					projectile.velocity = projectileSpeed * diff.SafeNormalize(Vector2.Zero);
            }
        }
    }
}