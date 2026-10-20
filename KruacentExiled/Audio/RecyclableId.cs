using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using KE.Utils.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Audio
{
    public sealed class RecyclableId : IEquatable<RecyclableId>
    {
        public const int MinThreshold = 16;
        public readonly int Value;

        private static Queue<int> FreeIds = new Queue<int>();
        private static int _autoIncrement;

        public RecyclableId()
        {
            int num = MinThreshold;
            int value;
            if (FreeIds.Count >= num)
            {
                value = FreeIds.Dequeue();
            }
            else
            {
                value = _autoIncrement++;
            }
            Value = value;

        }


        public void Destroy()
        {
            if (Value != 0)
            {
                FreeIds.Enqueue(Value);
            }
        }



        public bool Equals(RecyclableId other)
        {
            return other.Value == Value;
        }
        public override bool Equals(object obj)
        {
            if (obj is RecyclableId other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
