using Terraria.DataStructures;

namespace wdfeerCrazyMod.Accessories;

internal class DangerlessAmulet : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("+50% Projectile damage as a separate multiplier\nWhenever you deal damage, spawn homing hostile projectiles");
    }
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.value = Terraria.Item.buyPrice(gold: 7);
        Item.rare = 5;
        Item.accessory = true;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<DangerlessPlayer>().enabled = true;
    }
}
class DangerlessPlayer : ModPlayer
{
    public bool enabled;
    public override void ResetEffects()
        => enabled = false;
    public const byte SPAWN_COOLDOWN = 2;
    byte spawnTimer = SPAWN_COOLDOWN;
    public bool ReadyToSpawn => spawnTimer >= SPAWN_COOLDOWN;
    public override void PostUpdate()
    {
        if (enabled && spawnTimer < SPAWN_COOLDOWN)
            spawnTimer++;
    }
    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (!enabled)
            return;
        if (!target.CanBeChasedBy() && target.type != NPCID.TargetDummy)
            return;
        if (!ReadyToSpawn)
            return;
        modifiers.FinalDamage *= 1.5f;
        Projectile hostile = Projectile.NewProjectileDirect(Terraria.Entity.InheritSource(proj), target.Center, proj.velocity.RotatedByRandom(0.314f), ModContent.ProjectileType<DangerlessProjectile>(), damage / 4 + 10, knockback / 3, Player.whoAmI);
        spawnTimer = 0;
    }
    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
    {
        if (damageSource.SourceProjectileType == ModContent.ProjectileType<DangerlessProjectile>())
            damageSource = PlayerDeathReason.ByCustomReason($"{Player.name} was too dangerless");
        return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
    }
}
class DangerlessProjectile : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.RubyBolt;
    public override void SetDefaults()
    {
        Projectile.hostile = true;
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.tileCollide = false;
        Projectile.light = 0.75f;
        Projectile.timeLeft = 160;
    }
    public Player target => Main.player[Projectile.owner];
    public override void AI()
    {
        if (Projectile.timeLeft <= 16)
        {
            Projectile.alpha = Projectile.timeLeft * 16;
            Projectile.light = Projectile.timeLeft * 0.75f / 16;
        }
        if ((Projectile.timeLeft > 45 && Projectile.timeLeft < 90) || target.dead || !target.active)
            return;
        float distance = target.Center.Distance(Projectile.Center);
        float homingMult = distance > 320f ? 1f : distance / 320f;
        Projectile.velocity += Vector2.Normalize(target.Center - Projectile.Center) * homingMult;
        if (Projectile.velocity.Length() > 16)
            Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 16;
    }
}