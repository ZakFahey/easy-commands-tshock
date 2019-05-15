using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;
using Microsoft.Xna.Framework;
using Command = EasyCommands.Command;
using System.Collections.Generic;
using System.Linq;

namespace Example.Commands
{
    [Command("easy-region")]
    [CommandPermissions("tshock.admin.region")]
    [HelpText("Manages regions.")]
    class RegionCommands : CommandCallbacks<TSPlayer>
    {
        [SubCommand("info")]
        public void RegionInfo(Region region, int page = 1)
        {
            List<string> lines = new List<string>
                        {
                            string.Format("X: {0}; Y: {1}; W: {2}; H: {3}, Z: {4}", region.Area.X, region.Area.Y, region.Area.Width, region.Area.Height, region.Z),
                            string.Concat("Owner: ", region.Owner),
                            string.Concat("Protected: ", region.DisableBuild.ToString()),
                        };

            if(region.AllowedIDs.Count > 0)
            {
                IEnumerable<string> sharedUsersSelector = region.AllowedIDs.Select(userId =>
                {
                    User user = TShock.Users.GetUserByID(userId);
                    if(user != null)
                        return user.Name;

                    return string.Concat("{ID: ", userId, "}");
                });
                List<string> extraLines = PaginationTools.BuildLinesFromTerms(sharedUsersSelector.Distinct());
                extraLines[0] = "Shared with: " + extraLines[0];
                lines.AddRange(extraLines);
            }
            else
            {
                lines.Add("Region is not shared with any users.");
            }

            if(region.AllowedGroups.Count > 0)
            {
                List<string> extraLines = PaginationTools.BuildLinesFromTerms(region.AllowedGroups.Distinct());
                extraLines[0] = "Shared with groups: " + extraLines[0];
                lines.AddRange(extraLines);
            }
            else
            {
                lines.Add("Region is not shared with any groups.");
            }

            PaginationTools.SendPage(
                Sender, page, lines, new PaginationTools.Settings
                {
                    HeaderFormat = string.Format("Information About Region \"{0}\" ({{0}}/{{1}}):", region.Name),
                    FooterFormat = string.Format("Type {0}easy-region info {1} {{0}} for more information.", TextOptions.CommandPrefix, region.Name)
                }
            );
        }

        [SubCommand("tp")]
        [CommandPermissions("tshock.tp.self")]
        public void TP([AllowSpaces]Region region)
        {
            Sender.Teleport(region.Area.Center.X * 16, region.Area.Center.Y * 16);
        }
    }
}
