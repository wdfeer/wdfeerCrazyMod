﻿namespace wdfeerCrazyMod.Projectiles;

internal class CopperShortswordTomeSword : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CopperShortswordStab;
    public override void SetDefaults()
    {
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 75;
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
