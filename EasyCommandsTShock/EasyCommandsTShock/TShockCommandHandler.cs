using System;
using EasyCommands;
using EasyCommands.Defaults;
using EasyCommands.Commands;
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

        protected override void HandleCommandException(Exception e)
        {
            TShock.Log.Error(e.ToString());
        }

        public override bool CanSeeCommand(TSPlayer sender, CommandDelegate<TSPlayer> command)
        {
            var permissions = command.GetCustomAttribute<CommandPermissions>();
            return permissions == null || permissions.Permissions.Any(perm => sender.Group.HasPermission(perm));
        }

        public override void PreCheck(TSPlayer sender, CommandDelegate<TSPlayer> command)
        {
            // Stop the server from running a command if he's not allowed to, it's allowed by default
            if (sender is TSServerPlayer && command.GetCustomAttribute<AllowServer>()?.Allow == false)
            {
                Fail("The server doesn't have the permission to execute this command.");
            }

            CommandPermissions permissions = command.GetCustomAttribute<CommandPermissions>();

            // Stop a user from running a command or subcommand if they don't have permission to use it
            if(permissions != null && permissions.Permissions.Any(p => !sender.HasPermission(p)))
            {
                Fail("You don't have the necessary permission to do that.");
            }
        }
    }
}
