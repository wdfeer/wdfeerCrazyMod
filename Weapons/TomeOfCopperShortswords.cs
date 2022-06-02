using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Weapons
{
	public class TomeOfCopperShortswords : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Number of Copper Shortswords and their damage increases after defeating EoC, BoC or Eoc, Skeletron");
		}
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 2;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 56;
			Item.height = 60;
			Item.useTime = 13;
			Item.useAnimation = 13;
			Item.knockBack = 4;
			Item.value = Item.buyPrice(silver: 50);
			Item.rare = 2;
			{
				SoundStyle style = SoundID.Item8;
				style.Volume *= 0.5f;
				Item.UseSound = style;
			}
			Item.autoReuse = true;
			Item.shootSpeed = 12;
			Item.shoot = ModContent.ProjectileType<Projectiles.CopperShortswordTomeSword>();
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
		int GetExtraDamage()
        {
			int result = 0;
			if (NPC.downedSlimeKing)
				result++;
			if (NPC.downedBoss1)
				result += 3;
			if (NPC.downedBoss2)
				result += 3;
			if (NPC.downedBoss3)
				result += 3;
			return result;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 target = Main.MouseWorld;
			damage += GetExtraDamage();
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
				projectile.CritChance = player.GetWeaponCrit(Item);
			}
			return false;
		}
    }
}