namespace wdfeerCrazyMod.Projectiles;

internal class TrueTinShortswordProjectile : TrueCopperShortswordProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.TinShortswordStab;
    public override void SetDefaults()
    {
        baseTimeLeft = 9;
        base.SetDefaults();
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
    }
}
