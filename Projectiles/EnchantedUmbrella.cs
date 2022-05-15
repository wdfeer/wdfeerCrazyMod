using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace wdfeerCrazyMod.Projectiles
{
    internal class EnchantedUmbrella : ModProjectile
    {
        public override string Texture => "wdfeerCrazyMod/Items/EnchantedUmbrella";
		public bool launched = false;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Umbrella");
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 28;
			Projectile.tileCollide = false; // Makes the minion go through tiles freely

			// These below are needed for a minion weapon
			Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
			Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
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
			if (launched)
				return;
			Player owner = Main.player[Projectile.owner];

			if (!CheckActive(owner))
			{
				return;
			}

			SearchForTargets(owner, out bool foundTarget, out Vector2 targetCenter);
			Movement(foundTarget, targetCenter, owner.position - new Vector2(owner.width / 2, 0));
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

		private void SearchForTargets(Player owner, out bool foundTarget, out Vector2 targetCenter)
		{
			// Starting search distance
			float distanceFromTarget = 700f;
			targetCenter = Projectile.position;
			foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC)
			{
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);

				// Reasonable distance away so it doesn't target across multiple screens
				if (between > 90f && between < 900f)
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
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						if (between < 120f)
							continue;
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
						// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
						bool closeThroughWall = between < 300f;

						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
						{
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}

			Projectile.friendly = foundTarget;
		}
		Vector2 vectorFromOwner = new Vector2(0, 60);
		private void Movement(bool foundTarget, Vector2 targetCenter, Vector2 ownerCenter)
		{
			if (!foundTarget)
            {
				RotateAroundOwner(ownerCenter, 2);
            }
            else
            {
				Vector2 diff = targetCenter - ownerCenter;
				diff.Normalize();
				float LengthOfDifferenceBetweenOwnerToTargetAndOwnerToProjectile = (diff - vectorFromOwner.SafeNormalize(Vector2.Zero)).Length();
				if (LengthOfDifferenceBetweenOwnerToTargetAndOwnerToProjectile > 0.02f)
                {
					RotateAroundOwner(ownerCenter, 2f * LengthOfDifferenceBetweenOwnerToTargetAndOwnerToProjectile);
                }
                else
                {
					Projectile.velocity = vectorFromOwner.SafeNormalize(Vector2.Zero) * 20;
					Projectile.extraUpdates = 1;
					Projectile.timeLeft = 60;
					Projectile.usesLocalNPCImmunity = true;
					Projectile.localNPCHitCooldown = -1;
					launched = true;
					Projectile.netUpdate = true;
                }
            }
		}
		private void RotateAroundOwner(Vector2 ownerCenter, float degrees)
        {
			Projectile.position = ownerCenter + vectorFromOwner;
			vectorFromOwner = vectorFromOwner.RotatedBy(MathHelper.ToRadians(degrees));
			Projectile.rotation = (float)Math.Atan2(vectorFromOwner.Y, vectorFromOwner.X) + MathHelper.PiOver2;
		}
		private void Visuals()
		{
			// Some visuals here
			Lighting.AddLight(Projectile.Center, new Vector3(0.9f,0.9f,1.4f));
		}
	}
}
