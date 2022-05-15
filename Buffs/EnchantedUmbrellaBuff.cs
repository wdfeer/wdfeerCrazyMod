using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Buffs
{
    internal class EnchantedUmbrellaBuff : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Umbrella");
			Description.SetDefault("The enchanted umbrella will protect you");

			Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
			Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			// If the minions exist reset the buff time, otherwise remove the buff from the player
			int numOfUmbrellas = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.EnchantedUmbrella>()];
			if (numOfUmbrellas > 0)
			{
				player.buffTime[buffIndex] = 18000;
				player.statDefense += 5 * numOfUmbrellas;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
