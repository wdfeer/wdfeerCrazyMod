global using Microsoft.Xna.Framework;
global using Terraria;
global using Terraria.ID;
global using Terraria.ModLoader;
global using System;
using System.IO;

namespace wdfeerCrazyMod;

public class wdfeerCrazyMod : Mod
{
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        MessageType messageType = (MessageType)reader.ReadByte();
        switch (messageType)
        {
            case MessageType.MouseControlledCopperShortsword:
                int projectileID = reader.ReadInt32();
                float velocityX = reader.ReadSingle();
                float velocityY = reader.ReadSingle();
                Projectile projectile = Main.projectile[projectileID];
                projectile.velocity = new Vector2(velocityX, velocityY);

                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = GetPacket();
                    packet.Write((byte)messageType);
                    packet.Write(projectileID);
                    packet.Write(velocityX);
                    packet.Write(velocityY);
                    packet.Send(ignoreClient: whoAmI);
                }
                break;
            case MessageType.ProjectileRotation:
                projectileID = reader.ReadInt32();
                float rotation = reader.ReadSingle();
                if (Main.netMode == NetmodeID.Server)
                {
                    SyncProjectileRotation(projectileID, rotation, whoAmI);
                }
                else
                {
                    Main.projectile[projectileID].rotation = rotation;
                }
                break;
            default:
                break;
        }
    }
    public void SyncProjectileRotation(Projectile proj, float rotation)
        => SyncProjectileRotation(proj.whoAmI, rotation);
    public void SyncProjectileRotation(int proj, float rotation, int ignoreClient = -1)
    {
        ModPacket packet = GetPacket();
        packet.Write((byte)MessageType.ProjectileRotation);
        packet.Write(proj);
        packet.Write(rotation);
        packet.Send(ignoreClient: ignoreClient);
    }
}
public enum MessageType
{
    MouseControlledCopperShortsword,
    ProjectileRotation
}