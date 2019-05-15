using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;

namespace EasyCommandsTShock
{
    /// <summary>
    /// The documentation for a command
    /// </summary>
    public class HelpText : CustomAttribute
    {
        public string Documentation { get; private set; }

        public HelpText(string documentation)
        {
            Documentation = documentation;
        }
    }

    /// <summary>
    /// The permissions for the command
    /// </summary>
    public class CommandPermissions : CustomAttribute
    {
        public string[] Permissions { get; private set; }

        public CommandPermissions(params string[] permissions)
        {
            Permissions = permissions;
        }
    }

    /// <summary>
    /// Whether the server can run this command
    /// </summary>
    public class AllowServer : CustomAttribute
    {
        public bool Allow { get; private set; }

        public AllowServer(bool allow)
        {
            Allow = allow;
        }
    }

    /// <summary>
    /// Whether the command is logged
    /// </summary>
    public class DoLog : CustomAttribute
    {
        public bool Log { get; private set; }

        public DoLog(bool log)
        {
            Log = log;
        }
    }

    /// <summary>
    /// Specifies that a command argument is read as a team color
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class TeamColor : Attribute
    {
    }

    /// <summary>
    /// Specifies that a command argument is read as an item prefix
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ItemPrefix : Attribute
    {
    }

    /// <summary>
    /// Specifies that a command argument is read as a buff
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class Buff : Attribute
    {
    }
}
