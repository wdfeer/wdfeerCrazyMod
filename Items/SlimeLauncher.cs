using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Items
{
	public class SlimeLauncher : ModItem
	{
        public override string Texture => "Terraria/Images/Item_" + ItemID.RocketLauncher;
        public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Launches hostile slimes at high velocity");
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.damage = 200;
			Item.DamageType = DamageClass.Ranged;
			Item.knockBack = 13;
			Item.rare = 7;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useAmmo = AmmoID.Gel;
			Item.shoot = 12;
			Item.shootSpeed = 24;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 19);
			recipe.AddIngredient(ItemID.VolatileGelatin);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
        public override Vector2? HoldoutOffset()
        {
			return new Vector2(0, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			{ 
				Vector2 muzzleOffset = velocity.SafeNormalize(Vector2.Zero) * Item.width;
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
					position += muzzleOffset;
			}
			NPC npc = NPC.NewNPCDirect(source, position, ModContent.NPCType<NPCs.SlimeLauncherSlime>());
			npc.velocity = velocity;
			npc.damage = damage;
			return false;
		}
    }
}