using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Utils.API.Models
{
    internal class SelectedModel
    {
        public const float BLINK_REFRESH_RATE = .3f;


        public Primitive SelectedPrimitive;
        private Color _baseColor;
        private CoroutineHandle _handle;

        public static event Action<Primitive> OnChangedSelection;
        public static event Action OnUnSelect;

        internal SelectedModel()
        {
            
        }

        public void ChangedSelectedPrim(Primitive newPrim)
        {
            if (newPrim == null) return;


            UnSelect();

            if (SelectedPrimitive == null || newPrim != SelectedPrimitive)
            {
                OnChangedSelection?.Invoke(newPrim);
                _baseColor = newPrim.Color;
                _handle = Timing.RunCoroutine(Blink(newPrim));
            }
            else
            {
                OnUnSelect?.Invoke();
            }

        }


        private void UnSelect()
        {
            if (_handle.IsRunning)
            {

                Timing.KillCoroutines(_handle);
                SelectedPrimitive.Color = _baseColor;
            }
            SelectedPrimitive = null;
        }


        private IEnumerator<float> Blink(Primitive p)
        {
            SelectedPrimitive = p;
            Color baseColor = p.Color;
            Color baseTrans = new(baseColor.r, baseColor.g, baseColor.b, baseColor.a / 2);
            bool transparent = false;
            while (true)
            {
                if (transparent)
                {
                    p.Color = baseTrans;
                }
                else
                {
                    p.Color = baseColor;
                }
                yield return Timing.WaitForSeconds(BLINK_REFRESH_RATE);
                transparent = !transparent;
            }
        }

    }
}
