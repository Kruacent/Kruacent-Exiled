using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.API.Features
{
    public struct RecyclableSettingId : IEquatable<RecyclableSettingId>
    {
        public const int MinThreshold = 16;
        public readonly int Value;

        private static Queue<int> FreeIds = new Queue<int>();
        private static int _autoIncrement;

        private static int Min => MainPlugin.Configs.CustomScpSliderRangeMin;
        private static int Max => MainPlugin.Configs.CustomScpSliderRangeMax;

        public RecyclableSettingId(SettingBase setting)
        {
            Value = setting.Id;
        }
        
        /*public RecyclableSettingId()
        {
            int num = MinThreshold;
            int value;
            if (FreeIds.Count >= num)
            {
                value = FreeIds.Dequeue();
            }
            else
            {
                value = _autoIncrement++ + Min;
                if(value > Max)
                {
                    throw new ArgumentOutOfRangeException("ID out of range, please increase the CustomScpSliderRangeMax in the plugins settings");
                }
            }
            Log.Debug("creating id =" + value);
            Value = value;

        }*/


        public void Destroy()
        {
            if(Value != 0)
            {
                FreeIds.Enqueue(Value);
            }
        }



        public bool Equals(RecyclableSettingId other)
        {
            return other.Value == this.Value;
        }
        public override bool Equals(object obj)
        {
            if (obj is RecyclableSettingId other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value;
        }

    }
}
