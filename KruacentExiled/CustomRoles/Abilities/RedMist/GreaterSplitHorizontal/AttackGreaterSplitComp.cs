using CustomPlayerEffects;
using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace KruacentExiled.CustomRoles.Abilities.RedMist.GreaterSplitHorizontal
{
    public class AttackGreaterSplitComp : MonoBehaviour
    {

        private static bool Debug => MainPlugin.Instance.Config.Debug;
        private Player player;
        private bool currentUsing;
        private bool attacking;
        private GreaterSplitHorizontal ability;
        public const float TimeAttack = 3;
        private readonly float StartTimeAttack;
        private readonly float EndTimeAttack;

        private const float HighPowerRequirement = .1f;
        private const float MedPowerRequirement = .3f;


        public AttackGreaterSplitComp()
        {

            StartTimeAttack = Mathf.Abs(TimeAttack) / 2f;
            EndTimeAttack = -StartTimeAttack;

        }
        private static HintPosition Position = new DebugPosition();
        public void Init(Player player, GreaterSplitHorizontal ability)
        {
            this.player = player;
            this.ability = ability;
            currentUsing = false;
            attacking = false;

            if (Debug)
            {
                DisplayHandler.Instance.CreateAuto(player, (args) => GetDebug(), Position.HintPlacement, HintSyncSpeed.Fastest);
            }
        }

        private void OnDestroy()
        {

            if (player != null)
            {
                DisplayHandler.Instance.RemoveHint(player, Position.HintPlacement);
            }

        }


        private string GetDebug()
        {
            float power = GetPower(time);
            return "time = " + time + "\npower = " + power + '(' + GetPowerDebug(power) + ')';

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
            catch (Exception e)
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


            if (power <= StartTimeAttack * HighPowerRequirement)
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

            if (Debug)
            {
                CheckPoint(position, position, direction, range, angle);
                if (checkBack)
                {
                    CheckPoint(position, position, directionBack, range, angle);
                }
            }


            foreach (Player target in Player.List)
            {

                Vector3 targetPosition = target.Position;
                KELog.Debug("fornt");

                

                if (!CheckPoint(targetPosition, position, direction, range, angle) & !(checkBack && CheckPoint(targetPosition, position, directionBack, range, angle)))
                {
                    continue;
                }
                KELog.Debug("player " + target.Nickname);
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


                target.Hurt(GreaterSplitHorizontal.Damage, DamageType.Scp1509);

            }

        }

        public const float height = 1f;
        private bool CheckPoint(Vector3 point, Vector3 center, Vector3 direction, float size, float halfAngle)
        {
            Vector2 position = new Vector2(point.x - center.x, point.z - center.z);
            Vector3 playerPosition = player.Position;
            float sqrMag = position.sqrMagnitude;

            float rad = halfAngle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(direction.x, direction.z);

            dir /= Mathf.Sqrt(dir.sqrMagnitude);
            Vector2 leftDir = new Vector2(dir.x * Mathf.Cos(rad) - dir.y * Mathf.Sin(rad), dir.x * Mathf.Sin(rad) + dir.y * Mathf.Cos(rad));
            Vector2 rightDir = new Vector2(dir.x * Mathf.Cos(-rad) - dir.y * Mathf.Sin(-rad), dir.x * Mathf.Sin(-rad) + dir.y * Mathf.Cos(-rad));

            Vector3 leftPoint = center + new Vector3(leftDir.x, 0, leftDir.y) * size;
            Vector3 rightPoint = center + new Vector3(rightDir.x, 0, rightDir.y) * size;

            Vector3 frontPoint = playerPosition + direction * size;
            if (Debug)
            {
                DrawableLines.IsDebugModeEnabled = true;
                DrawableLines.GenerateLine(10, Color.yellow, playerPosition, leftPoint, frontPoint, rightPoint, player.Position);
            }
            
            if (sqrMag <= 0.0001f)
                return false;

            float radius = Mathf.Sqrt(sqrMag);
            if (radius > size)
                return false;

            

            Vector2 pointDir = position / radius;

            float cosThreshold = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

            float dot = Vector2.Dot(dir, pointDir);


            


            KELog.Debug("center =" + center);
            KELog.Debug("left =" + leftPoint);
            KELog.Debug("rightPoint =" + rightPoint);



            return dot >= cosThreshold && point.y < center.y + height && point.y > center.y - height;
        }

    }

}
