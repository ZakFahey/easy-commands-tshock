using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;

namespace EasyCommandsTShock
{
    class TShockCommandRepository : CommandRepository<TSPlayer>
    {
        public TShockCommandRepository(Context<TSPlayer> context) : base(context) { }

        public override void Invoke(TSPlayer sender, string command)
        {
            Commands.HandleCommand(sender, command);
        }

        protected override void AddCommand(CommandDelegate<TSPlayer> command, string[] names)
        {
            CommandDelegate commandDelegate = (CommandArgs args) =>
            {
                try
                {
                    int firstSpace = args.Message.IndexOf(' ');
                    // Get the command text without the command name
                    string rawCommandArguments = firstSpace == -1 ? "" : args.Message.Substring(firstSpace + 1);
                    command.Invoke(args.Player, rawCommandArguments);
                }
                // TShock's command system does its own exception handling, so we have to catch error messages here.
                catch(CommandParsingException e)
                {
                    args.Player.SendErrorMessage(e.Message);
                }
                catch(CommandExecutionException e)
                {
                    args.Player.SendErrorMessage(e.Message);
                }
            };

            var helpText = command.GetCustomAttribute<HelpText>();

            var tshockCommand = new TShockAPI.Command(commandDelegate, names)
            {
                HelpText = $"{(helpText != null ? helpText.Documentation : "")} Syntax: {command.SyntaxDocumentation()}"
            };

            Commands.ChatCommands.Add(tshockCommand);
        }
    }
}
