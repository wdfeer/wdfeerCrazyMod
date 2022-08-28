using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            default:
                break;
        }
    }
}
public enum MessageType
{
    MouseControlledCopperShortsword
}