using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace wdfeerCrazyMod.NPCs
{
	// This ModNPC serves as an example of a completely custom AI.
	public class SlimeLauncherSlime : ModNPC
	{
        public override string Texture => "Terraria/Images/NPC_" + NPCID.BlueSlime;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime");
			Main.npcFrameCount[NPC.type] = 2; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.BlueSlime);
			NPC.lifeMax = 120;
			NPC.life = 120;
			NPC.aiStyle = -1;
			NPC.friendly = true;
		}
        public override void AI()
        {
			NPC.life -= 2;
            for (int i = 0; i < Main.maxNPCs && NPC.life > 0; i++)
            {
				if (i == NPC.whoAmI)
					continue;
				NPC npc = Main.npc[i];
				if (npc.friendly || !npc.active)
					continue;
				if (npc.getRect().Intersects(NPC.getRect()))
                {
					npc.StrikeNPC(NPC.damage * NPC.life / NPC.lifeMax, NPC.velocity.Length() / 12, -1);
					NPC.StrikeNPC(npc.damage, 0, -1);
                }
            }
        }
    }
}
