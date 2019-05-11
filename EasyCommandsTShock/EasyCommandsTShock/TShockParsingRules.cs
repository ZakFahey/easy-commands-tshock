using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Globalization;

namespace EasyCommandsTShock
{
    class TShockParsingRules : ParsingRules<TSPlayer>
    {
        //TODO: time of day, team color, item prefix, buff - these are primitive types

        [ParseRule]
        public TSPlayer ParsePlayer(string arg)
        {
            var players = TShock.Utils.FindPlayer(arg);
            if(players.Count == 0)
            {
                Fail("Invalid player!", false);
            }
            if(players.Count > 1)
            {
                Fail("More than one match found:\n" + string.Join(", ", players.Select(p => p.Name)), false);
            }
            return players[0];
        }

        [ParseRule]
        public User ParseUser(string arg)
        {
            User user = TShock.Users.GetUserByName(arg);
            if(user == null)
            {
                Fail($"User {arg} does not exist.", false);
            }
            return user;
        }

        [ParseRule]
        public Item ParseItem(string arg)
        {
            var items = TShock.Utils.GetItemByIdOrName(arg);
            if(items.Count == 0)
            {
                Fail("Invalid item.", false);
            }
            if(items.Count > 1)
            {
                Fail("More than one match found:\n" + string.Join(", ", items.Select(i => i.Name)), false);
            }
            return items[0];
        }

        [ParseRule]
        public Group ParseGroup(string arg)
        {
            var group = TShock.Groups.FirstOrDefault(x => x.Name == arg);
            if(group == null)
            {
                Fail($"Group {arg} does not exist.", false);
            }
            return group;
        }

        [ParseRule]
        public NPC ParseNPC(string arg)
        {
            var npcs = TShock.Utils.GetNPCByIdOrName(arg);
            if(npcs.Count == 0)
            {
                Fail("Invalid mob type!", false);
            }
            if(npcs.Count > 1)
            {
                Fail("More than one match found:\n" + string.Join(", ", npcs.Select(n => $"{n.FullName}({n.type})")), false);
            }
            return npcs[0];
        }

        [ParseRule]
        public Region ParseRegion(string arg)
        {
            Region region = TShock.Regions.GetRegionByName(arg);
            int id;
            if(region == null && int.TryParse(arg, out id))
            {
                region = TShock.Regions.GetRegionByID(id);
            }
            if(region == null)
            {
                Fail($"Region \"{arg}\" does not exist.");
            }
            return region;
        }

        [ParseRule]
        public Color ParseColor(string arg)
        {
            // First check by name
            Color color;
            if(ColorNames.GetColorFromName(arg, out color))
            {
                return color;
            }

            // Then check by rgb
            string[] rgb = arg.Split(',').Select(x => x.Trim()).ToArray();
            if(rgb.Length == 3)
            {
                try
                {
                    return new Color(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
                }
                catch { }
            }

            // Then check by hex code
            if(arg.StartsWith("#") && arg.Length == 7)
            {
                uint hexNum = uint.Parse(arg.Substring(1), NumberStyles.HexNumber);
                return new Color(hexNum);
            }

            // Nothing works, so invalid argument
            Fail("{0} must be a valid color! You can use a color name, comma-separated RGB triplet, or hex value.");
            return new Color();
        }
    }
}
