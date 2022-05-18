using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Projectiles
{
    internal class FantasyBulletProjectileSecond : ModProjectile
    {
        public NPC target;
        public override string Texture => "wdfeerCrazyMod/Projectiles/FantasyBulletProjectile";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fantasy Bullet");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<FantasyBulletProjectileInitial>());
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            if (!target.active)
                return;
            Projectile.velocity += (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) / 5;
            if (Projectile.velocity.Length() > 16)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 16;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft > 160)
                return false;
            return base.CanHitNPC(target);
        }
    }
}
