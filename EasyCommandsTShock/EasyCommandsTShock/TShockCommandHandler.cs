using System;
using EasyCommands;
using EasyCommands.Defaults;
using TShockAPI;
using System.Linq;

namespace EasyCommandsTShock
{
    public class TShockCommandHandler : CommandHandler<TSPlayer>
    {
        protected override void Initialize()
        {
            Context.TextOptions.CommandPrefix = TShock.Config.CommandSpecifier;
            AddParsingRules(typeof(DefaultParsingRules<TSPlayer>));
            AddParsingRules(typeof(TShockParsingRules));
        }

        protected override Type CommandRepositoryToUse() => typeof(TShockCommandRepository);

        protected override void SendFailMessage(TSPlayer sender, string message)
        {
            sender.SendErrorMessage(message);
        }

        public override void PreCheck(TSPlayer sender, CommandDelegate<TSPlayer> command)
        {
            CommandPermissions permissions = command.GetCustomAttribute<CommandPermissions>();

            // Stop a user from running a command or subcommand if they don't have permission to use it
            if(permissions != null && permissions.Permissions.Any(p => !sender.HasPermission(p)))
            {
                Fail("You don't have the necessary permission to do that.");
            }
        }
    }
}
