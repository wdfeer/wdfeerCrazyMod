using Terraria.DataStructures;

namespace wdfeerCrazyMod.Accessories;

internal class ChaosInABottle : ModItem
{
    protected virtual float ChaosStateDurationMult => 0.75f;
    protected virtual float IncomingDamageMult => 1.25f;
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.value = Terraria.Item.buyPrice(gold: 7);
        Item.rare = 5;
        Item.accessory = true;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var modPlayer = player.GetModPlayer<ChaosInABottlePlayer>();
        modPlayer.enabled = true;
        modPlayer.chaosStateDurationMult = ChaosStateDurationMult;
        modPlayer.incomingDamageMult = IncomingDamageMult;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.ChaosElementalBanner);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }
    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.ModItem is ChaosInABottle && incomingItem.ModItem is ChaosInABottle)
            return false;
        return true;
    }
}
class ChaosInABottlePlayer : ModPlayer
{
    public bool enabled;
    public float chaosStateDurationMult;
    public float incomingDamageMult;
    public override void ResetEffects()
    {
        enabled = false;
        chaosStateDurationMult = 1f;
        incomingDamageMult = 1f;
    }
    bool chaosModified = false;
    public override void PostUpdateEquips()
    {
        if (!enabled)
            return;
        if (Player.HasBuff(BuffID.ChaosState))
        {
            if (!chaosModified)
            {
                ref int buffTime = ref Player.buffTime[Player.FindBuffIndex(BuffID.ChaosState)];
                buffTime = (int)(buffTime * chaosStateDurationMult);
                chaosModified = true;
            }
        }
        else
        {
            chaosModified = false;
        }
    }
    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        if (enabled)
            modifiers.FinalDamage *= incomingDamageMult;
    }
    public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        if (enabled)
            modifiers.FinalDamage *= incomingDamageMult;
    }
}