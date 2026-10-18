using KE.Utils.API;
using KE.Utils.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled.Map.Surface.SupplyDrops.SupplyDropTimeModifier
{
    internal abstract class SupplyDropTimeModifierBase
    {

        protected abstract void SubscribeEvents();
        protected abstract void UnsubscribeEvents();


        protected abstract TimeSpan TimeAdded { get; }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if(value == _isActive)
                {
                    return;
                }


                if(value && !_isActive)
                {
                    SupplyDrop.ChangeTime(TimeAdded);
                }

                if(!value && _isActive)
                {
                    SupplyDrop.ChangeTime(-TimeAdded);
                }


            }
        }


        private static List<SupplyDropTimeModifierBase> _modifiers;


        public static void InternalSubscribeEvents()
        {
            _modifiers = GetAllSupplyDropModifier();
            foreach(SupplyDropTimeModifierBase modifierBase in _modifiers)
            {
                modifierBase.SubscribeEvents();
            }
        }
        public static void InternalUnsubscribeEvents()
        {
            foreach (SupplyDropTimeModifierBase modifierBase in _modifiers)
            {
                modifierBase.UnsubscribeEvents();
            }

        }

        private static List<SupplyDropTimeModifierBase> GetAllSupplyDropModifier()
        {
            return ReflectionHelper.GetObjects<SupplyDropTimeModifierBase>().ToList();
        }
    }
}
