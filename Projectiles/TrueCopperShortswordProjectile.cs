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
    internal class TrueCopperShortswordProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CopperShortswordStab;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Copper Shortsword");
        }
        protected int baseTimeLeft = 15;
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = baseTimeLeft;
            Projectile.penetrate = 1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 4;
        }
        public override void AI()
        {
            if (Projectile.velocity.Length() > 1)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.timeLeft < baseTimeLeft)
            {
                Projectile.alpha = 255 - (255 * Projectile.timeLeft / 15);
            }
        }
    }
}
