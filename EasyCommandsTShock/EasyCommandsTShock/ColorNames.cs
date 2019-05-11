using TShockAPI;
using TShockAPI.DB;
using EasyCommands;
using TerrariaApi.Server;
using Terraria;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace EasyCommandsTShock
{
    /// <summary>
    /// Utility class for the Color parsing rule
    /// </summary>
    static class ColorNames
    {
        private static Dictionary<string, Color> colors = new Dictionary<string, Color>();

        static ColorNames()
        {
            foreach(PropertyInfo color in typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                if(color.DeclaringType != typeof(Color)) continue;
                colors[color.Name] = (Color)color.GetValue(null);
            }
        }

        public static bool GetColorFromName(string name, out Color color)
        {
            return colors.TryGetValue(name, out color);
        }
    }
}
