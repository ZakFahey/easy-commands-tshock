using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;

namespace Example
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public Plugin(Main game) : base(game) { }

        public static TShockCommandHandler CommandHandler;

        public override string Author => "Zak Fahey";

        public override string Description => "Demo utilizing the Easy Commands library.";

        public override string Name => "Easy Commands for TShock Example";

        public override Version Version => new Version(1, 0);

        public override void Initialize()
        {
            CommandHandler = new TShockCommandHandler();
            CommandHandler.AddFlagRule(typeof(ExampleFlags));
            CommandHandler.RegisterCommands("Example.Commands");
        }
    }
}
