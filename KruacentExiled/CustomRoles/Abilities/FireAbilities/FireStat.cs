using Exiled.API.Features;
using KE.Utils.API.CustomStats;
using KE.Utils.API.CustomStats.GUI;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KruacentExiled.CustomRoles.API.Features;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomRoles.Abilities.FireAbilities
{
    public class FireStat : CustomStatBar, ICustomRoleStat
    {

        public float BaseCapacity => MaxValue;
        public override float CurValue { get; set; }

        public override float MinValue => 0;

        private float maxvalue = 100;
        public override float MaxValue
        {
            get
            {
                return maxvalue;
            }
            set
            {
                maxvalue = value;
            }
        }

        public float FireRegen { get; set; } = 3f;

        public override Color ColorBar => new Color32(252, 186, 3,byte.MaxValue);
        public override Color ColorText => new Color32(255, 255, 255, byte.MaxValue);

        public string CustomRole => "SCP106_SCP457";

        public override int Width => 80;

        public override char Segment => '|';




        public override void ClassChanged()
        {
            maxvalue = 100f;
            CurValue = 0f;
        }

        public override void Init(ReferenceHub ply)
        {
            base.Init(ply);
            StatBarPosition = new SCP106StatBarPosition();

        }

        public override bool Check()
        {
            return false;
            Player player = Player.Get(Hub);
            if (!KECustomRole.Get(player).Any(role => KECustomRole.Get(CustomRole) == role))
            {
                return false;
            }

            return base.Check();
        }

        public override void Update()
        {
            float num = FireRegen * Time.deltaTime;
            if (num > 0f)
            {
                if (CurValue < MaxValue)
                {
                    CurValue = Mathf.MoveTowards(CurValue, MaxValue, num);
                }
            }
            else if (CurValue > 0f)
            {
                CurValue += num;
            }
            base.Update();
        }
        
    }
}
