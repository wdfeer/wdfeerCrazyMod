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

namespace wdfeerCrazyMod.Projectiles
{
    internal class PossessedUziProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Uzi;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Uzi");
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 36;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely

			// These below are needed for a minion weapon
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
			return false;
		}

		// The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];

			if (!CheckActive(owner))
			{
				return;
			}

			GeneralBehavior(owner);
			SearchForTargets(owner, out bool foundTarget, out Vector2 targetCenter);
			Attack(foundTarget, targetCenter);
			Visuals();
		}

		// This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<Buffs.PossessedUziBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<Buffs.PossessedUziBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

		private void GeneralBehavior(Player owner)
		{
			int[] allUzis = Main.projectile
								.Where(proj => proj.active && proj.type == Projectile.type && proj.owner == Projectile.owner)
								.Select(proj => proj.whoAmI)
								.ToArray();

			Vector2 OwnerToIdlePosition = new Vector2(0, -48 * (float)Math.Sqrt(allUzis.Length));
			
			int myUziIndex = Array.IndexOf(allUzis, Projectile.whoAmI);
			OwnerToIdlePosition = OwnerToIdlePosition.RotatedBy(MathHelper.ToRadians(360 / allUzis.Length * myUziIndex));

			Projectile.position = owner.VisualPosition + OwnerToIdlePosition;
		}
		private void SearchForTargets(Player owner, out bool foundTarget, out Vector2 targetCenter)
		{
            float distanceToTarget;
            GetPlayersTarget(owner, out foundTarget, out distanceToTarget, out targetCenter);
			if (foundTarget)
				return;
			else
			{
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (!npc.CanBeChasedBy())
                        continue;
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceToTarget;
                    bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                    // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                    // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                    if (((closest && inRange) || !foundTarget) && lineOfSight)
                    {
                        distanceToTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

			Projectile.friendly = foundTarget;
		}
		private void GetPlayersTarget(Player owner, out bool foundTarget, out float distanceToTarget, out Vector2 targetCenter)
		{
			// Starting search distance
			distanceToTarget = 700f;
			targetCenter = Projectile.position;
			foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);
				bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
				if (!lineOfSight)
					return;
				if (between < 1600f)
				{
					distanceToTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}
		}
		int attackCooldown = 9;
		int attackTimer = 0;
		private void Attack(bool foundTarget, Vector2 targetCenter)
		{
			if (!foundTarget)
				return;
			attackTimer++;
			if (attackTimer < attackCooldown)
				return;
			Vector2 toTarget = targetCenter - Projectile.Center;
			{
				Vector2 launchVelocity = Vector2.Normalize(toTarget) * 16;
				Projectile proj = Projectile.NewProjectileDirect(Entity.InheritSource(Projectile), Projectile.Center, launchVelocity, ProjectileID.BulletHighVelocity, Projectile.damage, Projectile.knockBack, Projectile.owner);
				proj.CritChance = Projectile.CritChance; 
			}

			attackTimer = 0;

			SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);

			Projectile.rotation = (float)Math.Atan2(toTarget.Y, toTarget.X);
		}
		private void Visuals()
		{
			return;
		}
	}
}
