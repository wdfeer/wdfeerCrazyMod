﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using wdfeerCrazyMod.Projectiles;

namespace wdfeerCrazyMod.Weapons;
internal class BulletHell : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Spawns an array of projectiles around you\nHas three types of attacks");
    }
    public override void SetDefaults()
    {
        Item.damage = 160;
        Item.crit = 16;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 16;
        Item.knockBack = 0f;
        Item.useTime = 5;
        Item.useAnimation = 60;
        Item.useLimitPerAnimation = 4;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.value = Item.sellPrice(gold: 25);
        Item.rare = ItemRarityID.Yellow;
        Item.shoot = ModContent.ProjectileType<BulletHellProjectile>();
        Item.shootSpeed = 14;
        Item.autoReuse = true;
    }
    int castTimer = 0;
    public override void UpdateInventory(Player player)
        => castTimer++;
    float useNum = 0;
    int attackNum = 0;
    public override bool? UseItem(Player player)
    {
        if (castTimer >= Item.useAnimation)
        {
            useNum = 0;
            castTimer = 0;
            SoundEngine.PlaySound(SoundID.Item43, player.Center);
            UpdateCurrentAttack();
            attackNum++;
        }
        return base.UseItem(player);
    }
    void UpdateCurrentAttack()
    {
        switch (attackNum)
        {
            case 0:
                currentAttack = (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) =>
                {
                    int numOfProjectiles = 15;
                    float angleStep = MathF.PI * 2 / numOfProjectiles;
                    velocity = velocity.RotatedBy(-useNum * (angleStep / (float)Item.useLimitPerAnimation));
                    for (int i = 0; i < numOfProjectiles; i++)
                    {
                        var projectile = Projectile.NewProjectileDirect(
                            source,
                            position,
                            velocity.RotatedBy(i * angleStep),
                            type,
                            damage,
                            knockback,
                            player.whoAmI);
                        projectile.penetrate = 2;
                        (projectile.ModProjectile as BulletHellProjectile).ai = (Projectile projectile) =>
                        {
                            if (projectile.timeLeft == 92)
                            {
                                projectile.velocity /= 4;
                            }
                        };
                    }
                };
                return;
            case 1:
                currentAttack = (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) =>
                {
                    int numOfProjectiles = 24;
                    float angleStep = MathF.PI * 2 / numOfProjectiles;
                    velocity = velocity.RotatedBy(useNum * (angleStep / (float)Item.useLimitPerAnimation)) / 2;
                    for (int i = 0; i < numOfProjectiles; i++)
                    {
                        var projectile = Projectile.NewProjectileDirect(
                            source,
                            position,
                            velocity.RotatedBy(i * angleStep),
                            type,
                            damage,
                            knockback,
                            player.whoAmI);
                        (projectile.ModProjectile as BulletHellProjectile).ai = (Projectile projectile) =>
                        {
                            projectile.velocity = projectile.velocity.RotatedBy(0.01f);
                        };
                    }
                };
                return;
            case 2:
                currentAttack = (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) =>
                {
                    int numOfProjectiles = 16;
                    float angleStep = MathF.PI * 2 / numOfProjectiles;
                    for (int i = 0; i < numOfProjectiles; i++)
                    {
                        var projectile = Projectile.NewProjectileDirect(
                            source,
                            position,
                            velocity.RotatedBy(i * angleStep),
                            type,
                            damage,
                            knockback,
                            player.whoAmI);
                        (projectile.ModProjectile as BulletHellProjectile).ai = (Projectile projectile) =>
                        {
                            if (projectile.timeLeft == 70)
                                projectile.velocity *= -0.65f;
                        };
                    }
                };
                break;
            default:
                attackNum = 0;
                UpdateCurrentAttack();
                break;
        }
    }
    Action<Player, EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float> currentAttack;
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        currentAttack(player, source, position, velocity, type, damage, knockback);
        useNum++;
        return false;
    }
}
