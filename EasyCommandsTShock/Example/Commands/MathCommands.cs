using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using EasyCommands.Commands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;
using Command = EasyCommands.Command;

// A few simple commands copied from the Easy Commands example project
namespace Example.Commands
{
    class MathCommands : CommandCallbacks<TSPlayer>
    {
        [Command("add")]
        [HelpText("Adds two integers together.")]
        public void Add(int num1, int num2)
        {
            Sender.SendInfoMessage($"{num1} + {num2} = {num1 + num2}");
        }

        [Command] // If we do not include the command name, it can be inferred from the method name.
        [HelpText("Subtracts num2 from num1.")]
        public void Subtract(
            [ParamName("num1")]
            int num_1,
            [ParamName("num2")]
            int num_2)
        {
            Sender.SendInfoMessage($"{num_1} - {num_2} = {num_1 - num_2}");
        }

        [Command("divide", "div")]
        [HelpText("Divides num1 by num2.")]
        public void Divide(float num1, float num2)
        {
            Sender.SendInfoMessage($"{num1} / {num2} = {num1 / num2}");
        }

        [Command("add3or4")]
        [HelpText("Adds 3 or 4 integers together.")]
        public void Add3or4(int num1, int num2, int num3, int num4 = 0)
        {
            Sender.SendInfoMessage($"sum = {num1 + num2 + num3 + num4}");
        }
    }
}
