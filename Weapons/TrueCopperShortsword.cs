using System.Linq;
using Terraria.DataStructures;

namespace wdfeerCrazyMod.Weapons;

public class TrueCopperShortsword : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 90;
		Item.DamageType = DamageClass.Melee;
		Item.noMelee = true;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.scale = 0;
		Item.width = 56;
		Item.height = 60;
		Item.useTime = 13;
		Item.useAnimation = 13;
		Item.knockBack = 4;
		Item.value = Item.buyPrice(gold: 12);
		Item.rare = 9;
		Item.autoReuse = true;
		Item.shootSpeed = 12;
		Item.shoot = ModContent.ProjectileType<Projectiles.TrueCopperShortswordProjectile>();
	}

	public override void AddRecipes()
	{
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.EmpressBlade);
		recipe.AddIngredient(ItemID.CopperShortsword);
		recipe.AddTile(TileID.LunarCraftingStation);
		recipe.Register();
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		NPC target = FindTarget(player);
		if (target == null)
			return true;
		Vector2 targetCenter = target.Center;
		Vector2 currentDirrection = Vector2.Normalize(velocity);
		float homingDenominator = Main.rand.Next(600, 1200);
		for (int i = 0; i < 25 && position.Distance(targetCenter) > 24; i++)
		{
			position += currentDirrection * 48;
			Vector2 toTarget = targetCenter - position;
			currentDirrection = Vector2.Normalize(currentDirrection + toTarget * i / homingDenominator);

			int projectileID = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
			Projectile projectile = Main.projectile[projectileID];
			projectile.CritChance = player.GetWeaponCrit(Item);
			projectile.rotation = currentDirrection.ToRotation() + MathHelper.PiOver4;
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)MessageType.ProjectileRotation);
                packet.Write(projectileID);
                packet.Write(projectile.rotation);
                packet.Send();
            }
        }
		return false;
	}
	private NPC FindTarget(Player player)
	{
		NPC target = Main.npc.Where(npc => npc.CanBeChasedBy() && !npc.friendly).MinBy(npc => (npc.Center - player.Center).Length());
		if (target != null && target.Center.Distance(player.Center) < 840)
			return target;
		return null;
	}
}