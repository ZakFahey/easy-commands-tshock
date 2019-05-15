using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Globalization;
using System.Collections.Generic;

namespace EasyCommandsTShock
{
    class TShockParsingRules : ParsingRules<TSPlayer>
    {
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
        
        [ParseRule]
        public int ParseTeam(string arg, TeamColor attributeOverride)
        {
            switch(arg.ToLower())
            {
                case "none":
                case "white": return 0;
                case "red": return 1;
                case "green": return 2;
                case "blue": return 3;
                case "yellow": return 4;
                case "pink": return 5;
            }
            Fail("Invalid syntax! {0} must be a team color!");
            return -1;
        }

        [ParseRule]
        public int ParseItemPrefix(string arg, ItemPrefix attributeOverride)
        {
            /* TODO: in TShock's implementation, there's this check:
            
            if (item.accessory && prefixIds.Contains(PrefixID.Quick))
				{
					prefixIds.Remove(PrefixID.Quick);
					prefixIds.Remove(PrefixID.Quick2);
					prefixIds.Add(PrefixID.Quick2);
				}
				else if (!item.accessory && prefixIds.Contains(PrefixID.Quick))
					prefixIds.Remove(PrefixID.Quick2);
             
            Is it necessary to remove this? And if so, how do you get the item context? */

            List<int> prefixes = TShock.Utils.GetPrefixByIdOrName(arg);
            if(prefixes.Count == 0)
            {
                Fail($"No prefix matched \"{arg}\".", false);
            }
            if(prefixes.Count > 1)
            {
                Fail("More than one match found:\n" + string.Join(", ", prefixes), false);
            }
            if(prefixes[0] <= 0 || prefixes[0] >= Main.maxBuffTypes)
            {
                Fail("Invalid buff ID!");
            }
            return prefixes[0];
        }

        [ParseRule]
        public int ParseBuff(string arg, Buff attributeOverride)
        {
            List<int> buffs = TShock.Utils.GetBuffByName(arg);
            if(buffs.Count == 0)
            {
                Fail("Invalid buff name!", false);
            }
            else if(buffs.Count > 1)
            {
                Fail("More than one match found:\n" + string.Join(", ", buffs.Select(f => Lang.GetBuffName(f))), false);
            }
            return buffs[0];
        }
    }
}
