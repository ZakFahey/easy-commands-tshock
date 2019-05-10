using EasyCommands;
using EasyCommands.Defaults;
using TShockAPI;


namespace EasyCommandsTShock
{
    public class TShockCommandHandler : CommandHandler<TSPlayer>
    {
        protected override void Initialize()
        {
            AddParsingRules(typeof(DefaultParsingRules<TSPlayer>));
            AddParsingRules(typeof(TShockParsingRules<TSPlayer>));
        }

        protected override void SendFailMessage(TSPlayer sender, string message)
        {
            sender.SendErrorMessage(message);
        }
    }
}
