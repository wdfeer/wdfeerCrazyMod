using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Projectiles;

internal class WaterOrb : ModProjectile
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Water Orb");
        Main.projFrames[Projectile.type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.DamageType = DamageClass.Magic;
        Projectile.width = 12;
        Projectile.height = 12;
        Projectile.scale = 2f;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 300;
    }
    Player owner => Main.player[Projectile.owner];
    int manaConsumption => (int)(owner.manaCost * 4);
    public override void AI()
    {
        if (Projectile.timeLeft < 270)
            Projectile.tileCollide = true;

        if (Projectile.velocity.Length() < 10)
        {
            Projectile.velocity *= 1.1f;
        }
        else if (owner.statMana > manaConsumption)
        {
            Projectile.extraUpdates++;
            Projectile.velocity *= 0.5f;
            owner.statMana -= manaConsumption;
        }

        if (Projectile.velocity.Length() > 2)
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.PiOver2;
            Projectile.frame = 1;
        }
        else
        {
            Projectile.frame = 0;
        }
    }
    public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
    {
        float powerModifier = Projectile.velocity.Length() * (Projectile.extraUpdates + 1) / 16;
        damage = (int)(damage * powerModifier);
        knockback = (int)(knockback * powerModifier);
    }
    public override void Kill(int timeLeft)
    {
        for (int i = 0; i < 12; i++)
        {
            Dust d = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height), 1, 1, DustID.Water);
        }
    }
}
