using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;

namespace EasyCommandsTShock
{
    public class HelpText : CustomAttribute
    {
        public string Documentation { get; private set; }

        public HelpText(string documentation)
        {
            Documentation = documentation;
        }
    }

    public class Permissions : CustomAttribute
    {
        public string Perms { get; private set; }

        public Permissions(string perms)
        {
            Perms = perms;
        }
    }
}
