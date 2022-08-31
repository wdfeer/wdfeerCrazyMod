using Terraria;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Buffs;

internal class EnchantedBoomerangBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Enchanted Boomerang");
        Description.SetDefault("The enchanted boomerang will fight for you");

        Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
    }

    public override void Update(Player player, ref int buffIndex)
    {
        // If the minions exist reset the buff time, otherwise remove the buff from the player
        if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.EnchantedBoomerang>()] > 0)
        {
            player.buffTime[buffIndex] = 18000;
        }
        else
        {
            player.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
