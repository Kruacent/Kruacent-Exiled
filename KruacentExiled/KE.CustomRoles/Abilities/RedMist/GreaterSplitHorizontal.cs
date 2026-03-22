using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Scp1509;
using InventorySystem.Items.MicroHID.Modules;
using KE.CustomRoles.API.Interfaces.Ability;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.Utils.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
namespace KE.CustomRoles.Abilities.RedMist
{
    public class GreaterSplitHorizontal : EgoAbility
    {
        public const string FailEgo = "OnRushFailEGO";
        public const string FailWeapon = "OnRushFailWeapon";


        public override string Name { get; } = "GreaterSplitHorizontal";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Greater Split : Horizontal",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                    ["OnRushFailEGO"] = "You need to manifest your E.G.O. first",
                    ["OnRushFailWeapon"] = "You need your weapon",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                    ["OnRushFailEGO"] = "todo",
                    ["OnRushFailWeapon"] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;

        protected override NeedActive NeedEGOActive => NeedActive.Either; // active

        public const float Damage = 200;

        public const float MaxDistance = 5;

        
        

        public static readonly LayerMasks Mask = LayerMasks.Scp173Teleport | LayerMasks.Glass;

        public const float Size = 5;

        


        protected override void AbilityAdded(Player player)
        {

            if (!player.GameObject.TryGetComponent<AttackGreaterSplit>(out var comp))
            {
                comp = player.GameObject.AddComponent<AttackGreaterSplit>();
            }
            comp.Init(player, this);
            

            base.AbilityAdded(player);
        }

        protected override void AbilityRemoved(Player player)
        {
            if (player.GameObject.TryGetComponent<AttackGreaterSplit>(out var comp))
            {
                UnityEngine.Object.Destroy(comp);
            }
            base.AbilityRemoved(player);
        }


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp1509.TriggeringAttack += OnTriggeringAttack;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp1509.TriggeringAttack -= OnTriggeringAttack;
            base.UnsubscribeEvents();
        }





        private void OnTriggeringAttack(TriggeringAttackEventArgs ev)
        {
            Player player = ev.Player;
            if (!Check(player)) return;
            if (player.GameObject.TryGetComponent<AttackGreaterSplit>(out var comp))
            {
                comp.OnTriggerAttack();
            }

        }


        
        

        protected override bool LaunchedAbility(Player player,EGO ego)
        {



            KELog.Debug("check weapopgn");
            

            
            if(player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509)
            {
                ShowEffectHint(player, FailWeapon);
                return false;
            }


            if (player.GameObject.TryGetComponent<AttackGreaterSplit>(out var comp))
            {
                comp.StartAttack();
            }


            return true;
        }
        private static bool CheckPoint(Vector3 point,Vector3 center, Vector3 direction)
        {


            Vector2 position = new Vector2(point.x - center.x, point.z - center.z);

            float radius = position.magnitude;
            if (radius > Size)
                return false;

            Vector2 dir = new Vector2(direction.x, direction.z).normalized;
            Vector2 pointDir = position.normalized;
            float sectorAngle = 60f;
            float halfAngle = sectorAngle * 0.5f;
            float cosThreshold = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

            DrawSphere(position, .1f, Color.cyan);

            return Vector2.Dot(dir, pointDir) >= cosThreshold;
        }
        
        private static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask) => OnRush.Linecast(start, end, out hit, layerMask);

        private static void DrawSphere(Vector3 position, float size, Color color) => OnRush.DrawSphere(position, size, color);



        private class AttackGreaterSplit : MonoBehaviour
        {
            private Player player;
            private readonly Collider[] NonAlloc = new Collider[64];
            private bool currentUsing;
            private bool attacking;
            private GreaterSplitHorizontal ability;
            public void Init(Player player,GreaterSplitHorizontal ability)
            {
                this.player = player;
                this.ability = ability;
                currentUsing = false;
                attacking = false;
            }

            private float time;
            public void StartAttack()
            {
                currentUsing = true;
                time = 10;
            }


            public void OnTriggerAttack()
            {
                if (currentUsing)
                {
                    attacking = true;
                }
            }


            private void Update()
            {
                if (!currentUsing) return;


                time -= Timing.DeltaTime;

                if(time <= 0 || attacking)
                {
                    if (ability.Check(player))
                    {
                        try
                        {
                            LaunchedAttack(time);
                        }
                        catch(Exception e)
                        {
                            Log.Error(e);
                        }
                        
                    }
                    attacking = false;
                    currentUsing = false;
                }
            }



            private void LaunchedAttack(float remainingTime)
            {

                KELog.Debug("remainign time=" + remainingTime);

                Vector3 feetposition = player.Position - (Vector3.up * player.Scale.y) / 2 + player.CameraTransform.forward * .1f;


                Vector3 direction = player.CameraTransform.forward;

                direction = direction.NormalizeIgnoreY();

                DrawSphere(feetposition, Size, Color.yellow);



                int detect = Physics.OverlapSphereNonAlloc(feetposition, Size, NonAlloc, HitregUtils.DetectionMask);
                KELog.Debug("detect=" + detect);

                Collider collider;
                for (int i = 0; i < detect; i++)
                {
                    collider = NonAlloc[i];

                    if(collider == null)
                    {
                        continue;
                    }

                    if (!CheckPoint(collider.transform.position, feetposition, direction))
                    {
                        continue;
                    }


                    if (!collider.TryGetComponent<IDestructible>(out var destructible) || destructible == null)
                    {
                        continue;
                    }

                    if (Linecast(feetposition, destructible.CenterOfMass, out var hitInfo, PlayerRolesUtils.AttackMask)
                        && collider != hitInfo.collider)
                    {
                        continue;
                    }
                                        
                    Player target = Player.Get(collider);

                    if (target != player)
                    {
                        DrawSphere(collider.transform.position, .2f, Color.red);
                    }

                    if (target == null || target == player)
                    {
                        continue;
                    }
                        

                    if (destructible is not null)
                    {
                        PlayerStatsSystem.DamageHandlerBase handler = new GenericDamageHandler(target, player, Damage, DamageType.Scp1509, new DamageHandlerBase.CassieAnnouncement("")).Base;
                        destructible.Damage(Damage, handler, destructible.CenterOfMass);
                    }
                }

            }
        }

    }
}
