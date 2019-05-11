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

    public class CommandPermissions : CustomAttribute
    {
        public string[] Permissions { get; private set; }

        public CommandPermissions(params string[] permissions)
        {
            Permissions = permissions;
        }
    }

    public class AllowServer : CustomAttribute
    {
        public bool Allow { get; private set; }

        public AllowServer(bool allow)
        {
            Allow = allow;
        }
    }

    public class DoLog : CustomAttribute
    {
        public bool Log { get; private set; }

        public DoLog(bool log)
        {
            Log = log;
        }
    }
}
