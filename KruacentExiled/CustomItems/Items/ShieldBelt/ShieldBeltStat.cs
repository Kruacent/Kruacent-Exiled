using AdminToys;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using KE.Utils.API.Features.Models;
using KruacentExiled.Audio;
using Mirror;
using ProjectMER.Commands.Modifying.Position;
using ProjectMER.Commands.Modifying.Rotation;
using ProjectMER.Commands.Modifying.Scale;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Color = UnityEngine.Color;

namespace KruacentExiled.CustomItems.Items.ShieldBelt
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


        public const string NameReset = "Shield_Reset";
        public void RechargeTick()
        {

            if (timeRemaining <= 0 && recharging)
            {
                Log.Debug("recharged");
                currentCharge = 20;
                AudioHandler.Instance.PlayToAll(SoundType.Noise, NameReset, player.GameObject, 10);
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
                string absorbnoise = nameAbsorb.RandomItem();
                try
                {
                    AudioHandler.Instance.PlayToAll(SoundType.Noise, absorbnoise, player.GameObject, 10);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
                
                return 0;
            }
            else
            {
                return damage;
            }

        }

        private static readonly string[] nameAbsorb = new string[] { "Shield_Absorb_1b", "Shield_Absorb_1c", "Shield_Absorb_1d" };
        private static readonly string[] nameBroken = new string[] { "Shield_Broken_1a", "Shield_Broken_1c" };
        public void Break()
        {

            if (!recharging)
            {
                Log.Debug("breakign");
                timeRemaining = TimeBroken;
                currentCharge = 0;
                recharging = true;
                string breaknoise = nameBroken.RandomItem();

                AudioHandler.Instance.PlayToAll(SoundType.Noise, breaknoise, player.GameObject, 10);
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
            

            //MirrorExtensions.SendFakeSyncVar<PrimitiveFlags>(player, prim.Base.netIdentity, typeof(PrimitiveObjectToy), "PrimitiveFlags", PrimitiveFlags.None);


            return prim;
        }

        public void Awake()
        {
            try
            {
                player = Player.Get(transform.root.gameObject);

                primitive = CreatePrimitive(player);
                
                currentCharge = Base;
                timeRemaining = 0;
                primitive.Spawn();
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            
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
