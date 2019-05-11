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
        //TODO: example commands demonstrating subcommands, User parse rule, Item parse rule, Group parse rule, NPC parse rule, Region parse rule, 

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
        
        [Command("easy-tp")]
        [CommandPermissions("tshock.tp.self")]
        [HelpText("Teleport yourself or some other player to another player.")]
        public void TP(TSPlayer player, TSPlayer player2 = null)
        {
            TSPlayer teleporting, target;
            if(player2 == null)
            {
                teleporting = Sender;
                target = player;
            }
            else
            {
                teleporting = player;
                target = player2;

                if(Sender.HasPermission("tshock.tp.others"))
                {
                    Fail("You do not have access to this command.");
                }
                if(!teleporting.TPAllow && !Sender.HasPermission(Permissions.tpoverride))
                {
                    Fail($"{teleporting} has disabled players from teleporting.");
                }
            }

            if(!target.TPAllow && !Sender.HasPermission(Permissions.tpoverride))
            {
                Fail($"{target} has disabled players from teleporting.");
            }

            teleporting.Teleport(target.TPlayer.position.X, target.TPlayer.position.Y);
        }
    }
}
