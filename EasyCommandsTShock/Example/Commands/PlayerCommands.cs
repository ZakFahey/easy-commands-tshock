﻿using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;
using Command = EasyCommands.Command;

namespace Example.Commands
{
    class PlayerCommands : CommandCallbacks<TSPlayer>
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
    }
}