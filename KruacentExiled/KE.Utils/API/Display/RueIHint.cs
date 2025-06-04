using KE.Utils.API.Display.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Display
{
    public class RueIHint
    {
        private Position _position;
        public Position Position { get { return _position; } }
        private string _content;
        public string RawContent { get { return _content; } }
        private float _duration = -1;
        public float Duration { get { return _duration; } }

        public RueIHint(HPosition hposition, VPosition vposition, string content)
        {
            _position = new(hposition, vposition);
            _content = content;
        }
        public RueIHint(HPosition hposition, VPosition vposition, string content, float duration)
        {
            _position = new(hposition, vposition);
            _content = content;
            _duration = duration;
        }



    }
}
