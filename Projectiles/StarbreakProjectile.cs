﻿namespace wdfeerCrazyMod.Projectiles;

internal class StarbreakProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.DamageType = DamageClass.Summon;
        Projectile.friendly = true;
        Projectile.timeLeft = 60;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 3;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
    }
    public override void AI()
    {
        Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Projectile.timeLeft = 1;
    }
    public override void Kill(int timeLeft)
    {
        for (int i = 0; i < 25; i++)
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(60, 60), DustID.Firework_Blue);
            d.noGravity = true;
        }
    }
}
