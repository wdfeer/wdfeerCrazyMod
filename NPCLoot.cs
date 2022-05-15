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
        public override void ModifyNPCLoot(NPC npc, Terraria.ModLoader.NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.DarkCaster:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.EnchantedUmbrella>(), 14));
                    break;
                default:
                    break;
            }
        }
    }
}
