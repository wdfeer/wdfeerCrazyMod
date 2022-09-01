namespace wdfeerCrazyMod.Projectiles;

internal class CopperShortswordMouseControlled : ModProjectile
{
    public float speed = 8f;
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CopperShortswordStab;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("CopperShortsword");
    }
    public override void SetDefaults()
    {
        Projectile.DamageType = DamageClass.Summon;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 15;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 6;
    }
    public override void AI()
    {
        Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(45);
        if (Projectile.timeLeft < 15)
        {
            Projectile.alpha = 255 - (255 * Projectile.timeLeft / 15);
        }

        Player owner = Main.player[Projectile.owner];
        if (owner.active && !owner.dead && owner.HeldItem.type == ModContent.ItemType<Weapons.CopperShortswordMouseControlled>())
        {
            Projectile.timeLeft = 15;
            if (Main.myPlayer == owner.whoAmI)
            {
                Vector2 diff = (Main.MouseWorld - Projectile.Center);
                if (diff.Length() > 45)
                {
                    Projectile.velocity = speed * diff.SafeNormalize(Vector2.Zero);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = Mod.GetPacket();
                        packet.Write((byte)MessageType.MouseControlledCopperShortsword);
                        packet.Write(Projectile.whoAmI);
                        packet.Write(Projectile.velocity.X);
                        packet.Write(Projectile.velocity.Y);
                        packet.Send();
                    }
                }
            }
        }
    }
}
