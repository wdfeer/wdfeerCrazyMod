using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Projectiles;

internal class BulletHellProjectile : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RubyBolt;
    public override void SetDefaults()
    {
        Projectile.timeLeft = 120;
        Projectile.tileCollide = false;
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.friendly = true;
        Projectile.light = 0.3f;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
    }
    public Player Owner => Main.player[Projectile.owner];
    public override void AI()
    {
        Vector2 ownerPosDiff = Owner.position - Owner.oldPosition;
        Projectile.position += ownerPosDiff;
        ExtraAI();
    }
    protected virtual void ExtraAI() { }
    public override bool? CanHitNPC(NPC target)
    {
        if (Projectile.timeLeft > 112)
            return false;
        return base.CanHitNPC(target);
    }
}
class BulletHellProjectile1 : BulletHellProjectile
{
    protected override void ExtraAI()
    {
        if (Projectile.timeLeft == 92)
        {
            Projectile.velocity /= 4;
        }
    }
}
class BulletHellProjectile2 : BulletHellProjectile
{
    protected override void ExtraAI()
    {
        Projectile.velocity = Projectile.velocity.RotatedBy(0.01f);
    }
}
class BulletHellProjectile3 : BulletHellProjectile
{
    protected override void ExtraAI()
    {
        if (Projectile.timeLeft == 70)
            Projectile.velocity *= -0.65f;
    }
}
