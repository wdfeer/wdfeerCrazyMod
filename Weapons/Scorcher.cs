using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace wdfeerCrazyMod.Weapons;

internal class Scorcher : ModItem
{
    public const int DURATION = 130;
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault($"Throws a turret which shoots explosive rounds at nearby enemies\nDamage increases exponentially with each shot\nEach turret lasts {DURATION / 60} seconds");

        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 2;
        Item.knockBack = 4f;
        Item.mana = 12; // mana cost
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 40;
        Item.useAnimation = 40;
        Item.useStyle = ItemUseStyleID.Swing; // how the player's arm moves when using the item
        Item.value = Terraria.Item.sellPrice(gold: 3);
        Item.rare = 2;
        Item.UseSound = SoundID.Item1; // What sound should play when using the item
        Item.autoReuse = true;

        Item.noMelee = true; // this item doesn't do any melee damage
        Item.noUseGraphic = true;
        Item.DamageType = DamageClass.Summon; // Makes the damage register as summon. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type
        Item.shoot = ModContent.ProjectileType<Projectiles.ScorcherProjectile>(); // This item creates the minion projectile
        Item.shootSpeed = 16f;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;

        return false;
    }
}
