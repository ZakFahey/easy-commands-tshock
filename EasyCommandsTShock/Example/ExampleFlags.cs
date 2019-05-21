using System;
using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using EasyCommands.Commands;
using TerrariaApi.Server;
using Terraria;
using EasyCommandsTShock;
using Microsoft.Xna.Framework;
using Command = EasyCommands.Command;
using System.Linq;
using TShockAPI.Localization;

namespace Example
{
    [FlagsArgument]
    class ExampleFlags
    {
        [FlagParams("-a")]
        public string A = "DEFAULT";

        [FlagParams("-b")]
        public int B = 0;

        [FlagParams("-c", "-C")]
        public TSPlayer C = null;

        [FlagParams("--d")]
        public bool D = false;
    }
}
