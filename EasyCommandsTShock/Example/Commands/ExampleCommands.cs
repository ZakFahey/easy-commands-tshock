using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using EasyCommands.Commands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;
using Microsoft.Xna.Framework;
using Command = EasyCommands.Command;
using System.Linq;
using TShockAPI.Localization;

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

        [Command("num-npcs", "num-mobs")]
        [HelpText("Gets the number of npcs for a certain type.")]
        public void GetNumNPCs([AllowSpaces]NPC type = null)
        {
            if(type == null)
            {
                int count = Main.npc.Count(n => n != null && n.active);
                Sender.SendInfoMessage($"There {(count == 1 ? "is" : "are")} {count} NPC{(count == 1 ? "" : "s")} on the server.");
            }
            else
            {
                int count = Main.npc.Count(n => n != null && n.active && n.type == type.type);
                Sender.SendInfoMessage($"There {(count == 1 ? "is" : "are")} {count} {type.FullName} NPC{(count == 1 ? "" : "s")} on the server.");
            }
        }

        [Command("playersonteam", "pot")]
        [HelpText("Lists the number of players on a given team.")]
        public void GetNumPlayersOnTeam([TeamColor]int team)
        {
            string CommandNumberToName(int num)
            {
                switch(num)
                {
                    case 0: return "white";
                    case 1: return "red";
                    case 2: return "green";
                    case 3: return "blue";
                    case 4: return "yellow";
                    case 5: return "pink";
                    default: return null;
                }
            }

            int count = TShock.Players.Count(p => p != null && p.Active && p.Team == team);
            Sender.SendInfoMessage($"There {(count == 1 ? "is" : "are")} {count} player{(count == 1 ? "" : "s")} on the {CommandNumberToName(team)} team.");
        }

        [Command("ips")]
        [HelpText("Lists the IP addresses associated with a user.")]
        public void ListIPs([AllowSpaces]User user)
        {
            Sender.SendInfoMessage($"{user.Name}'s IPs: {user.KnownIps}");
        }

        [Command("easy-give")]
        [CommandPermissions("tshock.item.give")]
        [HelpText("Gives another player an item.")]
        public void Give(Item item, TSPlayer player, int amount = 0, [ItemPrefix]int prefix = 0)
        {
            if(amount == 0 || amount > item.maxStack)
            {
                amount = item.maxStack;
            }
            if(player.GiveItemCheck(item.type, EnglishLanguage.GetItemNameById(item.type), item.width, item.height, amount, prefix))
            {
                Sender.SendSuccessMessage(string.Format("Gave {0} {1} {2}(s).", player.Name, amount, item.Name));
                player.SendSuccessMessage(string.Format("{0} gave you {1} {2}(s).", Sender.Name, amount, item.Name));
            }
            else
            {
                Fail("You cannot spawn banned items.");
            }
        }

        [Command]
        [CommandPermissions("tshock.buff.others")]
        [HelpText("Gives all players in the specified group a buff.")]
        public void BuffGroup(Group group, [Buff]int buff, [ParamName("time (seconds)")]int time = 3600)
        {
            foreach(TSPlayer player in TShock.Players)
            {
                if(player != null && player.Active && player.Group == group)
                {
                    player.SetBuff(buff, time * 60);
                    player.SendSuccessMessage(string.Format("{0} has buffed you with {1}({2}) for {3} seconds!",
                                                            Sender.Name, TShock.Utils.GetBuffName(buff),
                                                            TShock.Utils.GetBuffDescription(buff), (time)));
                }
            }
            Sender.SendSuccessMessage(string.Format("You have buffed {0} with {1}({2}) for {3} seconds!",
                                                    group.Name, TShock.Utils.GetBuffName(buff),
                                                    TShock.Utils.GetBuffDescription(buff), (time)));
        }
    }
}
