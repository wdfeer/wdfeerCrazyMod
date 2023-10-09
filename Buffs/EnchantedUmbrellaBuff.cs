namespace wdfeerCrazyMod.Buffs;

internal class EnchantedUmbrellaBuff : ModBuff
{

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
