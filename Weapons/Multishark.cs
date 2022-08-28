using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Weapons;

	public class Multishark : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("More shark");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Megashark);
			Item.crit += 10;
			Item.useAnimation -= 1;
			Item.useTime -= 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Megashark);
			recipe.AddIngredient(ItemID.SoulofLight, 9);
			recipe.AddIngredient(ItemID.SoulofNight, 9);
			recipe.AddCondition(Terraria.Localization.NetworkText.Empty, (recipe) => NPC.downedEmpressOfLight);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4,0);
		}	
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			{ 
				Vector2 muzzleOffset = velocity.SafeNormalize(Vector2.Zero) * Item.width;
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
					position += muzzleOffset;
			}
			for (int i = 0; i < 2; i++)
        {
				int projectileID = Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(8)), type, damage, knockback, player.whoAmI);
				Main.projectile[projectileID].CritChance = player.GetWeaponCrit(Item);
			}
			return false;
		}
}