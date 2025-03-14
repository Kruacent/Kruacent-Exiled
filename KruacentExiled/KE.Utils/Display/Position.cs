using KE.Utils.Display.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.Display
{
    public struct Position
    {
        private HPosition _hposition;
        public HPosition HPosition { get { return _hposition; } }
        private float _vposition;
        [Obsolete("use RawVPosition Instead")]
        public VPosition VPosition { get { return (VPosition)_vposition; } }
        public float RawVPosition { get { return _vposition; } }

        private static readonly Position globalEvent = new (HPosition.Center, 900);
        private static readonly Position customItemShow = new(HPosition.Right, 200);
        private static readonly Position customRoleShow = new(HPosition.Left, 200);
        private static readonly Position customRoleEffect = new(HPosition.Left, 400);
        private static readonly Position customGlobalEffect = new(HPosition.Right, 400);
        public static Position GlobalEvent
        {
            get { return globalEvent; }
        }
        public static Position CustomItemShow
        {
            get { return customItemShow; }
        }
        public static Position CustomRoleShow
        {
            get { return customRoleShow; }
        }
        public static Position CustomRoleEffect
        {
            get { return customRoleEffect; }
        }
        public static Position CustomGlobalEffect
        {
            get { return customGlobalEffect; }
        }

        public Position(HPosition hposition, VPosition vposition)
        {
            _hposition = hposition;
            _vposition = (float) vposition;
        }

        public Position(HPosition hposition, float vposition)
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
