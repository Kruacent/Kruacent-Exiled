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
    internal class ModelSelection
    {
        public const float BLINK_REFRESH_RATE = .3f;


        internal Model SelectedModel;
        public Primitive SelectedPrimitive;

        public static event Action<Primitive> OnChangedSelection;
        public static event Action OnUnSelect;

        public void ChangedSelectedPrim(Primitive newPrim)
        {
            if (newPrim == null) return;



            Log.Info(SelectedPrimitive == null);
            Log.Info(newPrim.GameObject.GetInstanceID() != SelectedPrimitive?.GameObject.GetInstanceID());

            if (SelectedPrimitive == null || newPrim.GameObject.GetInstanceID() != SelectedPrimitive?.GameObject.GetInstanceID())
            {
                Log.Info("selecting");
                OnChangedSelection?.Invoke(newPrim);
                SelectedPrimitive = newPrim;
            }
            else
            {
                Log.Info("unselecting");
                SelectedPrimitive = null;
                OnUnSelect?.Invoke();
            }

        }


        

    }
}
