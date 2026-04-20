using System.Collections.Generic;

namespace KruacentExiled.Map.Others.BlackoutNDoor.Handlers
{
    public class Pattern
    {
        public static readonly HashSet<Pattern> AllPatterns = new HashSet<Pattern>()
        {
            new Pattern
            (new List<MapEvent>()
            {
                new Blackout(),new DoorStuck()
            })
            ,
            new Pattern
            (new List<MapEvent>()
            {
                new Blackout(),new Blackout(),new DoorStuck(),new DoorStuck()
            })
            ,
            new Pattern
            (new List<MapEvent>()
            {
                new DoorStuck(),new Blackout(),new Both(),new Blackout(),new DoorStuck(),new Both()
            })            
            ,
            new Pattern
            (new List<MapEvent>()
            {
                new DoorStuck(),new Blackout(),new Both()
            })
            ,
            new Pattern
            (new List<MapEvent>()
            {
                new DoorStuck(),new Both()
            })
            ,
            new Pattern
            (new List<MapEvent>()
            {
                new Blackout(),new Both()
            })
            ,
            new Pattern
            (new List<MapEvent>()
            {
                new Both()
            })
        };


        private int current = -1;
        private readonly List<MapEvent> _pattern;

        public Pattern(List<MapEvent> pattern)
        {
            _pattern = new List<MapEvent>();
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

        public MapEvent SeeNext()
        {
            return _pattern[(current + 1) % _pattern.Count];
        }




    }
}
