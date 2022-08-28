using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Weapons;

	public class TrueTinShortsword : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 70;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.scale = 0;
			Item.width = 56;
			Item.height = 60;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.knockBack = 4;
			Item.value = Item.buyPrice(gold: 12);
			Item.rare = 9;
			Item.autoReuse = true;
			Item.shootSpeed = 12;
			Item.shoot = ModContent.ProjectileType<Projectiles.TrueTinShortswordProjectile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.EmpressBlade);
			recipe.AddIngredient(ItemID.TinShortsword);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
			Vector2 target = Main.MouseWorld;
			Vector2 defaultPosition = position;
			for (int k = -1; k < 2; k++)
			{
				Vector2 currentDirrection = Vector2.Normalize(velocity);
				currentDirrection += currentDirrection.RotatedBy(MathHelper.PiOver2) * k;
				position = defaultPosition;
				for ((int i, float d) = (0, position.Distance(target)); i < 20 && d > 24; (i, d) = (i + 1, position.Distance(target)))
				{
					position += currentDirrection * 48;
					Vector2 toTarget = target - position;
					currentDirrection = Vector2.Normalize(currentDirrection + toTarget * i / 2500);

					int projectileID = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
					Projectile projectile = Main.projectile[projectileID];
					projectile.rotation = currentDirrection.ToRotation() + MathHelper.PiOver4;
					if (d > 256)
						projectile.timeLeft = (int)(projectile.timeLeft * 256 / d);
				}
			}
			return false;
		}
}