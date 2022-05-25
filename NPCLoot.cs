using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod
{
    internal class NPCLoot : GlobalNPC
    {
        public IItemDropRule GetItemDropRuleForType(int type)
        {
            switch (type)
            {
                case NPCID.DarkCaster:
                    return ItemDropRule.Common(ModContent.ItemType<Weapons.EnchantedUmbrella>(), 14);
                case NPCID.MossHornet or NPCID.BigMossHornet or NPCID.GiantMossHornet or NPCID.TinyMossHornet or NPCID.LittleMossHornet:
                    return ItemDropRule.Common(ModContent.ItemType<Accessories.PlutoniumAmulet>(), 40);
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
}
