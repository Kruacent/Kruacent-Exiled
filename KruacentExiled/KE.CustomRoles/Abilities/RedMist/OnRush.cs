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
    public class OnRush : EgoAbility
    {
        public override string Name { get; } = "OnRush";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "On Rush",
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
        public override float Cooldown { get; } = 120f;

        protected override NeedActive NeedEGOActive => NeedActive.Either;

        public const float Damage = 100;

        public const float MaxDistance = 5;

        
        private Collider[] NonAlloc = new Collider[64];

        public static readonly LayerMasks Mask = LayerMasks.Scp173Teleport | LayerMasks.Glass;
        protected override bool LaunchedAbility(Player player,EGO ego)
        {


            KELog.Debug("check weapopgn");
            

            
            if(player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509)
            {
                //show OnRushFailWeapon
                return false;
            }


            float size = 1;

            Vector3 feetposition = player.Position- (Vector3.up*player.Scale.y)/2 + player.CameraTransform.forward*.1f;
            Vector3 position = player.Position + player.CameraTransform.forward*.1f;


            DrawSphere(position, size, Color.green);
            DrawSphere(feetposition, size, Color.green);
            Vector3 direction = player.CameraTransform.forward;

            direction = direction.NormalizeIgnoreY();
            
            
            Vector3 end = position + direction * MaxDistance;
            Vector3 feetend = feetposition + direction * MaxDistance;
            DrawSphere(end, size, Color.yellow);
            DrawSphere(feetend, size, Color.yellow);
            Vector3 teleport = end;

            Vector3 wallPoint = end;




            if (Physics.Raycast(position, direction,out RaycastHit wallHit, MaxDistance, (int)Mask)
                | Physics.Raycast(feetposition, direction, out RaycastHit wallHitFeet, MaxDistance, (int)Mask))
            {
                Draw.Sphere(wallHit.point, Quaternion.identity, Vector3.one / 8f, Color.blue, 10, Player.Enumerable);
                Draw.Sphere(wallHitFeet.point, Quaternion.identity, Vector3.one / 8f, Color.blue, 10, Player.Enumerable);

                if(Vector3.Distance(wallHit.point,position) <= Vector3.Distance(wallHitFeet.point, feetposition))
                {
                    teleport = wallHit.point + direction * (-1f);
                }
                else
                {
                    teleport = wallHitFeet.point + direction * (-1f);
                }
                Vector3 directionToPoint = teleport - player.Position;
                bool behind = Vector3.Dot(direction, directionToPoint) < 0f;


                wallPoint = teleport;
                if (behind)
                {
                    teleport = Vector3.zero;
                }



            }

            int detect = Physics.OverlapCapsuleNonAlloc(position, wallPoint, size, NonAlloc, HitregUtils.DetectionMask);

            for (int i = 0;i < detect;i++)
            {
                Collider collider = NonAlloc[i];

                //KELog.Debug("hit collider ="+collider);
                
                if (collider.TryGetComponent<IDestructible>(out var destructible) &&
                    (!Linecast(position, destructible?.CenterOfMass ?? Vector3.zero, out var hitInfo, PlayerRolesUtils.AttackMask)
                    || collider == hitInfo.collider))
                {
                    Player target = Player.Get(collider);

                    if(target is null || target != player)
                    {
                        DrawSphere(collider.transform.position,.2f, Color.red);
                    }


                    if (destructible is not null)
                    {
                        PlayerStatsSystem.DamageHandlerBase handler = new GenericDamageHandler(target, player, Damage, DamageType.Scp1509, new DamageHandlerBase.CassieAnnouncement("")).Base;
                        destructible.Damage(Damage, handler, destructible.CenterOfMass);
                    }
                }
            }

            DrawSphere(teleport,.25f, Color.magenta);

            if(teleport != Vector3.zero)
            {
                player.Teleport(teleport);
            }

            return true;
        }


        private static bool Debug => MainPlugin.Configs.Debug;
        public static bool Linecast(Vector3 start,Vector3 end,out RaycastHit hit,int layerMask)
        {
            if (Debug)
            {
                //Draw.Line(start, end, Color.red, 10, Player.Enumerable);
            }
            

            
            return Physics.Linecast(start, end, out hit, layerMask);

        }

        public static void DrawSphere(Vector3 position, float size,Color color)
        {
            if (Debug)
            {
                Draw.Sphere(position, Quaternion.identity, Vector3.one * size, color, 10, Player.Enumerable);
            }
            
        }


    }
}
