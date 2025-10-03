using Exiled.API.Features;
using Mirror;
using PlayerRoles.PlayableScps.Scp3114;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.Map.Others.BlackoutNDoor.Handlers
{
    public class Pattern
    {
        public static readonly HashSet<Pattern> AllPatterns = new()
        {
            new Pattern
            ([
                new Blackout(),new DoorStuck()
            ])
            ,
            new Pattern
            ([
                new Blackout(),new Blackout(),new DoorStuck(),new DoorStuck()
            ])
            ,
            new Pattern
            ([
                new DoorStuck(),new Blackout(),new Both(),new Blackout(),new DoorStuck(),new Both()
            ])            
            ,
            new Pattern
            ([
                new DoorStuck(),new Blackout(),new Both()
            ])
            ,
            new Pattern
            ([
                new DoorStuck(),new Both()
            ])
            ,
            new Pattern
            ([
                new Blackout(),new Both()
            ])
            ,
            new Pattern
            ([
                new Both()
            ])
        };


        private int current = -1;
        private readonly List<MapEvent> _pattern;

        public Pattern(List<MapEvent> pattern)
        {
            _pattern = new();
            for (int i = 0; i < pattern.Count; i++)
            {
                _pattern.Add(pattern[i]);
            }
        }


        public MapEvent GetNext()
        {
            current = (current + 1) % _pattern.Count;
            return _pattern[current];
        }




    }
}
