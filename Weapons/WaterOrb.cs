using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Weapons
{
    public class WaterOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Casts orbs of water on your enemies\nThey accelerate over time, increasing their damage but draining mana");
        }
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 1;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.scale = 0;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.knockBack = 2;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = 2;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shootSpeed = 12;
            Item.shoot = ModContent.ProjectileType<Projectiles.WaterOrb>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.MouseWorld;
            position = position + Main.rand.NextVector2CircularEdge(64, 64);
            Dust d = Dust.NewDustPerfect(position, DustID.CopperCoin);
            d.noGravity = true;

            velocity = (target - position).SafeNormalize(Vector2.Zero) * velocity.Length() / 100;

            int projectileID = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Projectile projectile = Main.projectile[projectileID];
            projectile.Center = position;
            projectile.CritChance = player.GetWeaponCrit(Item);
            return false;
        }
    }
}