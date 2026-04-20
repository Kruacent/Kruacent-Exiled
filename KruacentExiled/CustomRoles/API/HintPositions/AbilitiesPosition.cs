using Exiled.API.Enums;
using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KruacentExiled.CustomRoles.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.CustomRoles.API.HintPositions
{
    public class AbilitiesPosition : HintPosition
    {


        public const float BaseYPosition = 900;
        public const float Increments = -50;
        private float yposition = BaseYPosition;
        public override float Xposition => -300;

        public override float Yposition => yposition;

        private int index;
        public int Index => index;

        private static List<AbilitiesPosition> nonalloc = new List<AbilitiesPosition>(KEAbilities.InitialAbilitySlot);

        
        public static AbilitiesPosition GetIndex(int index)
        {
            if (!nonalloc.TryGet(index, out AbilitiesPosition position))
            {
                position = new AbilitiesPosition()
                {
                    yposition = BaseYPosition - index * 50,
                    index = index
                };
            }
            Log.Debug($"get index {index} y pos=" + position.Yposition);
            return position;
        }

        public override HintAlignment HintAlignment => HintAlignment.Left;
    }
}
