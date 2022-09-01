using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria.Audio;
using Terraria.GameContent;
using wdfeerCrazyMod.Weapons;

namespace wdfeerCrazyMod.Projectiles;

internal class ScorcherProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Scorcher");
        Main.projFrames[Projectile.type] = 3;
    }
    public override void SetDefaults()
    {
        Projectile.timeLeft = Scorcher.DURATION;
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.DamageType = DamageClass.Summon;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.tileCollide = true;
    }
    public static float FireInterval => 15;
    public float FireTimer
    {
        get => Projectile.ai[0];
        set { Projectile.ai[0] = value; }
    }
    public float Charge => FireTimer > FireInterval ? 1f : FireTimer / FireInterval;
    int shotsFired = 0;
    int ShotsFired
    {
        get => shotsFired; set
        {
            if (value > 12)
                value = 12;
            shotsFired = value;
        }
    }
    public override void AI()
    {
        if (Projectile.velocity.Length() > 1)
            Projectile.rotation = Projectile.velocity.ToRotation();
        Projectile.velocity *= 0.9f;
        Projectile.light = Charge;

        FireTimer++;

        if (Charge < 0.33f)
            Projectile.frame = 0;
        else if (Charge < 0.67f)
            Projectile.frame = 1;
        else Projectile.frame = 2;

        if (FireTimer > FireInterval)
        {
            Vector2 target = GetTarget();
            if (target != Vector2.Zero)
            {
                Projectile.rotation = Projectile.Center.AngleTo(target);
                Fire(target);
                ShotsFired++;

                FireTimer = 0;
            }
        }
    }
    private void Fire(Vector2 target)
    {
        Vector2 launchVelocity = (target - Projectile.Center).SafeNormalize(Vector2.Zero) * 16;
        Projectile bullet = Projectile.NewProjectileDirect(Terraria.Entity.InheritSource(Projectile),
                                                        Projectile.Center,
                                                        launchVelocity,
                                                        ModContent.ProjectileType<ScorcherBullet>(),
                                                        (int)MathF.Pow(Projectile.damage, ShotsFired + 1),
                                                        Projectile.knockBack,
                                                        Projectile.owner);
        SoundEngine.PlaySound(SoundID.Item40, Projectile.Center);
    }
    private Vector2 GetTarget()
    {
        var potentialTarget = Main.npc
            .Where(npc => npc.active && !npc.friendly && npc.CanBeChasedBy() && LineOfSight(npc.position, npc.width, npc.height))
            .MinBy(npc => (npc.Center - Projectile.Center).Length());
        if (potentialTarget != null && potentialTarget.Center.Distance(Projectile.Center) < 1600)
            return potentialTarget.Center;
        return Vector2.Zero;
    }
    private bool LineOfSight(Vector2 position, int width, int height)
        => Collision.CanHitLine(Projectile.Center, 0, 0, position, width, height);
    public override void Kill(int timeLeft)
    {
        for (int i = 0; i < 15; i++)
        {
            Dust.NewDustPerfect(
                Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.width),
                DustID.Smoke,
                Scale: 0.6f);
        }
    }
}
class ScorcherBullet : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ExplosiveBullet;
    public override void SetDefaults()
    {
        Projectile.width = 8;
        Projectile.height = 8;
        Projectile.aiStyle = ProjAIStyleID.Arrow;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.DamageType = DamageClass.Summon;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 240;
        Projectile.light = 0.2f;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.extraUpdates = 3;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;

        AIType = ProjectileID.ExplosiveBullet;
    }
    public override void PostAI()
    {
        if (Projectile.timeLeft > 238)
            Projectile.alpha = 32;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(Projectile.type);
        Texture2D texture = TextureAssets.Projectile[ProjectileID.ExplosiveBullet].Value;

        // Redraw the projectile with the color not influenced by light
        Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
        for (int k = 0; k < Projectile.oldPos.Length; k++)
        {
            Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
            Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
        }

        return true;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Explode();
        return false;
    }
    public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
    {
        target.AddBuff(BuffID.OnFire, 600);
        Explode();
    }
    public override void OnHitPlayer(Player target, int damage, bool crit)
    {
        target.AddBuff(BuffID.OnFire, 360);
        Explode();
    }
    void Explode()
    {
        Projectile.Resize(40, 40);
        Projectile.timeLeft = 2;
        Projectile.tileCollide = false;
    }
    public override void Kill(int timeLeft)
    {
        for (int i = 0; i < 18; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);

        }
        for (int i = 0; i < 7; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
        }
        SoundEngine.PlaySound(SoundID.Item14.WithVolumeScale(0.6f), Projectile.position);
    }
}