using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Items
{
	public class TomeOfCopperShortswords : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Unlimited blade works\nNumber of Copper Shortswords increases after defeating EoC, BoC or Eoc, Skeletron");
		}
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 2;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.scale = 0.5f;
			Item.width = 56;
			Item.height = 60;
			Item.useTime = 13;
			Item.useAnimation = 13;
			Item.knockBack = 4;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = 2;
			Item.UseSound = SoundID.Item37.WithVolume(0.4f).WithPitchVariance(-0.2f);
			Item.autoReuse = true;
			Item.shootSpeed = 12;
			Item.shoot = ModContent.ProjectileType<Projectiles.CopperShortsword>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperShortsword);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
		int GetNumberOfSwords()
        {
			int result = 2;
			if (NPC.downedBoss1)
				result++;
			if (NPC.downedBoss2)
				result++;
			if (NPC.downedBoss3)
				result++;
			return result;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 target = Main.MouseWorld;

			int numOfSwords = GetNumberOfSwords();
            for (int i = 0; i < numOfSwords; i++)
            {
				position = position + Main.rand.NextVector2Circular(100, 100);
				Dust d = Dust.NewDustPerfect(position, DustID.CopperCoin);
				d.noGravity = true;

				velocity = (target - position).SafeNormalize(Vector2.Zero) * velocity.Length();

				int projectileID = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				Projectile projectile = Main.projectile[projectileID];
				projectile.Center = position;
			}
			return false;
		}
    }
}