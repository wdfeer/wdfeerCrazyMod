using Terraria.GameContent.ItemDropRules;
using wdfeerCrazyMod.Accessories;
using wdfeerCrazyMod.Weapons;

namespace wdfeerCrazyMod;

internal class NPCLoot : GlobalNPC
{
    public IItemDropRule GetItemDropRuleForType(int type)
    {
        switch (type)
        {
            case NPCID.EyeofCthulhu:
                return ItemDropRule.NotScalingWithLuck(ModContent.ItemType<WaterOrb>(), 6);
            case NPCID.DarkCaster:
                return ItemDropRule.Common(ModContent.ItemType<EnchantedUmbrella>(), 14);
            case NPCID.MossHornet or NPCID.BigMossHornet or NPCID.GiantMossHornet or NPCID.TinyMossHornet or NPCID.LittleMossHornet:
                return ItemDropRule.Common(ModContent.ItemType<PlutoniumAmulet>(), 40);
            case NPCID.ChaosElemental:
                return ItemDropRule.Common(ModContent.ItemType<ChaosInABottle>(), 50);
            case NPCID.TheDestroyer or NPCID.SkeletronPrime:
                return ItemDropRule.NotScalingWithLuck(ModContent.ItemType<DangerlessAmulet>(), 4);
            case NPCID.HallowBoss:
                return ItemDropRule.NotScalingWithLuck(ModContent.ItemType<BulletHell>(), 3);
            default:
                return null;
        }
    }
    public override void ModifyNPCLoot(NPC npc, Terraria.ModLoader.NPCLoot npcLoot)
    {
        var dropRule = GetItemDropRuleForType(npc.type);
        if (dropRule != null)
            npcLoot.Add(dropRule);
    }
}
