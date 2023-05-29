using System;
using TShockAPI;
using EasyCommands;
using EasyCommands.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCommandsTShock
{
    public class TShockCommandRepository : CommandRepository<TSPlayer>
    {
        public TShockCommandRepository(Context<TSPlayer> context) : base(context) { }

        public override async Task Invoke(TSPlayer sender, string command)
        {
            Commands.HandleCommand(sender, command);
        }

        /// <summary> Handles when a command throws an unexpected exception. </summary>
        protected virtual void HandleCommandException(CommandArgs args, Exception e)
        {
            TShock.Log.Error(e.ToString());
            args.Player.SendErrorMessage(Context.TextOptions.CommandThrewException);
        }

        protected override void AddCommand(CommandDelegate<TSPlayer> command, string[] names)
        {
            CommandDelegate commandDelegate = async (CommandArgs args) =>
            {
                try
                {
                    int firstSpace = args.Message.IndexOf(' ');
                    // Get the command text without the command name
                    string rawCommandArguments = firstSpace == -1 ? "" : args.Message.Substring(firstSpace + 1);
                    await command.Invoke(args.Player, rawCommandArguments);
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
                catch (Exception e)
                {
                    HandleCommandException(args, e);
                }
            };

            var helpText = command.GetCustomAttribute<HelpText>();
            var permissions = command.GetCustomAttribute<CommandPermissions>();
            var allowServer = command.GetCustomAttribute<AllowServer>();
            var doLog = command.GetCustomAttribute<DoLog>();

            var tshockCommand = new TShockAPI.Command(permissions != null ? permissions.Permissions.ToList() : new List<string>(), commandDelegate, names)
            {
                HelpText = $"{(helpText != null ? helpText.Documentation : "")} Syntax: {command.SyntaxDocumentation(TSPlayer.Server)}",
                AllowServer = allowServer != null ? allowServer.Allow : true,
                DoLog = doLog != null ? doLog.Log : true
            };

            Commands.ChatCommands.Add(tshockCommand);
        }
    }
}
