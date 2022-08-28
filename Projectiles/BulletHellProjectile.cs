using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
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
    public Action<Projectile> ai = (Projectile projectile) => { };
    public override void AI()
    {
        Vector2 ownerPosDiff = Owner.position - Owner.oldPosition;
        Projectile.position += ownerPosDiff;
        ai(Projectile);
    }
    public override bool? CanHitNPC(NPC target)
    {
        if (Projectile.timeLeft > 112)
            return false;
        return base.CanHitNPC(target);
    }
}
