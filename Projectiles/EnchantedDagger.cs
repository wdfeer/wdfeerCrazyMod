using System.Linq;

namespace wdfeerCrazyMod.Projectiles;

internal class EnchantedDagger : ModProjectile
{
    public override string Texture => "wdfeerCrazyMod/Weapons/EnchantedDagger";
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Enchanted Dagger");
    }
    int baseTimeLeft = 0;
    public override void SetDefaults()
    {
        baseTimeLeft = 100;
        Projectile.CloneDefaults(ProjectileID.MagicDagger);
        Projectile.timeLeft = baseTimeLeft;
        Projectile.penetrate = 4;
    }
    public override void AI()
    {
        Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
        if (Projectile.timeLeft == baseTimeLeft - 15)
        {
            Vector2 target = FindTarget();
            if (target != Vector2.Zero)
                Projectile.velocity = Projectile.velocity.Length() * (target - Projectile.Center).SafeNormalize(Vector2.Zero);
        }
        if (Projectile.timeLeft < 20)
        {
            Projectile.alpha = 255 - (255 * Projectile.timeLeft / 20);
        }
    }
    private Vector2 FindTarget()
    {
        var potentialTarget = Main.npc
            .Where(npc => npc.active && !npc.friendly && npc.CanBeChasedBy())
            .MinBy(npc => (npc.Center - Projectile.Center).Length());
        if (potentialTarget != null && potentialTarget.Center.Distance(Projectile.Center) < 600)
            return potentialTarget.Center;
        return Vector2.Zero;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Projectile.timeLeft = baseTimeLeft;
    }
}
