using System.Linq;
using Terraria.Audio;

namespace wdfeerCrazyMod.Projectiles;

internal class AutobowProjectile : ModProjectile
{
    public override string Texture => "wdfeerCrazyMod/Weapons/Autobow";
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Autobow");
    }
    int baseTimeLeft = 0;
    public override void SetDefaults()
    {
        baseTimeLeft = 480;
        Projectile.timeLeft = baseTimeLeft;
        Projectile.width = 18;
        Projectile.height = 36;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
    }
    public override void AI()
    {
        if (Projectile.velocity.Y < 16)
            Projectile.velocity.Y += 0.12f;
        if (Projectile.timeLeft % 10 == 0)
        {
            Vector2 target = GetTarget();
            if (target != Vector2.Zero)
            {
                Fire(target);

                SoundEngine.PlaySound(SoundID.Item5);

                Vector2 vectorToTarget = target - Projectile.Center;
                Projectile.rotation = (float)Math.Atan2(vectorToTarget.Y, vectorToTarget.X);
            }
        }
        if (Projectile.timeLeft < 20)
        {
            Projectile.alpha = 255 - (255 * Projectile.timeLeft / 20);
        }
    }
    private void Fire(Vector2 target)
    {
        Vector2 launchVelocity = (target - Projectile.Center).SafeNormalize(Vector2.Zero) * 16;
        Projectile arrow = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile),
                                                        Projectile.Center,
                                                        launchVelocity,
                                                        ProjectileID.WoodenArrowFriendly,
                                                        Projectile.damage / 2,
                                                        Projectile.knockBack / 2,
                                                        Projectile.owner);
        arrow.usesLocalNPCImmunity = true;
        arrow.localNPCHitCooldown = -1;
        arrow.CritChance = Projectile.CritChance;
    }
    private Vector2 GetTarget()
    {
        var potentialTarget = Main.npc
            .Where(npc => npc.active && !npc.friendly && npc.CanBeChasedBy() && LineOfSight(npc.position, npc.width, npc.height))
            .MinBy(npc => (npc.Center - Projectile.Center).Length());
        if (potentialTarget != null && potentialTarget.Center.Distance(Projectile.Center) < 900)
            return potentialTarget.Center;
        return Vector2.Zero;
    }
    private bool LineOfSight(Vector2 position, int width, int height)
        => Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, position, width, height);
    public override void Kill(int timeLeft)
    {
        for (int i = 0; i < 15; i++)
        {
            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.width),
                                DustID.Titanium);
        }
    }
}
