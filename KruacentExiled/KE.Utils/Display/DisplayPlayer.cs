using Exiled.API.Features;
using MEC;
using RueI.Displays;
using RueI.Elements;
using RueI.Elements.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.Display
{
    public class DisplayPlayer
    {
        private static Dictionary<Player,DisplayPlayer> _displays = new();
        private RueI.Displays.Display _display;
        private Player _player;
        public Player Player { get { return _player; } }
        public static DisplayPlayer Get(Player player)
        {
            if (!_displays.ContainsKey(player))
                _displays.Add(player, new DisplayPlayer(player));
            return _displays[player];
        }

        public DisplayPlayer(Player player)
        {
            _player = player;
            _display = new (DisplayCore.Get(player.ReferenceHub));
        }

        public Element Hint(float position, string text)
        {
            SetElement element = new(position, text);
            _display.Elements.Add(element);
            UpdateCore(_player);
            return element;
        }


        public Element Hint(float position,string text,float seconds)
        {
            SetElement element = new(position, text);
            _display.Elements.Add(element);
            UpdateCore(_player);
            Timing.CallDelayed(seconds, () =>
            {
                _display.Elements.Remove(element);
                UpdateCore(_player);
            });
            return element;
        }

        public bool RemoveHint(Element elem)
        {
            bool result = _display.Elements.Remove(elem);
            UpdateCore(_player);
            return result;
        }


        public static void UpdateCore(Player player) => DisplayCore.Get(player.ReferenceHub).Update();


    }


        
}
