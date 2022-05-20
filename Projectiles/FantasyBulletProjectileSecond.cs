using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
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
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 2;
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
            if (Projectile.timeLeft > 220)
                return false;
            return base.CanHitNPC(target);
        }
        int ricochetTimesLeft = 5;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (ricochetTimesLeft <= 0)
                return true;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X * 1.1f;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 1.1f;
            }

            ricochetTimesLeft--;
            return false;
        }
    }
}
