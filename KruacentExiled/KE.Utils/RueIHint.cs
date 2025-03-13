using Exiled.API.Features;
using Exiled.Events.Commands.PluginManager;
using RueI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils
{

    /// <summary>
    /// Hint with position
    /// </summary>
    public class RueIHint
    {
        public Hint Hint { get; set; }
        public float Position { get; set; }
        public RueIHint(Hint hint,float position) 
        {
            Hint = hint;
            Position = position;
        }
        public RueIHint(string content, float duration = 3f, bool show = true, float position = 0)
        {
            Hint = new Hint(content,duration,show);
            Position = position;

        }
        public RueIHint() 
        { 
            Hint = new Hint();
            Position = 0;
        }
    }
}
