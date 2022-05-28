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
    internal class TrueTinShortswordProjectile : TrueCopperShortswordProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.TinShortswordStab;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tin Shortsword");
        }
        public override void SetDefaults()
        {
            baseTimeLeft = 9;
            base.SetDefaults();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
    }
}
