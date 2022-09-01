﻿namespace wdfeerCrazyMod.Projectiles;

internal class EnchantedBoomerang : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.EnchantedBoomerang;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Enchanted Boomerang");
        // This is necessary for right-click targeting
        ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

        Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
        ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
    }
    float passiveRotationMultiplier = 1;
    public sealed override void SetDefaults()
    {
        Projectile.width = 18;
        Projectile.height = 28;
        Projectile.tileCollide = false; // Makes the minion go through tiles freely

        // These below are needed for a minion weapon
        Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
        Projectile.minion = true; // Declares this as a minion (has many effects)
        Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
        Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
        Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles

        passiveRotationMultiplier = Main.rand.NextBool() ? 1 : -1;
    }

    // Here you can decide if your minion breaks things like grass or pots
    public override bool? CanCutTiles()
    {
        return false;
    }

    // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
    public override bool MinionContactDamage()
    {
        return true;
    }

    // The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
    public override void AI()
    {
        Player owner = Main.player[Projectile.owner];

        if (!CheckActive(owner))
        {
            return;
        }

        GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
        SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
        Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
        Visuals();
    }

    // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
    private bool CheckActive(Player owner)
    {
        if (owner.dead || !owner.active)
        {
            owner.ClearBuff(ModContent.BuffType<Buffs.EnchantedBoomerangBuff>());

            return false;
        }

        if (owner.HasBuff(ModContent.BuffType<Buffs.EnchantedBoomerangBuff>()))
        {
            Projectile.timeLeft = 2;
        }

        return true;
    }

    private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
    {
        Vector2 idlePosition = owner.Center;
        idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

        // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
        // The index is projectile.minionPos
        float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
        idlePosition.X += minionPositionOffsetX; // Go behind the player

        // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

        // Teleport to player if distance is too big
        vectorToIdlePosition = idlePosition - Projectile.Center;
        distanceToIdlePosition = vectorToIdlePosition.Length();

        if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
        {
            // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
            // and then set netUpdate to true
            Projectile.position = idlePosition;
            Projectile.velocity *= 0.1f;
            Projectile.netUpdate = true;
        }

        // If your minion is flying, you want to do this independently of any conditions
        float overlapVelocity = 0.04f;

        // Fix overlap with other minions
        for (int i = 0; i < Main.maxProjectiles; i++)
        {
            Projectile other = Main.projectile[i];

            if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
            {
                if (Projectile.position.X < other.position.X)
                {
                    Projectile.velocity.X -= overlapVelocity;
                }
                else
                {
                    Projectile.velocity.X += overlapVelocity;
                }

                if (Projectile.position.Y < other.position.Y)
                {
                    Projectile.velocity.Y -= overlapVelocity;
                }
                else
                {
                    Projectile.velocity.Y += overlapVelocity;
                }
            }
        }
    }

    private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
    {
        // Starting search distance
        distanceFromTarget = 700f;
        targetCenter = Projectile.position;
        foundTarget = false;

        // This code is required if your minion weapon has the targeting feature
        if (owner.HasMinionAttackTargetNPC)
        {
            NPC npc = Main.npc[owner.MinionAttackTargetNPC];
            float between = Vector2.Distance(npc.Center, Projectile.Center);

            // Reasonable distance away so it doesn't target across multiple screens
            if (between < 2000f)
            {
                distanceFromTarget = between;
                targetCenter = npc.Center;
                foundTarget = true;
            }
        }

        if (!foundTarget)
        {
            // This code is required either way, used for finding a target
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy())
                {
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                    // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                    // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                    bool closeThroughWall = between < 100f;

                    if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }
        }

        Projectile.friendly = foundTarget;
    }

    private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
    {
        float speed = 9f;
        float inertia = 20f;

        if (foundTarget)
        {
            if (distanceFromTarget > 40)
            {
                Vector2 direction = targetCenter - Projectile.Center;
                direction.Normalize();
                if (distanceFromTarget > 100)
                {
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction * speed) / inertia;
                }
                else
                {
                    speed = 32f;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction * speed) / inertia;
                }
            }
        }
        else
        {
            // Minion doesn't have a target: return to player and idle
            if (distanceToIdlePosition > 600f)
            {
                // Speed up the minion if it's away from the player
                speed = 12f;
                inertia = 60f;
            }
            else
            {
                // Slow down the minion if closer to the player
                speed = 4f;
                inertia = 80f;
            }

            if (distanceToIdlePosition > 20f)
            {
                // The immediate range around the player (when it passively floats about)

                // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            else if (Projectile.velocity == Vector2.Zero)
            {
                // If there is a case where it's not moving at all, give it a little "poke"
                Projectile.velocity.X = -0.15f;
                Projectile.velocity.Y = -0.05f;
            }
        }
    }
    float rotationDelta = 0f;
    private void Visuals()
    {
        // So it will lean slightly towards the direction it's moving
        Projectile.rotation = (float)(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.Pi) + rotationDelta * passiveRotationMultiplier;
        float velocityLength = Projectile.velocity.Length();
        if (velocityLength < 2f)
            velocityLength = 2f;
        rotationDelta += MathHelper.ToRadians(36f / velocityLength);
        if (rotationDelta > MathHelper.TwoPi)
            rotationDelta = rotationDelta - MathHelper.TwoPi;
        // Some visuals here
        Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 0.5f, 0.7f) * 0.78f);
    }
}
