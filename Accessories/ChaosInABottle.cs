using Terraria.DataStructures;

namespace wdfeerCrazyMod.Accessories;

internal class ChaosInABottle : ModItem
{
    public const float CHAOS_STATE_DURATION_MULT = 0.75f;
    public const float INCOMING_DAMAGE_MULT = 1.25f;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Chaos in a Bottle");
        Tooltip.SetDefault($"-{(int)(100 - CHAOS_STATE_DURATION_MULT * 100f)}% Chaos State duration\n+{(int)(INCOMING_DAMAGE_MULT * 100f - 100)}% damage taken");
    }
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
        player.GetModPlayer<ChaosInABottlePlayer>().enabled = true;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.ChaosElementalBanner);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }
}
class ChaosInABottlePlayer : ModPlayer
{
    public bool enabled;
    public override void ResetEffects()
        => enabled = false;
    bool chaosModified = false;
    public override void PostUpdateBuffs()
    {
        if (Player.HasBuff(BuffID.ChaosState)){
            if (!chaosModified)
            {
                ref int buffTime = ref Player.buffTime[Player.FindBuffIndex(BuffID.ChaosState)];
                buffTime = (int)(buffTime * ChaosInABottle.CHAOS_STATE_DURATION_MULT);
                chaosModified = true;
            }
        } else
        {
            chaosModified = false;
        }
    }
    public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
    {
        if (enabled)
            damage = (int)(damage * ChaosInABottle.INCOMING_DAMAGE_MULT);
        return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
    }
}