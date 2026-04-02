using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CreditTags.Features;
using Exiled.Events.EventArgs.Scp1509;
using HintServiceMeow.Core.Enum;
using InventorySystem.Items.MicroHID.Modules;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.CustomRoles.CR.SCP;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace KE.CustomRoles.Abilities.RedMist.GreaterSplitHorizontal
{
    public class GreaterSplitHorizontal : EgoAbility
    {
        public const string FailEgo = "GreaterSplitFailEGO";
        public const string FailWeapon = "GreaterSplitFailWeapon";


        public override string Name { get; } = "GreaterSplitHorizontal";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Greater Split : Horizontal",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                    [FailEgo] = "You need to manifest your E.G.O. first",
                    [FailWeapon] = "You need your weapon",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                    [FailEgo] = "todo",
                    [FailWeapon] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;

        protected override NeedActive NeedEGOActive => NeedActive.NeedActive;

        public const float Damage = 200;

        public const float MaxDistance = 5;

        public static readonly LayerMasks Mask = LayerMasks.Scp173Teleport | LayerMasks.Glass;
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
            if (!ev.IsAllowed) return;
            if (player.GameObject.TryGetComponent<AttackGreaterSplit>(out var comp))
            {
                ev.IsAllowed = !comp.OnTriggerAttack();

            }
            ev.Scp1509.NextResurrectTime = float.MaxValue;
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
        
        private class DebugPosition : HintPosition
        {
            public override float Xposition => 800;
            public override float Yposition => 400;
            public override HintAlignment HintAlignment => HintAlignment.Center;
            public override string Name => "DebugGreaterSplit";
        }

        private class AttackGreaterSplit : MonoBehaviour
        {
            private Player player;
            private bool currentUsing;
            private bool attacking;
            private GreaterSplitHorizontal ability;
            public const float TimeAttack = 10;
            private readonly float StartTimeAttack;
            private readonly float EndTimeAttack;

            private const float HighPowerRequirement = .1f;
            private const float MedPowerRequirement = .3f;


            public AttackGreaterSplit()
            {

                StartTimeAttack = Mathf.Abs(TimeAttack) / 2f;
                EndTimeAttack = -StartTimeAttack;

            }
            private static HintPosition Position = new DebugPosition();
            public void Init(Player player,GreaterSplitHorizontal ability)
            {
                this.player = player;
                this.ability = ability;
                currentUsing = false;
                attacking = false;

                if (MainPlugin.Instance.Config.Debug)
                {
                    DisplayHandler.Instance.CreateAuto(player, (args) => GetDebug(), Position.HintPlacement, HintSyncSpeed.Fast);
                }
            }

            private void OnDestroy()
            {

                if(player != null)
                {
                    DisplayHandler.Instance.RemoveHint(player, Position.HintPlacement);
                }

            }


            private string GetDebug()
            {
                float power = GetPower(time);
                return "time = " + time + "\npower = " + power + '('+ GetPowerDebug(power) + ')'; 

            }

            private string GetPowerDebug(float power)
            {
                if (power <= StartTimeAttack * HighPowerRequirement)
                {
                    return "high";
                }
                else if (power <= StartTimeAttack * MedPowerRequirement)
                {
                    return "medium";
                }
                else
                {
                    return "low";
                }
            }


            private float time;
            public void StartAttack()
            {
                currentUsing = true;
                time = StartTimeAttack;
            }


            public bool OnTriggerAttack()
            {
                if (currentUsing)
                {
                    attacking = true;
                    
                }
                return attacking;
            }


            private void Update()
            {
                try
                {
                    UpdateAttack();
                }
                catch(Exception e)
                {
                    Log.Error(e);
                }
            }

            private void UpdateAttack()
            {
                if (!currentUsing) return;


                time -= Timing.DeltaTime;



                if (time <= EndTimeAttack || attacking)
                {
                    if (ability.Check(player))
                    {
                        float power = GetPower(time);


                        LaunchedAttack(power);
                    }
                    attacking = false;
                    currentUsing = false;
                }
            }


            public float GetPower(float time)
            {
                return Mathf.Abs(time);
            }

            private void LaunchedAttack(float power)
            {
                
                if (player == null || player.GameObject == null) return;
                KELog.Debug($"power (0~{StartTimeAttack})= {power}");

                float angle = 1f;
                float range = 1f;
                bool checkBack = false;


                if(power <= StartTimeAttack * HighPowerRequirement)
                {
                    angle = 60f;
                    range = 7f;
                    checkBack = true;
                    KELog.Debug("high power");
                }
                else if (power <= StartTimeAttack * MedPowerRequirement)
                {
                    angle = 60f;
                    range = 7f;
                    KELog.Debug("med power");
                }
                else
                {
                    angle = 30f;
                    range = 5f;
                    KELog.Debug("low power");
                }


                


                
                Vector3 direction = player.Transform.forward.NormalizeIgnoreY();
                Vector3 directionBack = -player.Transform.forward.NormalizeIgnoreY();

                
                
                Vector3 position = player.Position;

                foreach (Player target in Player.List)
                {

                    Vector3 targetPosition = target.Position;
                    KELog.Debug("fornt");

                    //!checkBack || CheckPoint(targetPosition,position, directionBack,range,angle)


                    if(!CheckPoint(targetPosition, position, direction, range, angle) & !(checkBack && CheckPoint(targetPosition, position, directionBack, range, angle)))
                    {
                        continue;
                    }
                    KELog.Debug("player "+ target.Nickname);
                    if (player == target)
                    {
                        continue;
                    }                   
                    

                    KELog.Debug("linecast");
                    if (Physics.Linecast(position, targetPosition, out var hitInfo, PlayerRolesUtils.AttackMask))
                    {
                        continue;
                    }

                    KELog.Debug("add damage");


                    target.Hurt(Damage, DamageType.Scp1509);

                }

            }

            public const float height = 1f;
            private bool CheckPoint(Vector3 point, Vector3 center, Vector3 direction, float size, float halfAngle)
            {
                Vector2 position = new Vector2(point.x - center.x, point.z - center.z);

                float sqrMag = position.sqrMagnitude;

                if (sqrMag <= 0.0001f)
                    return false;

                float radius = Mathf.Sqrt(sqrMag);
                if (radius > size)
                    return false;

                Vector2 dir = new Vector2(direction.x, direction.z);

                if (dir.sqrMagnitude <= 0.0001f)
                    return false;

                dir /= Mathf.Sqrt(dir.sqrMagnitude);

                Vector2 pointDir = position / radius;

                float cosThreshold = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

                float dot = Vector2.Dot(dir, pointDir);
                float rad = halfAngle * Mathf.Deg2Rad;

                Vector2 leftDir = new Vector2(dir.x * Mathf.Cos(rad) - dir.y * Mathf.Sin(rad), dir.x * Mathf.Sin(rad) + dir.y * Mathf.Cos(rad));
                Vector2 rightDir = new Vector2(dir.x * Mathf.Cos(-rad) - dir.y * Mathf.Sin(-rad), dir.x * Mathf.Sin(-rad) + dir.y * Mathf.Cos(-rad));

                Vector3 leftPoint = center + new Vector3(leftDir.x, 0, leftDir.y) * size;
                Vector3 rightPoint = center + new Vector3(rightDir.x, 0, rightDir.y) * size;

                Vector3 frontPoint = player.Position + direction * size;

                DrawableLines.IsDebugModeEnabled = true;
                DrawableLines.GenerateLine(10, Color.yellow,player.Position, leftPoint, frontPoint, rightPoint,player.Position);


                KELog.Debug("center =" + center);
                KELog.Debug("left =" + leftPoint);
                KELog.Debug("rightPoint =" + rightPoint);



                return dot >= cosThreshold && point.y < center.y + height && point.y > center.y - height;
            }

        }

    }
}
