using wdfeerCrazyMod.Weapons;

namespace wdfeerCrazyMod;

internal class NPCShop : GlobalNPC
{
    public override void ModifyShop(Terraria.ModLoader.NPCShop shop)
    {
        if (shop.NpcType == NPCID.Steampunker)
        {
            shop.Add(ModContent.ItemType<Autobow>());
        }
    }
}
