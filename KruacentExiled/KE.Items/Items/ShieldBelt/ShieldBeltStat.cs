using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Items.Items.ShieldBelt
{
    public class ShieldBeltStat : MonoBehaviour
    {
        public const float MaxCharge = 220;
        public const float RechargeRatePerS = 13;
        public const float TimeBroken = 50;
        public const float Base = 20;
        public static readonly float MaxSize = 2;
        public static readonly float MinSize = 1.5f;

        public float CurrentCharge => currentCharge;

        private float currentCharge;
        private float timeRemaining;
        private bool recharging = false;

        private Player player;
        private Primitive primitive;
        public void RechargeTick()
        {

            if (timeRemaining <= 0 && recharging)
            {
                Log.Debug("recharged");
                currentCharge = 20;
                recharging = false;
            }

            if (currentCharge <= 0)
            {
                Break();
            }

            if (!(primitive is null))
            {
                float percent = Mathf.Clamp01(currentCharge / MaxCharge);
                primitive.Scale = Mathf.Lerp(MinSize, MaxSize, percent)*Vector3.one;

            }


            if (!recharging)
            {
                if (!primitive.Visible)
                {
                    primitive.Visible = true;
                }

                if (currentCharge != MaxCharge)
                {

                    float tempcharge = currentCharge + RechargeRatePerS * Time.deltaTime;
                    currentCharge = Mathf.Clamp(tempcharge, 0, MaxCharge);
                }

            }
            else
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0)
                {
                    timeRemaining = 0;
                }
                if (primitive.Visible)
                {
                    primitive.Visible = false;
                }
            }


        }


        public float Damage(float damage)
        {


            currentCharge = Mathf.Clamp(currentCharge - damage, 0, MaxCharge);
            Log.Debug("cur=" + currentCharge);
            Log.Debug("time=" + timeRemaining);

            if (IsActive)
            {
                return 0;
            }
            else
            {
                return damage;
            }

        }


        public void Break()
        {

            if (!recharging)
            {
                Log.Debug("breakign");
                timeRemaining = TimeBroken;
                currentCharge = 0;
                recharging = true;
                player.PlayShieldBreakSound();
            }

        }


        public bool IsActive
        {
            get
            {
                return currentCharge > 0;
            }
        }
        public bool IsRecharging
        {
            get
            {
                return recharging;
            }
        }
        private Primitive CreatePrimitive(Player player)
        {
            Primitive prim = Primitive.Create(null, null, null, false);
            prim.Collidable = false;
            prim.Visible = true;
            prim.Transform.parent = player.ReferenceHub.transform;
            prim.Transform.localPosition = Vector3.zero;
            prim.Scale = MaxSize*Vector3.one;
            prim.Color = new Color32(50, 50, 50, 50);
            prim.MovementSmoothing = 0;
            prim.Spawn();

            


            return prim;
        }
        public void Awake()
        {
            player = Player.Get(transform.root.gameObject);
            primitive = CreatePrimitive(player);
            currentCharge = Base;
            timeRemaining = 0;
        }


        public void Destroy()
        {
            Log.Debug($"destroying {this}");
            Destroy(this);
        }

        private void OnDestroy()
        {
            primitive.Destroy();
            primitive = null;
        }

        public void Update()
        {
            RechargeTick();
        }
    }
}
