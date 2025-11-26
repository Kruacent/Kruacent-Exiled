using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using KE.Utils.API.CustomStats;
using System.Linq;
using UnityEngine;

namespace KE.CustomRoles.Abilities.FireAbilities
{
    public class FireStat : CustomStatBar, ICustomRoleStat
    {

        public float BaseCapacity => MaxValue;

        public override float CurValue { get; set; }

        public override float MinValue => 0;

        private float maxvalue = 10;
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

        public float FireRegen { get; set; } = .3f;

        public override Color Color => Color.red;

        public string CustomRole => "SCP106_SCP457";

        public override void ClassChanged()
        {
            maxvalue = 10f;
            CurValue = 0f;
        }

        public override void Init(ReferenceHub ply)
        {
            base.Init(ply);
            StatBarPosition = new Utils.API.Displays.DisplayMeow.Placements.SCP106StatBarPosition();
        }
        public override string GetRaw()
        {
            Player player =Player.Get(Hub);
            
            if(!KECustomRole.Get(player).Any(role => KECustomRole.Get(CustomRole) == role))
            {
                return string.Empty;
            }


            return base.GetRaw();
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
