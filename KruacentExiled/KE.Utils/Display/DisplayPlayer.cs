using Exiled.API.Features;
using Exiled.API.Interfaces;
using KE.Utils.Display.Enums;
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
            UpdateCore(_player);
            return element;
        }


        public Element Hint(Position position,string text,float seconds)
        {
            if (_hints.ContainsKey(position)) return null;
            SetElement element = new((float)position.VPosition, $"<align={position.HPosition.ToString().ToLower()}>"+text+"</align>");
            _display.Elements.Add(element);
            _hints.Add(position, element);
            UpdateCore(_player);
            Timing.CallDelayed(seconds, () =>
            {
                _display.Elements.Remove(element);
                _hints.Remove(position);
                UpdateCore(_player);
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
            UpdateCore(_player);
            return result;
        }

        public bool RemoveHint(Element elem)
        {
            bool result = _display.Elements.Remove(elem);
            _hints.Remove(_hints.First(x => x.Value == elem).Key);
            UpdateCore(_player);
            return result;
        }


        public static void UpdateCore(Player player) => DisplayCore.Get(player.ReferenceHub).Update();

        
    }
    public struct Position
    {
        private HPosition _hposition;
        public HPosition HPosition { get { return _hposition; } }
        private VPosition _vposition;
        public VPosition VPosition { get { return _vposition; } }

        public Position(HPosition hposition, VPosition vposition)
        {
            _hposition = hposition;
            _vposition = vposition;
        }

        public override bool Equals(object obj)
        {
            Position pos = (Position)obj;
            return pos.VPosition == VPosition && pos.HPosition == HPosition;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }



}
