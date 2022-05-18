using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Items
{
	public class EnchantedDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throws a magic enchanted dagger that homes in on your foes, kinda");
		}
		public override void SetDefaults()
		{
			Item.damage = 39;
			Item.crit = 12;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 5;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.scale = 0;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(silver: 50);
			Item.rare = 5;
			Item.UseSound = SoundID.Item19;
			Item.autoReuse = true;
			Item.shootSpeed = 16;
			Item.shoot = ModContent.ProjectileType<Projectiles.EnchantedDagger>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MagicDagger);
			recipe.AddIngredient(ItemID.SoulofLight, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
    }
}