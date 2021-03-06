using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Accessories
{
    internal class PlutoniumAmulet : ModItem
    {
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("+25% damage dealt\n+25% damage taken");
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Terraria.Item.buyPrice(gold: 2);
			Item.rare = 5;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Generic) += 0.25f;
			player.endurance -= 0.25f;
		}
	}
}
