using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using EasyCommands.Commands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;
using Command = EasyCommands.Command;

namespace Example.Commands
{
    class FlagCommand : CommandCallbacks<TSPlayer>
    {
        [Command("flag-test")]
        [HelpText("Demonstrates the flags argument.")]
        public void FlagTest(ExampleFlags flags)
        {
            string c = flags.C?.Name;
            
            Console.WriteLine($"A: {flags.A}");
            Console.WriteLine($"B: {flags.B}");
            Console.WriteLine($"C: {(c == null ? "null" : c)}");
            Console.WriteLine($"D: {flags.D}");
        }
    }
}
