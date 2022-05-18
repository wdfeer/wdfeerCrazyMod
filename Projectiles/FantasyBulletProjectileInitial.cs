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
    internal class FantasyBulletProjectileInitial : ModProjectile
    {
        public override string Texture => "wdfeerCrazyMod/Projectiles/FantasyBulletProjectile";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fantasy Bullet");
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 1;
            Projectile.scale = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }
        int numOfProjectilesSpawnedOnHit = 6;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Vector2 launchVelocity = new Vector2(-6, 0); // Create a velocity moving the left.
            launchVelocity = launchVelocity.RotatedByRandom(MathHelper.Pi);
            for (int i = 0; i < numOfProjectilesSpawnedOnHit; i++)
            {
                launchVelocity = launchVelocity.RotatedBy(MathHelper.Pi * 2 / numOfProjectilesSpawnedOnHit);

                int projectileID = Projectile.NewProjectile(Projectile.InheritSource(Projectile),
                                                            Projectile.Center,
                                                            launchVelocity,
                                                            ModContent.ProjectileType<FantasyBulletProjectileSecond>(),
                                                            Projectile.damage / 2,
                                                            Projectile.knockBack / 4,
                                                            Projectile.owner);
                FantasyBulletProjectileSecond modProj = Main.projectile[projectileID].ModProjectile as FantasyBulletProjectileSecond;
                modProj.target = target;
            }
        }
    }
}
