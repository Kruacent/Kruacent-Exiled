using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using InventorySystem.Items.MicroHID.Modules;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.Utils.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;

namespace KE.CustomRoles.Abilities.RedMist
{
    public class Spear : EgoAbility
    {
        public override string Name { get; } = "Spear";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Spear",
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

        protected override NeedActive NeedEGOActive => NeedActive.Either;

        public const float Damage = 200;

        public const float MaxDistance = 5;


        private Collider[] NonAlloc = new Collider[64];

        public static readonly LayerMasks Mask = LayerMasks.Scp173Teleport | LayerMasks.Glass;

        public const float Size = 5;
        protected override bool LaunchedAbility(Player player, EGO ego)
        {


            KELog.Debug("check weapopgn");



            if (player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509)
            {
                //show OnRushFailWeapon
                return false;
            }


            Vector3 feetposition = player.Position - (Vector3.up * player.Scale.y) / 2 + player.CameraTransform.forward * .1f;


            Vector3 direction = player.CameraTransform.forward;

            direction = direction.NormalizeIgnoreY();

            DrawSphere(feetposition, Size, Color.yellow);



            int detect = Physics.OverlapSphereNonAlloc(feetposition, Size, NonAlloc, HitregUtils.DetectionMask);
            Collider collider;
            for (int i = 0; i < detect; i++)
            {
                collider = NonAlloc[i];

                if (!CheckPoint(collider.transform.position, feetposition, direction))
                {
                    continue;
                }


                if (collider.TryGetComponent<IDestructible>(out var destructible) &&
                    (!Linecast(feetposition, destructible?.CenterOfMass ?? Vector3.zero, out var hitInfo, PlayerRolesUtils.AttackMask)
                    || collider == hitInfo.collider))
                {
                    Player target = Player.Get(collider);

                    if (target is null || target != player)
                    {
                        DrawSphere(collider.transform.position, .2f, Color.red);
                    }


                    if (destructible is not null)
                    {
                        PlayerStatsSystem.DamageHandlerBase handler = new GenericDamageHandler(target, player, Damage, DamageType.Scp1509, new DamageHandlerBase.CassieAnnouncement("")).Base;
                        destructible.Damage(Damage, handler, destructible.CenterOfMass);
                    }
                }
            }
            return true;
        }
        private static bool CheckPoint(Vector3 point, Vector3 center, Vector3 direction)
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

        private bool Linecast(Vector3 start, Vector3 end, out RaycastHit hit, int layerMask) => OnRush.Linecast(start, end, out hit, layerMask);

        private static void DrawSphere(Vector3 position, float size, Color color) => OnRush.DrawSphere(position, size, color);


    }
}