using wdfeerCrazyMod.Weapons;

namespace wdfeerCrazyMod;

internal class NPCShop : GlobalNPC
{
    public override void SetupShop(int type, Chest shop, ref int nextSlot)
    {
        if (type == NPCID.Steampunker)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Autobow>());
            nextSlot++;
        }
    }
}
