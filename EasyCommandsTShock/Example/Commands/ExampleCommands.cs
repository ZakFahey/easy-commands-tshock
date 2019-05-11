using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;
using Microsoft.Xna.Framework;
using Command = EasyCommands.Command;

namespace Example.Commands
{
    class ExampleCommands : CommandCallbacks<TSPlayer>
    {
        [Command("sendsillymessage")]
        [HelpText("Sends a very silly message to another player.")]
        public void SendSillyMessage(TSPlayer player, [AllowSpaces]string message)
        {
            string sillyMessage = "";
            for(int i = 0; i < message.Length; i++)
            {
                if(i % 2 == 0)
                {
                    sillyMessage += char.ToUpper(message[i]);
                }
                else
                {
                    sillyMessage += char.ToLower(message[i]);
                }
            }

            Sender.SendInfoMessage($"[To {player.Name}]: {sillyMessage}");
            player.SendInfoMessage($"[From {Sender.Name}]: {sillyMessage}");
        }

        [Command("broadcast-color", "bc-color")]
        [HelpText("Broadcast a message with a certain color.")]
        public void BroadcastWithColor(Color color, [AllowSpaces]string message)
        {
            TShock.Utils.Broadcast(message, color);
        }
    }
}
