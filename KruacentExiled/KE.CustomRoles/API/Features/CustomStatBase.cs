using Exiled.API.Features;
using KE.Utils.API.Displays.DisplayMeow;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.API.Features
{
    public abstract class CustomStatBase : MonoBehaviour
    {
        public abstract float CurValue { get; set; }

        public abstract float MinValue { get; }

        public abstract float MaxValue { get; set; }
        public abstract string Name { get; }

        public float NormalizedValue
        {
            get
            {
                if (MinValue != MaxValue)
                {
                    return (CurValue - MinValue) / (MaxValue - MinValue);
                }

                return 0f;
            }
        }

        public ReferenceHub Hub { get; set; }

        public virtual void AddAmount(float amount)
        {
            CurValue = Mathf.Clamp(CurValue + amount, MinValue, MaxValue);
        }

        public  void AddAmount(float amount, float percentageCap)
        {
            float max = MaxValue * Mathf.Clamp01(percentageCap);
            float value = CurValue + amount;
            AddAmount(value);
        }


        public virtual void FixedUpdate()
        {
            string text = Name + "\n" + Math.Floor(CurValue) + "/" + MaxValue;
            DisplayHandler.Instance.AddHint(MainPlugin.RightHPbars, Player.Get(Hub), text, Timing.WaitForOneFrame);
        }

        public virtual void ClassChanged()
        {
        }

        public virtual void Destroy()
        {
            Destroy(gameObject.GetComponent<CustomStatBase>());

        }

        public virtual void Init()
        {
            Hub = ReferenceHub.GetHub(gameObject);
        }


    }
}
