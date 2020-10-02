# Easy Commands for TShock

Has this ever happened to you?

![Literally just TShock's code](https://raw.githubusercontent.com/ZakFahey/easy-commands-tshock/master/code.png)

Well then you may be interested in this library. It removes the boilerplate of writing code for TShock commands by handling input parsing and validation for you so that you can focus on what's important. Essentially, you make a method, and the arguments of the method generate the arguments of the command. You can make optional commands, subcommands, and commands with multi-word arguments that don't require quotes. It lets you go from this:
```
private static void Give(CommandArgs args)
{
  if (args.Parameters.Count < 2)
  {
    args.Player.SendErrorMessage(
      "Invalid syntax! Proper syntax: {0}give <item type/id> <player> [item amount] [prefix id/name]", Specifier);
    return;
  }
  if (args.Parameters[0].Length == 0)
  {
    args.Player.SendErrorMessage("Missing item name/id.");
    return;
  }
  if (args.Parameters[1].Length == 0)
  {
    args.Player.SendErrorMessage("Missing player name.");
    return;
  }
  int itemAmount = 0;
  int prefix = 0;
  var items = TShock.Utils.GetItemByIdOrName(args.Parameters[0]);
  args.Parameters.RemoveAt(0);
  string plStr = args.Parameters[0];
  args.Parameters.RemoveAt(0);
  if (args.Parameters.Count == 1)
    int.TryParse(args.Parameters[0], out itemAmount);
  if (items.Count == 0)
  {
    args.Player.SendErrorMessage("Invalid item type!");
  }
  else if (items.Count > 1)
  {
    TShock.Utils.SendMultipleMatchError(args.Player, items.Select(i => $"{i.Name}({i.netID})"));
  }
  else
  {
    var item = items[0];

    if (args.Parameters.Count == 2)
    {
      int.TryParse(args.Parameters[0], out itemAmount);
      var prefixIds = TShock.Utils.GetPrefixByIdOrName(args.Parameters[1]);
      if (item.accessory && prefixIds.Contains(PrefixID.Quick))
      {
        prefixIds.Remove(PrefixID.Quick);
        prefixIds.Remove(PrefixID.Quick2);
        prefixIds.Add(PrefixID.Quick2);
      }
      else if (!item.accessory && prefixIds.Contains(PrefixID.Quick))
        prefixIds.Remove(PrefixID.Quick2);
      if (prefixIds.Count == 1)
        prefix = prefixIds[0];
    }

    if (item.type >= 1 && item.type < Main.maxItemTypes)
    {
      var players = TShock.Utils.FindPlayer(plStr);
      if (players.Count == 0)
      {
        args.Player.SendErrorMessage("Invalid player!");
      }
      else if (players.Count > 1)
      {
        TShock.Utils.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
      }
      else
      {
        var plr = players[0];
        if (plr.InventorySlotAvailable || (item.type > 70 && item.type < 75) || item.ammo > 0 || item.type == 58 || item.type == 184)
        {
          if (itemAmount == 0 || itemAmount > item.maxStack)
            itemAmount = item.maxStack;
          if (plr.GiveItemCheck(item.type, EnglishLanguage.GetItemNameById(item.type), itemAmount, prefix))
          {
            args.Player.SendSuccessMessage(string.Format("Gave {0} {1} {2}(s).", plr.Name, itemAmount, item.Name));
            plr.SendSuccessMessage(string.Format("{0} gave you {1} {2}(s).", args.Player.Name, itemAmount, item.Name));
          }
          else
          {
            args.Player.SendErrorMessage("You cannot spawn banned items.");
          }

        }
        else
        {
          args.Player.SendErrorMessage("Player does not have free slots!");
        }
      }
    }
    else
    {
      args.Player.SendErrorMessage("Invalid item type!");
    }
  }
}
```
to this:
```
[Command("easy-give")]
[CommandPermissions("tshock.item.give")]
[HelpText("Gives another player an item.")]
public void Give(Item item, TSPlayer player, int amount = 0, [ItemPrefix]int prefix = 0)
{
    if(amount == 0 || amount > item.maxStack)
    {
        amount = item.maxStack;
    }
    if(player.GiveItemCheck(item.type, EnglishLanguage.GetItemNameById(item.type), item.width, item.height, amount, prefix))
    {
        Sender.SendSuccessMessage(string.Format("Gave {0} {1} {2}(s).", player.Name, amount, item.Name));
        player.SendSuccessMessage(string.Format("{0} gave you {1} {2}(s).", Sender.Name, amount, item.Name));
    }
    else
    {
        Fail("You cannot spawn banned items.");
    }
}
```

## Installation
To install, simply go to Visual Studio's Package Manager Console in your TShock plugin project and run these commands:
```
Install-Package EasyCommands
Install-Package EasyCommandsTShock
```

When you run your plugin, you will need to copy `EasyCommands.dll` and `EasyCommandsTShock.dll` into your `ServerPlugins` folder as well as your own plugin.

## How to use
This library is an extension of my accompanying, more general-purpose library, [Easy Commands](https://github.com/ZakFahey/easy-commands). Read up on the [documentation](https://github.com/ZakFahey/easy-commands/tree/master/Documentation) there to get a more in-depth view of how everything works. You can also view the Example project in this repository to see the code in action.

If you want to run the Example project, build it and copy `EasyCommands.dll`, `EasyCommandsTShock.dll`, and `Example.dll` to your `ServerPlugins` folder.

To register your commands, all you need to do is create a command handler and have it register the namespace where your commands are. See [Plugin.cs](https://github.com/ZakFahey/easy-commands-tshock/blob/master/EasyCommandsTShock/Example/Plugin.cs):
```
CommandHandler = new TShockCommandHandler();
// You can also use a Type for this argument to register a single class
CommandHandler.RegisterCommands("Example.Commands");
```
In your commands namespace, you can create command callbacks by creating classes that inherit from `EasyCommands.CommandCallbacks<TSPlayer>`. To see the documentation for the syntax of these command callbacks, please see [this documentation](https://github.com/ZakFahey/easy-commands/blob/master/Documentation/Commands.md). You can use the [HelpText], [CommandPermissions], [AllowServer], and [DoLog] attributes with the command callbacks.

By default, this library supports using these types for method arguments: string, int, double, float, bool, TSPlayer, UserAccount, Item, Group, NPC, Region, Color, team color using [TeamColor], buff using [Buff], and item prefix using [ItemPrefix] (NOTE: item prefix may not currently work with the "quick" prefix; see [here](https://github.com/ZakFahey/easy-commands-tshock/blob/master/EasyCommandsTShock/EasyCommandsTShock/TShockParsingRules.cs#L156)). If those arguments aren't enough, you can add your own parameter handlers. See [this documentation](https://github.com/ZakFahey/easy-commands/blob/master/Documentation/ParsingRules.md).
