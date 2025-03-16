using Exiled.API.Features;
using Exiled.API.Interfaces;
using KE.Utils.Display.Enums;
using MEC;
using RueI.Displays;
using RueI.Elements;
using RueI.Elements.Delegates;
using RueI.Elements.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KE.Utils.Display
{
    public class DisplayPlayer
    {
        private static Dictionary<Player,DisplayPlayer> _displays = new();
        private RueI.Displays.Display _display;
        private Dictionary<Position, Element> _hints = new();
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

        public Element Hint(Position position, string text)
        {
            if (_hints.ContainsKey(position)) return null;
            SetElement element = new((float)position.VPosition, $"<align={position.HPosition}>" + text + "</align>");
            _display.Elements.Add(element);
            _hints.Add(position, element);
            UpdateCore();
            return element;
        }

        [Obsolete("doesn't work don't use it",true)]
        public DynamicElement Hint(Position position, GetContent getContent)
        {
            DynamicElement element = new(getContent, position.RawVPosition);
            _display.Elements.Add(element);
            _hints.Add(position, element);
            UpdateCore();
            return element;
        }


        public Element Hint(Position position,string text,float seconds)
        {
            if (_hints.ContainsKey(position)) return null;
            SetElement element = new((float)position.VPosition, $"<align={position.HPosition.ToString().ToLower()}>"+text+"</align>");
            _display.Elements.Add(element);
            _hints.Add(position, element);
            UpdateCore();
            Timing.CallDelayed(seconds, () =>
            {
                _display.Elements.Remove(element);
                _hints.Remove(position);
                UpdateCore();
            });
            return element;
        }


        public Element Hint(RueIHint hint)
        {
            if (hint.Duration < 0)
                return Hint(hint.Position, hint.RawContent);
            return Hint(hint.Position,hint.RawContent,hint.Duration);
        }

        public bool RemoveHint(Position placement)
        {
            if (!_hints.ContainsKey(placement)) return false;
            bool result = _display.Elements.Remove(_hints[placement]);
            _hints.Remove(placement);
            UpdateCore();
            return result;
        }

        public bool RemoveHint(Element elem)
        {
            bool result = _display.Elements.Remove(elem);
            _hints.Remove(_hints.First(x => x.Value == elem).Key);
            UpdateCore();
            return result;
        }

        private void UpdateCore() => UpdateCore(_player);
        public static void UpdateCore(Player player) => DisplayCore.Get(player.ReferenceHub).Update();

        
    }
    



}
