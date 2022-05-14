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
    internal class CopperShortswordMouseControlled : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CopperShortswordStab;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CopperShortsword");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 15;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(45);
            if (Projectile.timeLeft < 15)
            {
                Projectile.alpha = 255 - (255 * Projectile.timeLeft / 15);
            }
        }
    }
}
