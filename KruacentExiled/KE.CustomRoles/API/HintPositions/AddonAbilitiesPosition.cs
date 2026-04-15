using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using KE.CustomRoles.API.Features;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.HintPositions
{
    public class AddonAbilitiesPosition : HintPosition
    {


        private float yposition = AbilitiesPosition.BaseYPosition;
        public override float Xposition => -350;

        public override float Yposition => yposition;


        private static List<AddonAbilitiesPosition> nonalloc = new List<AddonAbilitiesPosition>(KEAbilities.InitialAbilitySlot);


        public static AddonAbilitiesPosition Get(AbilitiesPosition abilitiesPosition)
        {
            if (abilitiesPosition is null) throw new ArgumentNullException();
            int index = abilitiesPosition.Index;

            if (!nonalloc.TryGet(index, out AddonAbilitiesPosition position))
            {
                position = new AddonAbilitiesPosition()
                {
                    yposition = abilitiesPosition.Yposition+5
                };
            }
            Log.Debug($"addon get {index} y pos=" + position.Yposition);
            return position;

        }

        public override HintAlignment HintAlignment => HintAlignment.Left;
    }
}
