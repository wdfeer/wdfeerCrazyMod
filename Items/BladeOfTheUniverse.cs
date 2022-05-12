using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Items
{
	public class BladeOfTheUniverse : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("This is a crazy modded sword.");
		}
		public override void SetDefaults()
		{
			Item.damage = 128;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 12;
			Item.value = 10000;
			Item.rare = 9;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shootSpeed = 12;
			Item.shoot = 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FragmentSolar, 16);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
		const int numOfProjectiles = 6;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int[] types = new int[numOfProjectiles] {ProjectileID.MoonlordArrow, ProjectileID.MoonlordBullet, ProjectileID.LunarFlare, ProjectileID.Meowmere, ProjectileID.CopperShortswordStab, ProjectileID.LastPrismLaser};
            for (int i = 0; i < numOfProjectiles; i++)
            {
				int projectileID = Projectile.NewProjectile(source, position, velocity, types[i], damage, knockback, player.whoAmI);
				Projectile projectile = Main.projectile[projectileID];
				projectile.velocity = velocity.RotatedByRandom(0.314) * Main.rand.NextFloat(1f, 1.1f);
				projectile.DamageType = DamageClass.Melee;
			}

			return false;
		}
    }
}