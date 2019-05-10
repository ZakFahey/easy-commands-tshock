using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using System;

namespace EasyCommandsTShock
{
    class TShockParsingRules<TSender> : ParsingRules<TSender>
    {
        //TODO: implement
        //TODO: time of day, team color, item prefix, buff - these are primitive types
        //Item prefixes go with items. Issue?

        [ParseRule]
        public TSPlayer ParsePlayer(string arg)
        {
            throw new NotImplementedException();
        }

        [ParseRule]
        public User ParseUser(string arg)
        {
            throw new NotImplementedException();
        }

        [ParseRule]
        public Item ParseItem(string arg)
        {
            throw new NotImplementedException();
        }

        [ParseRule]
        public Group ParseGroup(string arg)
        {
            throw new NotImplementedException();
        }

        [ParseRule]
        public NPC ParseNPC(string arg)
        {
            throw new NotImplementedException();
        }

        [ParseRule]
        public Region ParseRegion(string arg)
        {
            throw new NotImplementedException();
        }
    }
}
