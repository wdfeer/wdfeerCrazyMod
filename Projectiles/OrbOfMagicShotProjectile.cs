﻿namespace wdfeerCrazyMod.Projectiles;

internal class OrbOfMagicShotProjectile : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.AmethystBolt;
    int dustType;
    Func<float, float> trigonometry;
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.AmethystBolt);
        Projectile.timeLeft = 180;
        Projectile.penetrate = 3;

        int[] possibleDustTypes = { DustID.Clentaminator_Purple, DustID.Clentaminator_Cyan, DustID.Clentaminator_Blue };
        dustType = possibleDustTypes[Main.rand.Next(0, possibleDustTypes.Length)];
        trigonometry = Main.rand.NextBool() ? MathF.Sin : MathF.Cos;
    }
    public override void AI()
    {
        Vector2 delta = Projectile.velocity.RotatedBy(MathHelper.PiOver2) * trigonometry(Projectile.timeLeft / MathF.PI);
        Projectile.position += delta / 2;

        Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height), dustType);
        d.noGravity = true;
        d.scale = 1.2f;
        d.velocity /= 3;
    }
    public override void Kill(int timeLeft)
    {
        for (int i = 0; i < 10; i++)
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height), dustType);
            d.noGravity = true;
        }
    }
}
