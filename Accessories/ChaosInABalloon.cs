using Terraria.DataStructures;

namespace wdfeerCrazyMod.Accessories;

internal class ChaosInABalloon : ChaosInABottle
{
    protected override float ChaosStateDurationMult => 0.67f;
    protected override float IncomingDamageMult => 1.4f;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Chaos in a Balloon");
        Tooltip.SetDefault($"-{(int)(100 - ChaosStateDurationMult * 100f)}% Chaos State duration\nIncreases jump height\n+{(int)(IncomingDamageMult * 100f - 100)}% damage taken");
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.value += Item.sellPrice(gold: 1);
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);
        player.jumpBoost = true;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<ChaosInABottle>());
        recipe.AddIngredient(ItemID.ShinyRedBalloon);
        recipe.AddTile(TileID.TinkerersWorkbench);
        recipe.Register();
    }
}