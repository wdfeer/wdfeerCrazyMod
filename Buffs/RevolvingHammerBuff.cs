namespace wdfeerCrazyMod.Buffs;

internal class RevolvingHammerBuff : ModBuff
{

    public override void Update(Player player, ref int buffIndex)
    {
        // If the minions exist reset the buff time, otherwise remove the buff from the player
        if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.RevolvingHammer>()] > 0)
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
