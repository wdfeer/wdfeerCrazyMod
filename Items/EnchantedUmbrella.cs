using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Items
{
    internal class EnchantedUmbrella : ModItem
    {
        public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons an enchanted umbrella to protect you and throw itself at a nearby enemy\nDoesn't consume minion slots\nUp to three umbrellas can be active at once\nEach umbrella provides 5 defense");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 49;
			Item.knockBack = 7f;
			Item.mana = 13; // mana cost
			Item.width = 45;
			Item.height = 53;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.scale = 0;
			Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
			Item.value = Item.sellPrice(gold: 1, silver: 20);
			Item.rare = 3;
			Item.UseSound = SoundID.Item44; // What sound should play when using the item

			// These below are needed for a minion weapon
			Item.noMelee = true; // this item doesn't do any melee damage
			Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
			Item.buffType = ModContent.BuffType<Buffs.EnchantedUmbrellaBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<Projectiles.EnchantedUmbrella>(); // This item creates the minion projectile
		}
        public override bool CanUseItem(Player player)
        {
			int numOfUmbrellas = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.EnchantedUmbrella>()];
			if (numOfUmbrellas >= 3)
				return false;
			return true;
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			// Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
			position = player.position;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 2);

			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			return false;
		}
	}
}
