using Exiled.API.Features;
using KE.Utils.API.Features;
using KE.Utils.API.GifAnimator;
using KE.Utils.API.KETextToy;
using KruacentExiled.CustomRoles;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using MEC;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KruacentExiled.CustomRoles.Abilities
{
    public class SetPosition : KEAbilities, ICustomIcon
    {
        public override string Name { get; } = "SetPosition";

        public const string TranslationNoTarget = "SetPositionNoTarget";
        public const string TranslationTooFar = "SetPositionTooFar";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Set Position",
                    [TranslationKeyDesc] = "Select the current position for another ability",
                    [TranslationNoTarget] = "No target set",
                    [TranslationTooFar] = "Position destroyed : too far away",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Selection de position",
                    [TranslationKeyDesc] = "Selectionne la position pour une autre abilité",
                    [TranslationNoTarget] = "Pas de position mise",
                    [TranslationTooFar] = "Position détruite : trop loin",
                }
            };
        }
        
        public override float Cooldown { get; } = 5f;
        public TextImage IconName => MainPlugin.Instance.icons[Name];



        protected override bool AbilityUsed(Player player)
        {

            Vector3 position = player.Position;

            if(SetPositionPosition.TryGet(player,out var setPosition))
            {
                setPosition.Destroy();
            }
            SetPositionPosition.Create(player,position);

 

            Log.Debug("set position at " + position);
            return base.AbilityUsed(player);
        }

        



        protected override void AbilityRemoved(Player player)
        {
            if(SetPositionPosition.TryGet(player,out var position))
            {
                position.Destroy();
            }
            base.AbilityRemoved(player);
        }

        public static bool TryGetTarget(Player p, out Vector3 target)
        {
            return SetPositionPosition.TryGetTarget(p, out target);
        }

        private class SetPositionPosition
        {
            public const float RefreshRate = 1f;
            public const float MaxDistance = 25;
            private static Dictionary<Player, SetPositionPosition> SelectedTarget = new Dictionary<Player, SetPositionPosition>();
            public Player Player { get; private set; }
            public Vector3 Position { get; }

            private ClientPrimitives.ClientSidePrimitive _clientSidePrimitive = null;


            private readonly CoroutineHandle handle;

            public static SetPositionPosition Create(Player player, Vector3 position)
            {
                if (player == null) throw new ArgumentNullException(nameof(player));
                return new SetPositionPosition(player, position);
            }

            private SetPositionPosition(Player player,Vector3 position)
            {
                Position = position;
                Player = player;
                _clientSidePrimitive = new ClientPrimitives.ClientSidePrimitive(Position, Quaternion.identity, Vector3.one * .1f, PrimitiveType.Sphere, Color.magenta, AdminToys.PrimitiveFlags.Visible, player);
                _clientSidePrimitive.SpawnClientPrimitive();
                handle = Timing.RunCoroutine(CheckPosition());
                SelectedTarget.Add(Player, this);
            }



            public static bool TryGet(Player player, out SetPositionPosition position)
            {
                return SelectedTarget.TryGetValue(player, out position);
            }


            public void Destroy()
            {
                _clientSidePrimitive.DestroyClientPrimitive();
                SelectedTarget.Remove(Player);
                Timing.KillCoroutines(handle);
                Player = null;
            }

            private bool ToDestroy = false;
            private IEnumerator<float> CheckPosition()
            {
                while(Player != null && !ToDestroy)
                {
                    float distance = (Position - Player.Position).sqrMagnitude;

                    yield return Timing.WaitForSeconds(RefreshRate);

                    if (distance >= MaxDistance * MaxDistance)
                    {
                        KELog.Debug("SetPosition to destroy "+ Position);
                        ToDestroy = true;
                    }

                    
                }

                if (ToDestroy)
                {

                    TranslationFeed(Player, TranslationTooFar);
                    KELog.Debug($"SetPosition ({Position}) destroyed : too far");
                    Destroy();
                }
            }


            public static bool TryGetTarget(Player player, out Vector3 target)
            {
                target = Vector3.zero;
                bool result = SelectedTarget.TryGetValue(player, out SetPositionPosition position);
                if (result)
                {
                    target = position.Position;
                }
                
                return result;
            }
        }



    }
}
