using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Weapons;

	public class Capper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("C.A.P.P.E.R");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SpaceGun);
			Item.damage *= 16 / Item.mana * 2;
			Item.mana = 16;
			Item.width = 33;
			Item.height = 22;
			Item.useTime = 9;
			Item.useAnimation = 9;
			Item.value += 30000;
			Item.rare = 4;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpaceGun);
			recipe.AddIngredient(ItemID.SoulofLight, 9);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			proj.usesLocalNPCImmunity = true;
			proj.localNPCHitCooldown = -1;
			return false;
		}
	}