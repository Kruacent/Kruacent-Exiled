using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.CreditTags.Features;
using InventorySystem.Items.MicroHID.Modules;
using KE.CustomRoles.API.Features;
using KE.CustomRoles.CR.MTF.RedMist;
using KE.Utils.API.Features;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
namespace KE.CustomRoles.Abilities.RedMist
{
    public class ForwardSlash : KEAbilities
    {
        public override string Name { get; } = "ForwardSlash";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Forward Slash",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                    ["ForwardSlashFailEGO"] = "You need to manifest your E.G.O. first",
                    ["ForwardSlashFailWeapon"] = "You need your weapon",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                    ["ForwardSlashFailEGO"] = "todo",
                    ["ForwardSlashFailWeapon"] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;
        public const float Damage = 100;

        public const float MaxDistance = 15;

        private RaycastHit[] NonAlloc = new RaycastHit[16];
        protected override bool AbilityUsed(Player player)
        {

            
            if (!player.ReferenceHub.gameObject.TryGetComponent<EGO>(out var ego))
            {
                return false;
            }



            if (!ego.Active)
            {
                //show ForwardSlashFailEGO
                return false;
            }
            KELog.Debug("check weapopgn");
            

            
            if(player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509)
            {
                //show ForwardSlashFailWeapon
                return false;
            }


            float size = 1;

            Vector3 position = player.Position + player.CameraTransform.forward;

            int detect = Physics.SphereCastNonAlloc(position, size, player.CameraTransform.forward, NonAlloc, MaxDistance, HitregUtils.DetectionMask);

            Draw.Sphere(position, Quaternion.identity, Vector3.one * size * 2f, Color.green, 10, Player.Enumerable);
            Vector3 direction = player.CameraTransform.forward;
            direction.Normalize();
            Vector3 end = position + direction * MaxDistance;
            Draw.Sphere(end, Quaternion.identity, Vector3.one * size * 2f, Color.yellow, 10,Player.Enumerable);

            for (int i = 0;i < detect;i++)
            {

                Collider collider = NonAlloc[i].collider;

                KELog.Debug("hit collider ="+collider);
                if (collider.TryGetComponent<IDestructible>(out var destructible) &&
                    (!Physics.Linecast(position, destructible.CenterOfMass, out var hitInfo, PlayerRolesUtils.AttackMask)
                    || collider == hitInfo.collider))
                {
                    Player target = Player.Get(collider);


                    HitboxIdentity.IsDamageable(ego.Hub, target.ReferenceHub);
                    destructible.Damage(Damage, new CustomDamageHandler(target, player, Damage, DamageType.Scp1509), destructible.CenterOfMass);
                }


            }
            



            return base.AbilityUsed(player);
        }


    }
}
