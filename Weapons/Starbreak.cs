using System.Linq;

namespace wdfeerCrazyMod.Weapons;

public class Starbreak : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Starbreak");
        Tooltip.SetDefault("Holding this item materialises an interstellar spear, attacking your foes while they are nearby");
    }
    public override void SetDefaults()
    {
        Item.damage = 101;
        Item.DamageType = DamageClass.Summon;
        Item.knockBack = 8;
        Item.value = Item.buyPrice(gold: 22);
        Item.rare = 9;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.FragmentStardust, 12);
        recipe.AddTile(TileID.LunarCraftingStation);
        recipe.Register();
    }
    Projectile projectile;
    public override void HoldItem(Player player)
    {
        if (Main.myPlayer != player.whoAmI)
            return;
        NPC[] nearbyEnemies = Main.npc.Where(npc => npc is { active: true, friendly: false } && npc.CanBeChasedBy() && (player.Center - npc.Center).Length() < 900).ToArray();
        if (nearbyEnemies.Length == 0)
            return;
        if (projectile == null || !projectile.active || projectile.type != ModContent.ProjectileType<Projectiles.StarbreakProjectile>() || projectile.owner != player.whoAmI)
        {
            NPC target = nearbyEnemies.MinBy(npc => (player.Center - npc.Center).Length());
            Vector2 spawnPosition = target.Center + Main.rand.NextVector2CircularEdge(target.width + 240, target.height + 240);
            Vector2 velocity = Vector2.Normalize(target.Center - spawnPosition) * 16;

            int projectileID = Projectile.NewProjectile(Item.GetSource_ItemUse(Item),
                                      spawnPosition,
                                      velocity,
                                      ModContent.ProjectileType<Projectiles.StarbreakProjectile>(),
                                      player.GetWeaponDamage(Item),
                                      Item.knockBack,
                                          player.whoAmI);
            projectile = Main.projectile[projectileID];
            projectile.CritChance = player.GetWeaponCrit(Item);
            projectile.netUpdate = true;
        }
    }
}