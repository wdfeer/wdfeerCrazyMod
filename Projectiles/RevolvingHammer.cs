using System.Linq;

namespace wdfeerCrazyMod.Projectiles;

internal class RevolvingHammer : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.PaladinsHammerFriendly;
    public sealed override void SetDefaults()
    {
        Projectile.width = 38;
        Projectile.height = 38;
        Projectile.tileCollide = false; // Makes the minion go through tiles freely
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 6;

        Main.projPet[Projectile.type] = true;
        Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
        Projectile.minion = true; // Declares this as a minion (has many effects)
        Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
        Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
        Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
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

        SearchForTargets(owner, out bool foundTarget, out Vector2 targetCenter);
        Movement(foundTarget, targetCenter, owner);
        Visuals();
    }
    // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
    private bool CheckActive(Player owner)
    {
        if (owner.dead || !owner.active)
        {
            owner.ClearBuff(ModContent.BuffType<Buffs.RevolvingHammerBuff>());

            return false;
        }

        if (owner.HasBuff(ModContent.BuffType<Buffs.RevolvingHammerBuff>()))
        {
            Projectile.timeLeft = 2;
        }

        return true;
    }

    private void SearchForTargets(Player owner, out bool foundTarget, out Vector2 targetCenter)
    {
        // Starting search distance
        float distanceFromTarget = 600f;
        targetCenter = Projectile.position;
        foundTarget = false;

        // This code is required if your minion weapon has the targeting feature
        if (owner.HasMinionAttackTargetNPC)
        {
            NPC npc = Main.npc[owner.MinionAttackTargetNPC];
            float between = Vector2.Distance(npc.Center, Projectile.Center);

            // Reasonable distance away so it doesn't target across multiple screens
            if (between > 80f && between < 600f)
            {
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
                    float betweenOwnerAndNPC = Vector2.Distance(npc.Center, owner.Center);
                    if (betweenOwnerAndNPC < 80f)
                        continue;
                    bool inRange = betweenOwnerAndNPC < distanceFromTarget;
                    if (inRange)
                    {
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }
        }

        Projectile.friendly = foundTarget;
    }
    float degreesOfRotation = 0;
    float distanceToOwnerCenter = 120;
    private void Movement(bool foundTarget, Vector2 targetCenter, Player owner)
    {
        Vector2 ownerCenter = owner.Center;
        if (!foundTarget)
        {
            degreesOfRotation++;
            Projectile.position = GetPosition(owner);
            Projectile.rotation = GetRotation(ownerCenter);
        }
        else
        {
            float distanceFromOwnerToTarget = ownerCenter.Distance(targetCenter);
            if (distanceFromOwnerToTarget > distanceToOwnerCenter)
                distanceToOwnerCenter += 5;
            else
                distanceToOwnerCenter -= 5;

            degreesOfRotation -= 7;
            Projectile.position = GetPosition(owner);
            Projectile.rotation = GetRotation(ownerCenter);
        }
    }
    private Vector2 GetPosition(Player owner)
    {
        Projectile[] hammers = GetAllHammers();
        RevolvingHammer hammerLeader = (hammers[0].ModProjectile as RevolvingHammer);
        int[] allHammerIDs = hammers.Select(ham => ham.whoAmI).ToArray();
        int myIndex = Array.IndexOf(allHammerIDs, Projectile.whoAmI);
        float rads = MathHelper.ToRadians(360 / allHammerIDs.Length * myIndex);
        if (myIndex != 0)
        {
            degreesOfRotation = hammerLeader.degreesOfRotation;
            distanceToOwnerCenter = hammerLeader.distanceToOwnerCenter;
        }
        Vector2 ownerToPosition = new Vector2(distanceToOwnerCenter, 0).RotatedBy(rads + MathHelper.ToRadians(degreesOfRotation));
        return owner.VisualPosition + ownerToPosition;
    }
    private float GetRotation(Vector2 ownerCenter)
    {
        Vector2 vectorFromOwner = Projectile.position - ownerCenter;
        return (float)Math.Atan2(vectorFromOwner.Y, vectorFromOwner.X) + MathHelper.PiOver4;
    }
    private Projectile[] GetAllHammers() => Main.projectile
                            .Where(proj => proj.active && proj.type == Projectile.type && proj.owner == Projectile.owner)
                            .ToArray();
    private void Visuals()
    {
        // Some visuals here
        Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.6f, 0.4f));
    }
}
