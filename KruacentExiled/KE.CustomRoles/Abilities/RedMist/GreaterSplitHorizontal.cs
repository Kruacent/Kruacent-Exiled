using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp1509;
using InventorySystem.Items.MicroHID.Modules;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.Utils.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;
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
                ev.IsAllowed = false;
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
        private static bool CheckPoint(Vector3 point, Vector3 center, Vector3 direction)
        {
            Vector2 position = new Vector2(point.x - center.x, point.z - center.z);

            float sqrMag = position.sqrMagnitude;

            if (sqrMag <= 0.0001f)
                return false;

            float radius = Mathf.Sqrt(sqrMag);
            if (radius > Size)
                return false;

            Vector2 dir = new Vector2(direction.x, direction.z);

            if (dir.sqrMagnitude <= 0.0001f)
                return false;

            dir /= Mathf.Sqrt(dir.sqrMagnitude); // safe normalize

            Vector2 pointDir = position / radius; // safer than normalized

            float halfAngle = 30f; // 60 / 2
            float cosThreshold = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

            float dot = Vector2.Dot(dir, pointDir);
            float rad = halfAngle * Mathf.Deg2Rad;

            Vector2 leftDir = new Vector2(dir.x * Mathf.Cos(rad) - dir.y * Mathf.Sin(rad), dir.x * Mathf.Sin(rad) + dir.y * Mathf.Cos(rad)); 
            Vector2 rightDir = new Vector2(dir.x * Mathf.Cos(-rad) - dir.y * Mathf.Sin(-rad), dir.x * Mathf.Sin(-rad) + dir.y * Mathf.Cos(-rad));

            Vector3 leftPoint = center + new Vector3(leftDir.x, 0, leftDir.y) * Size;
            Vector3 rightPoint = center + new Vector3(rightDir.x, 0, rightDir.y) * Size;
            KELog.Debug("center =" + center);
            KELog.Debug("left =" + leftPoint);
            KELog.Debug("rightPoint =" + rightPoint);

            DrawSphere(leftPoint, 0.2f, Color.green);
            DrawSphere(rightPoint, 0.2f, Color.red);
            if (float.IsNaN(dot))
            {
                Log.Error("NaN detected in CheckPoint");
                return false;
            }

            DrawSphere(center + new Vector3(position.x, 0, position.y), .1f, Color.cyan);

            return dot >= cosThreshold;
        }

        private static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask) => OnRush.Linecast(start, end, out hit, layerMask);

        private static void DrawSphere(Vector3 position, float size, Color color) => OnRush.DrawSphere(position, size, color);



        private class AttackGreaterSplit : MonoBehaviour
        {
            private Player player;
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



                if (time <= 0 || attacking)
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


            private bool InSphere(Vector3 center,Vector3 position,float radius)
            {
                float sqrDistance = (position - center).sqrMagnitude;
                float sqrRadius = radius * radius;

                return sqrDistance <= sqrRadius;
            }

            private void LaunchedAttack(float remainingTime)
            {
                
                if (player == null || player.GameObject == null) return;
                KELog.Debug("remainign time=" + remainingTime);
               
                Vector3 direction = player.Transform.forward;
                
                direction = direction.NormalizeIgnoreY();

                //NorthwoodLib.Pools.HashSetPool<Player>.Shared.Rent();
                HashSet<Player> toDamage = new();
                Vector3 position = player.Position;

                foreach (Player target in Player.List)
                {

                    Vector3 targetPosition = target.Position;
                    KELog.Debug("sphere");

                    if(player == target)
                    {
                        continue;
                    }

                    if (!InSphere(position, targetPosition, 5))
                    {
                        continue;
                    }

                    //KELog.Debug("fornt");
                    if (!CheckPoint(targetPosition, position, direction))
                    {
                        continue;
                    }

                    KELog.Debug("linecast");
                    if (!Linecast(position, targetPosition, out var hitInfo, PlayerRolesUtils.AttackMask))
                    {
                        continue;
                    }

                    KELog.Debug("add damage");
                    //DrawSphere(targetPosition, .2f, Color.red);
                    toDamage.Add(target);


                }


                foreach (Player target in toDamage)
                {
                    KELog.Debug("damaging" + target.Nickname);

                    //target.Hurt(Damage, DamageType.Scp1509);
                }
                //NorthwoodLib.Pools.HashSetPool<Player>.Shared.Return(toDamage);
            }

        }

    }
}
